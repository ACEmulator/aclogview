using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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

            // Center to our owner, if we have one
            if (Owner != null)
                Location = new Point(Owner.Location.X + Owner.Width / 2 - Width / 2, Owner.Location.Y + Owner.Height / 2 - Height / 2);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            searchAborted = true;

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

        // Manage your scrapers here
        // Comment out ones you do not wish to run
        // Add/Uncomment ones that you do want to run
        // todo: Auto populate checkboxes or a checkbox table so the user can select at runtime what scrapers they want
        // todo: use reflection to load the above based on Scraper type
        private readonly List<Scraper> scrapers = new List<Scraper>
        {
            new PacketSizeScraperC2S(),
            new VendorBuySellAmountScraperC2S(),
            new PacketTypesCountScraper(),
            new HeatMapScraper(),
        };

        private List<string> filesToProcess = new List<string>();

        private int filesProcessed;
        private int totalHits;
        private int totalExceptions;
        private bool searchAborted;

        private void btnStartSearch_Click(object sender, EventArgs e)
        {
            try
            {
                btnStartSearch.Enabled = false;

                filesToProcess = ToolUtil.GetPcapsInFolder(txtSearchPathRoot.Text);

                filesProcessed = 0;
                totalHits = 0;
                totalExceptions = 0;
                searchAborted = false;

                toolStripStatusLabel1.Text = "Files Processed: 0 of " + filesToProcess.Count;

                txtSearchPathRoot.Enabled = false;
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

            txtSearchPathRoot.Enabled = true;
            btnChangeSearchPathRoot.Enabled = true;
            btnStartSearch.Enabled = true;
            btnStopSearch.Enabled = false;
        }


        private void DoSearch()
        {
            foreach (var scraper in scrapers)
                scraper.Reset();

            Parallel.ForEach(filesToProcess, (currentFile) =>
            {
                if (searchAborted || Disposing || IsDisposed)
                    return;

                var records = PCapReader.LoadPcap(currentFile, true, ref searchAborted, out _);

                foreach (var scraper in scrapers)
                    scraper.ProcessFileRecords(currentFile, records, ref searchAborted);

                Interlocked.Increment(ref filesProcessed);
            });

            if (!Directory.Exists(txtOutputFolder.Text))
                Directory.CreateDirectory(txtOutputFolder.Text);

            foreach (var scraper in scrapers)
                scraper.WriteOutput(txtOutputFolder.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Files Processed: " + filesProcessed.ToString("N0") + " of " + filesToProcess.Count.ToString("N0");

            toolStripStatusLabel2.Text = "Total Hits: " + totalHits.ToString("N0");

            toolStripStatusLabel3.Text = "Message Exceptions: " + totalExceptions.ToString("N0");
        }
    }
}
