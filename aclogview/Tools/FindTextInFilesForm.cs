using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using aclogview.Properties;
using System.Text;

namespace aclogview
{
    public partial class FindTextInFilesForm : Form
    {
        public FindTextInFilesForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            txtSearchPathRoot.Text = Settings.Default.FindTextInFilesRoot;

            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Columns[0].ValueType = typeof(int);
            dataGridView1.Columns[1].ValueType = typeof(int);

            // Center to our owner, if we have one
            if (Owner != null)
                Location = new Point(Owner.Location.X + Owner.Width / 2 - Width / 2, Owner.Location.Y + Owner.Height / 2 - Height / 2);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            searchAborted = true;
            Settings.Default.FindTextInFilesRoot = txtSearchPathRoot.Text;
            base.OnClosing(e);
        }

        private void btnChangeSearchPathRoot_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog openFolder = new FolderBrowserDialog())
            {
                if (openFolder.ShowDialog() == DialogResult.OK)
                    txtSearchPathRoot.Text = openFolder.SelectedPath;
            }
        }


        private readonly List<string> filesToProcess = new List<string>();
        private string TextToSearchFor;
        private int filesProcessed;
        private int totalHits;
        private int totalExceptions;
        private bool searchAborted;
        private bool caseSensitive;

        private class ProcessFileResult
        {
            public string FileName;
            public int Hits;
            public int Exceptions;
        }

        private readonly ConcurrentBag<ProcessFileResult> processFileResults = new ConcurrentBag<ProcessFileResult>();

        private readonly ConcurrentDictionary<string, int> specialOutputHits = new ConcurrentDictionary<string, int>();
        private readonly ConcurrentQueue<string> specialOutputHitsQueue = new ConcurrentQueue<string>();

        private void btnStartSearch_Click(object sender, EventArgs e)
        {
            dataGridView1.RowCount = 0;

            try
            {
                btnStartSearch.Enabled = false;

                filesToProcess.Clear();
                TextToSearchFor = searchText.Text;
                filesProcessed = 0;
                totalHits = 0;
                totalExceptions = 0;
                searchAborted = false;
                caseSensitive = checkBox_CaseSensitive.Checked;

                ProcessFileResult result;
                while (!processFileResults.IsEmpty)
                    processFileResults.TryTake(out result);


                specialOutputHits.Clear();
                string specialOutputHitsResult;
                while (!specialOutputHitsQueue.IsEmpty)
                    specialOutputHitsQueue.TryDequeue(out specialOutputHitsResult);
                richTextBox1.Clear();


                filesToProcess.AddRange(Directory.GetFiles(txtSearchPathRoot.Text, "*.pcap", SearchOption.AllDirectories));
                filesToProcess.AddRange(Directory.GetFiles(txtSearchPathRoot.Text, "*.pcapng", SearchOption.AllDirectories));

                toolStripStatusLabel1.Text = "Files Processed: 0 of " + filesToProcess.Count;

                txtSearchPathRoot.Enabled = false;
                searchText.Enabled = false;
                btnChangeSearchPathRoot.Enabled = false;
                btnStopSearch.Enabled = true;

                timer1.Start();

                ThreadPool.QueueUserWorkItem((state) =>
                {
                    // Do the actual search here
                    DoSearch();

                    if (!Disposing && !IsDisposed)
                        btnStopSearch.BeginInvoke((Action)(() => btnStopSearch_Click(null, null)));
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                btnStopSearch_Click(null, null);
            }
        }

        private void btnStopSearch_Click(object sender, EventArgs e)
        {
            searchAborted = true;

            timer1.Stop();

            timer1_Tick(null, null);

            txtSearchPathRoot.Enabled = true;
            searchText.Enabled = true;
            btnChangeSearchPathRoot.Enabled = true;
            btnStartSearch.Enabled = true;
            btnStopSearch.Enabled = false;
        }


        private void DoSearch()
        {
            Parallel.ForEach(filesToProcess, (currentFile) =>
            {
                if (searchAborted || Disposing || IsDisposed)
                    return;

                try
                {
                    ProcessFile(currentFile);
                }
                catch { }
            });
        }

        private void ProcessFile(string fileName)
        {
            int hits = 0;
            int exceptions = 0;

            var records = PCapReader.LoadPcap(fileName, true, ref searchAborted);

            foreach (PacketRecord record in records)
            {
                if (searchAborted || Disposing || IsDisposed)
                    return;

                try
                {
                    if (record.data.Length <= 4 || (TextToSearchFor.Length > record.data.Length) )
                        continue;

					//BinaryReader messageDataReader = new BinaryReader(new MemoryStream(record.data));
					//var messageCode = messageDataReader.ReadUInt32();
					int messageCode = BitConverter.ToInt32(record.data, 0);


                    var result = SearchForText(record, TextToSearchFor, caseSensitive);
                    if (result > 0)
                        hits++;

                }
                catch
                {
                    // Do something with the exception maybe
                    exceptions++;

                    Interlocked.Increment(ref totalExceptions);
                }
            }

            Interlocked.Increment(ref filesProcessed);

            processFileResults.Add(new ProcessFileResult() { FileName = fileName, Hits = hits, Exceptions = exceptions });
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ProcessFileResult result;
            while (!processFileResults.IsEmpty)
            {
                if (processFileResults.TryTake(out result))
                {
                    var length = new FileInfo(result.FileName).Length;

                    if (result.Hits > 0 || result.Exceptions > 0)
                        dataGridView1.Rows.Add(result.Hits, result.Exceptions, length, result.FileName);
                }
            }

            string specialOutputHitsQueueResult;
            StringBuilder specialOutput = new StringBuilder();
            while (!specialOutputHitsQueue.IsEmpty)
            {
                if (specialOutputHitsQueue.TryDequeue(out specialOutputHitsQueueResult))
                    specialOutput.AppendLine(specialOutputHitsQueueResult);
            }
            richTextBox1.AppendText(specialOutput.ToString());

            toolStripStatusLabel1.Text = "Files Processed: " + filesProcessed.ToString("N0") + " of " + filesToProcess.Count.ToString("N0");

            toolStripStatusLabel2.Text = "Total Hits: " + totalHits.ToString("N0");

            toolStripStatusLabel3.Text = "Message Exceptions: " + totalExceptions.ToString("N0");
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            var fileName = (string)dataGridView1.Rows[e.RowIndex].Cells[3].Value;
            if (caseSensitive)
            {
                System.Diagnostics.Process.Start(Application.ExecutablePath, "-f" + '"' + fileName + '"' + " --cst " + '"' + TextToSearchFor + '"');
            }
            else
            {
                System.Diagnostics.Process.Start(Application.ExecutablePath, "-f" + '"' + fileName + '"' + " --cit " + '"' + TextToSearchFor + '"');
            }
        }

        private void searchText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btnStartSearch.PerformClick();
            }
        }

        private int SearchForText(PacketRecord record, string textToSearch, bool caseSensitive = true)
        {
            int exceptions = 0;
            int hits = 0;

            try
            {
                if (record.data.Length <= 4)
                    return hits;

                //BinaryReader fragDataReader = new BinaryReader(new MemoryStream(record.data));

                if (caseSensitive)
                {
                    int asciiResult = SearchBytePattern(ASCIIEncoding.ASCII.GetBytes(textToSearch), record.data);
                    int unicodeResult = SearchBytePattern(UnicodeEncoding.Unicode.GetBytes(textToSearch), record.data);
                    if (asciiResult > 0 || unicodeResult > 0)
                        hits++;
                }
                else
                {
                    string asciiStringData = System.Text.Encoding.ASCII.GetString(record.data);
                    string unicodeStringData = System.Text.Encoding.Unicode.GetString(record.data);
					// Shift the byte stream by 1 to catch any Unicode strings not on the previous two byte boundary
					string unicodeStringData2 = System.Text.Encoding.Unicode.GetString(record.data, 1, record.data.Length - 1);//(int)fragDataReader.BaseStream.Length - 1);
                    int asciiResultCI = asciiStringData.IndexOf(textToSearch, StringComparison.OrdinalIgnoreCase);
                    int unicodeResultCI = unicodeStringData.IndexOf(textToSearch, StringComparison.OrdinalIgnoreCase);
                    int unicodeResultCI2 = unicodeStringData2.IndexOf(textToSearch, StringComparison.OrdinalIgnoreCase);
                    if (asciiResultCI != -1 || unicodeResultCI != -1 || unicodeResultCI2 != -1)
                        hits++;
                }
            }
            catch
            {
                // Do something with the exception maybe
                exceptions++;

                //Interlocked.Increment(ref totalExceptions);
            }
            return hits;
        }

        private int SearchBytePattern(byte[] pattern, byte[] bytes)
        {
            int matches = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                if (pattern[0] == bytes[i] && bytes.Length - i >= pattern.Length)
                {
                    bool ismatch = true;
                    for (int j = 1; j < pattern.Length && ismatch == true; j++)
                    {
                        if (bytes[i + j] != pattern[j])
                            ismatch = false;
                    }
                    if (ismatch)
                    {
                        matches++;
                        i += pattern.Length - 1;
                    }
                }
            }
            return matches;
        }
    }
}
