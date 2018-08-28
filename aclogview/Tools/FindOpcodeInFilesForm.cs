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
    public partial class FindOpcodeInFilesForm : Form
    {
        public FindOpcodeInFilesForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            txtSearchPathRoot.Text = Settings.Default.FindOpcodeInFilesRoot;
            txtOpcode.Text = Settings.Default.FindOpcodeInFilesOpcode.ToString("X4");

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

            Settings.Default.FindOpcodeInFilesRoot = txtSearchPathRoot.Text;
            Settings.Default.FindOpcodeInFilesOpcode = OpCode;

            base.OnClosing(e);
        }

        int OpCode
        {
            get
            {
                int value;

                int.TryParse(txtOpcode.Text, NumberStyles.HexNumber, null, out value);

                return value;
            }
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
        private int opCodeToSearchFor;
        private int filesProcessed;
        private int totalHits;
        private int totalExceptions;
        private bool searchAborted;

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
                opCodeToSearchFor = OpCode;
                filesProcessed = 0;
                totalHits = 0;
                totalExceptions = 0;
                searchAborted = false;

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
                txtOpcode.Enabled = false;
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
            txtOpcode.Enabled = true;
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
            bool isPcapng = false;

            var records = PCapReader.LoadPcap(fileName, true, ref searchAborted, ref isPcapng);

            foreach (PacketRecord record in records)
            {
                if (searchAborted || Disposing || IsDisposed)
                    return;

                if (record.opcodes.Contains((PacketOpcode)opCodeToSearchFor))
                {
                    hits++;

                    Interlocked.Increment(ref totalHits);
                }

                // ********************************************************************
                // ************************ CUSTOM SEARCH CODE ************************ 
                // ********************************************************************
                // Custom search code that can output information to Special Output
                // Below are several commented out examples on how you can search through bulk pcaps for targeted data, and output detailed information to the output tab.

                try
                {
                    if (record.data.Length <= 4)
                        continue;

                    //BinaryReader messageDataReader = new BinaryReader(new MemoryStream(record.data));

                    //var messageCode = messageDataReader.ReadUInt32();

                    /*if (messageCode == 0x02BB) // Creature Message
                    {
                        var parsed = CM_Communication.HearSpeech.read(messageDataReader);

                        //if (parsed.ChatMessageType != 0x0C)
                        //    continue;

                        var output = parsed.ChatMessageType.ToString("X4") + " " + parsed.MessageText;

                        if (!specialOutputHits.ContainsKey(output))
                        {
                            if (specialOutputHits.TryAdd(output, 0))
                                specialOutputHitsQueue.Enqueue(output);
                        }
                    }*/

                    /*if (messageCode == 0xF745) // Create Object
                    {
                        var parsed = CM_Physics.CreateObject.read(messageDataReader);
                    }*/

                    /*if (messageCode == 0xF7B0) // Game Event
                    {
                        var character = messageDataReader.ReadUInt32(); // Character
                        var sequence = messageDataReader.ReadUInt32(); // Sequence
                        var _event = messageDataReader.ReadUInt32(); // Event

                        if (_event == 0x0147) // Group Chat
                        {
                            var parsed = CM_Communication.ChannelBroadcast.read(messageDataReader);

                            var output = parsed.GroupChatType.ToString("X4");
                            if (!specialOutputHits.ContainsKey(output))
                            {
                                if (specialOutputHits.TryAdd(output, 0))
                                    specialOutputHitsQueue.Enqueue(output);
                            }
                        }

                        if (_event == 0x02BD) // Tell
                        {
                            var parsed = CM_Communication.HearDirectSpeech.read(messageDataReader);

                            var output = parsed.ChatMessageType.ToString("X4");

                            if (!specialOutputHits.ContainsKey(output))
                            {
                                if (specialOutputHits.TryAdd(output, 0))
                                    specialOutputHitsQueue.Enqueue(output);
                            }
                        }
                    }*/

                    /*if (messageCode == 0xF7B1) // Game Action
                    {
                    }*/

                    /*if (messageCode == 0xF7DE) // TurbineChat
                    {
                        var parsed = CM_Admin.ChatServerData.read(messageDataReader);

                        string output = parsed.TurbineChatType.ToString("X2");

                        if (!specialOutputHits.ContainsKey(output))
                        {
                            if (specialOutputHits.TryAdd(output, 0))
                                specialOutputHitsQueue.Enqueue(output);
                        }
                    }*/

                    /*if (messageCode == 0xF7E0) // Server Message
                    {
                        var parsed = CM_Communication.TextBoxString.read(messageDataReader);

                        //var output = parsed.ChatMessageType.ToString("X4") + " " + parsed.MessageText + ",";
                        var output = parsed.ChatMessageType.ToString("X4");

                        if (!specialOutputHits.ContainsKey(output))
                        {
                            if (specialOutputHits.TryAdd(output, 0))
                                specialOutputHitsQueue.Enqueue(output);
                        }
                    }*/
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

            System.Diagnostics.Process.Start(Application.ExecutablePath, "-f" + '"' + fileName + '"' + " -o " + opCodeToSearchFor);
        }

        private void txtOpcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btnStartSearch.PerformClick();
            }
        }
    }
}
