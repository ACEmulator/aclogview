using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using aclogview.Properties;
using aclogview.Tools.Scrapers;

namespace aclogview.Tools
{
    public partial class PcapScraperForm : Form
    {
        public PcapScraperForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            txtSearchPathRoot.Text = Settings.Default.FindOpcodeInFilesRoot;
            txtOutputFolder.Text = Settings.Default.FragDatFileOutputFolder;

            // Use reflection to get all of our Scraper classes
            var scrapers = new List<Scraper>();
            foreach (Type type in Assembly.GetAssembly(typeof(Scraper)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Scraper))))
                scrapers.Add((Scraper)Activator.CreateInstance(type));

            foreach (var scraper in scrapers)
            {
                var index = dataGridView1.Rows.Add(false, scraper.GetType().Name, scraper.Description);
                dataGridView1.Rows[index].Tag = scraper;
            }

            // Center to our owner, if we have one
            if (Owner != null)
                Location = new Point(Owner.Location.X + Owner.Width / 2 - Width / 2, Owner.Location.Y + Owner.Height / 2 - Height / 2);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            searchAborted = true;
            writeOutputAborted = true;

            Settings.Default.FindOpcodeInFilesRoot = txtSearchPathRoot.Text;
            Settings.Default.FragDatFileOutputFolder = txtOutputFolder.Text;

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

        private void btnChangeOutputFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog openFolder = new FolderBrowserDialog())
            {
                if (openFolder.ShowDialog() == DialogResult.OK)
                    txtOutputFolder.Text = openFolder.SelectedPath;
            }
        }

        private readonly List<Scraper> scrapers = new List<Scraper>();

        private List<string> filesToProcess = new List<string>();

        private int filesProcessed;
        private readonly Object resultsLockObject = new Object();
        private long totalHits;
        private int totalExceptions;
        private bool searchAborted;
        private bool writeOutputAborted;
        private bool searchCompleted;

        private void btnStartSearch_Click(object sender, EventArgs e)
        {
            try
            {
                btnStartSearch.Enabled = false;

                scrapers.Clear();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(((DataGridViewCheckBoxCell)row.Cells[0]).Value))
                        scrapers.Add((Scraper)row.Tag);
                }

                filesToProcess = ToolUtil.GetPcapsInFolder(txtSearchPathRoot.Text);

                filesProcessed = 0;
                totalHits = 0;
                totalExceptions = 0;
                searchAborted = false;
                writeOutputAborted = false;
                searchCompleted = false;

                UpdateToolStrip("Processing Files");

                txtSearchPathRoot.Enabled = false;
                btnChangeSearchPathRoot.Enabled = false;
                dataGridView1.Enabled = false;
                btnStopSearch.Enabled = true;

                timer1.Start();

                ThreadPool.QueueUserWorkItem((state) =>
                {
                    // Do the actual search here
                    DoSearch();

                    searchCompleted = true;

                    if (!Disposing && !IsDisposed)
                        BeginInvoke((Action)(() => btnStopSearch_Click(null, null)));
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
            if (!searchCompleted)
            {
                if (!searchAborted && !writeOutputAborted)
                {
                    searchAborted = true;
                    btnStopSearch.Text = "Stop Writing Output";
                    return;
                }

                if (!writeOutputAborted)
                {
                    writeOutputAborted = true;
                    btnStopSearch.Text = "Stopping Write Output";
                    btnStopSearch.Enabled = false;
                    return;
                }
            }

            if (searchCompleted)
            {
                btnStopSearch.Text = "Stop Scrape";

                timer1.Stop();

                txtSearchPathRoot.Enabled = true;
                btnChangeSearchPathRoot.Enabled = true;
                dataGridView1.Enabled = true;
                btnStartSearch.Enabled = true;
                btnStopSearch.Enabled = false;
            }
        }


        private void DoSearch()
        {
            foreach (var scraper in scrapers)
                scraper.Reset();

            if (chkMultiThread.Checked)
                Parallel.ForEach(filesToProcess, ProcessFile);
            else
            {
                foreach (var currentFile in filesToProcess)
                    ProcessFile(currentFile);
            }

            WriteOutput();
        }

        private void ProcessFile(string fileName)
        {
            if (searchAborted || Disposing || IsDisposed)
                return;

            var records = PCapReader.LoadPcap(fileName, true, ref searchAborted, out _);

            if (chkExcludeNonRetailPcaps.Checked)
            {
                if (records.Count > 0)
                {
                    var servers = ServerList.FindBy(records[0].ipHeader, records[0].isSend);

                    if (servers.Count != 1 || !servers[0].IsRetail)
                        return;
                }
            }

            foreach (var scraper in scrapers)
            {
                if (searchAborted || Disposing || IsDisposed)
                    return;

                var results = scraper.ProcessFileRecords(fileName, records, ref searchAborted);

                lock (resultsLockObject)
                {
                    totalHits += results.hits;
                    totalExceptions += results.messageExceptions;
                }
            }

            Interlocked.Increment(ref filesProcessed);
        }

        private void WriteOutput()
        {
            if (writeOutputAborted || Disposing || IsDisposed)
                return;

            if (!Directory.Exists(txtOutputFolder.Text))
                Directory.CreateDirectory(txtOutputFolder.Text);

            UpdateToolStrip("Writing Output ...");

            foreach (var scraper in scrapers)
            {
                if (writeOutputAborted || Disposing || IsDisposed)
                    break;

                try
                {
                    scraper.WriteOutput(txtOutputFolder.Text, ref writeOutputAborted);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), scraper.GetType().Name);
                }
            }

            if (Disposing || IsDisposed)
                return;

            if (writeOutputAborted)
                UpdateToolStrip("Writing Output Aborted");
            else
                UpdateToolStrip("Writing Output Complete");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateToolStrip();
        }

        private void UpdateToolStrip(string status = null)
        {
            if (status != null)
                toolStripStatusLabel4.Text = "Status: " + status;

            toolStripStatusLabel1.Text = "Files Processed: " + filesProcessed.ToString("N0") + " of " + filesToProcess.Count.ToString("N0");

            toolStripStatusLabel2.Text = "Total Hits: " + totalHits.ToString("N0");

            toolStripStatusLabel3.Text = "Message Exceptions: " + totalExceptions.ToString("N0");
        }
    }
}
