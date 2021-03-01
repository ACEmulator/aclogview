using aclogview.Properties;
using aclogview.Tools.Scrapers;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace aclogview.Tools
{
    public partial class CombatScraper : Form
    {

        private List<string> filesToProcess = new List<string>();
        private List<string> creatureNames = new List<string>();

        private List<string> pcapFileNameList = new List<string>();

        private HashSet<uint> wieldedItemIDs = new HashSet<uint>();
        private HashSet<uint> characterIDs = new HashSet<uint>();


        StringBuilder scrapeResults = new StringBuilder();

        private int numberFilesToProcess = 0;
        private int filesProcessed;
        private readonly Object resultsLockObject = new Object();
        private long totalHits;
        private int totalExceptions;
        private bool searchAborted;
        private bool writeOutputAborted;
        private bool searchCompleted;
        //private string scrapeResults;
        private string fileNameHeading;
        // private string pcapFileNameList = "Creature Hits are in the following PCAP Files:\r\n";


        bool useCharList = false;

        public CombatScraper()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            tbSearchPathRoot.Text = Settings.Default.FindOpcodeInFilesRoot;
            tbOutputFolder.Text = Settings.Default.FragDatFileOutputFolder;
            tbCreatureName1.Text = Settings.Default.CreatureNameCombat1;
            tbCreatureName2.Text = Settings.Default.CreatureNameCombat2;
            tbCreatureName3.Text = Settings.Default.CreatureNameCombat3;
            tbCreatureName4.Text = Settings.Default.CreatureNameCombat4;
            tbCreatureName5.Text = Settings.Default.CreatureNameCombat5;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            searchAborted = true;
            writeOutputAborted = true;

            Settings.Default.FindOpcodeInFilesRoot = tbSearchPathRoot.Text;
            Settings.Default.FragDatFileOutputFolder = tbOutputFolder.Text;
            Settings.Default.CreatureNameCombat1 = tbCreatureName1.Text;
            Settings.Default.CreatureNameCombat2 = tbCreatureName2.Text;
            Settings.Default.CreatureNameCombat3 = tbCreatureName3.Text;
            Settings.Default.CreatureNameCombat4 = tbCreatureName4.Text;
            Settings.Default.CreatureNameCombat5 = tbCreatureName5.Text;

            base.OnClosing(e);
        }

        private void btnChangeSearchPathRoot_Click(object sender, EventArgs e)
        {

            using (FolderBrowserDialog openFolder = new FolderBrowserDialog())
            {
                openFolder.SelectedPath = tbSearchPathRoot.Text;
                if (openFolder.ShowDialog() == DialogResult.OK)
                    tbSearchPathRoot.Text = openFolder.SelectedPath;
            }
        }

        private void btnChangeOutputFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog openFolder = new FolderBrowserDialog())
            {
                openFolder.SelectedPath = tbOutputFolder.Text;
                if (openFolder.ShowDialog() == DialogResult.OK)
                    tbOutputFolder.Text = openFolder.SelectedPath;
            }
        }

        private void btnStopScrape_Click(object sender, EventArgs e)
        {
            if (!searchCompleted)
            {
                if (!searchAborted && !writeOutputAborted)
                {
                    searchAborted = true;
                    btnStopScrape.Text = "Stop Writing Output";
                    return;
                }

                if (!writeOutputAborted)
                {
                    writeOutputAborted = true;
                    btnStopScrape.Text = "Stopping Write Output";
                    btnStopScrape.Enabled = false;
                    return;
                }
            }

            if (searchCompleted)
            {
                btnStopScrape.Text = "Stop Scrape";

                timer1.Stop();

                tbSearchPathRoot.Enabled = true;
                btnChangeSearchPathRoot.Enabled = true;

                btnStartScrape.Enabled = true;
                btnStopScrape.Enabled = false;
            }
        }

        private void btnStartScrape_Click(object sender, EventArgs e)
        {
            //scrapeResults = "";
            creatureNames.Clear();
            // pcapFileNameList = "Creature Hits are in the following PCAP Files:\r\n";

            useCharList = chkCharList.Checked;

            if (tbCreatureName1.Text =="")
            {
                MessageBox.Show("Creature Name is blank", "Warning!");
                return;
            }

            if (tbCreatureName1.Text != "")
                creatureNames.Add(tbCreatureName1.Text);
            if (tbCreatureName2.Text != "")
                creatureNames.Add(tbCreatureName2.Text);
            if (tbCreatureName3.Text != "")
                creatureNames.Add(tbCreatureName3.Text);
            if (tbCreatureName4.Text != "")
                creatureNames.Add(tbCreatureName4.Text);
            if (tbCreatureName5.Text != "")
                creatureNames.Add(tbCreatureName5.Text);

            // Beginning of File Name
            fileNameHeading = tbCreatureName1.Text;
            if (tbFileNameDescription.Text != "")
                fileNameHeading = tbFileNameDescription.Text;
            

            // Scrape starts here
            try
            {
                filesProcessed = 0;
                totalHits = 0;
                totalExceptions = 0;
                searchAborted = false;
                writeOutputAborted = false;
                searchCompleted = false;

                UpdateToolStrip("Status: Getting File List, pleae wait...");

                btnStartScrape.Enabled = false;

                filesToProcess = ToolUtil.GetPcapsInFolder(tbSearchPathRoot.Text);

                UpdateToolStrip("Processing Files");

                tbSearchPathRoot.Enabled = false;
                btnChangeSearchPathRoot.Enabled = false;
              
                btnStopScrape.Enabled = true;
                timer1.Start();

                // Do the actual search here
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    DoSearch();

                    searchCompleted = true;

                    if (!Disposing && !IsDisposed)
                        BeginInvoke((Action)(() => btnStopScrape_Click(null, null)));

                });
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                btnStopScrape_Click(null, null);
            }

        }

        private void DoSearch()
        {
            foreach (var currentFile in filesToProcess)
                ProcessFile(currentFile);

            WriteOutput(scrapeResults.ToString());

            totalHits = 0;
            totalExceptions = 0;
            filesProcessed = 0;
            UpdateToolStrip("Scraping Characters and Weapons");
            if (chkExportCharsWeapons.Checked)
            {
                foreach (var currentFile in pcapFileNameList)
                    ExportCharacterWeapons(currentFile);
            }
            Interlocked.Increment(ref filesProcessed);
        }

        private void ProcessFile(string fileName)
        {
            numberFilesToProcess = filesToProcess.Count;
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

            var scraper = new CombatDamageGiven();
                if (searchAborted || Disposing || IsDisposed)
                    return;

            var results = scraper.ProcessFileRecords(fileName, records, creatureNames, ref useCharList, ref searchAborted);

                lock (resultsLockObject)
                {
                totalHits += results.hits;
                totalExceptions += results.messageExceptions;
                }

            scrapeResults.Append(results.combatInfo);
            if (results.hits > 0)
                pcapFileNameList.Add(fileName);

            characterIDs.UnionWith(results.characterIDs);
            wieldedItemIDs.UnionWith(results.wieldedItemIDs);          

            Interlocked.Increment(ref filesProcessed);
        }

        private void WriteOutput(string scrapeResults)
        {
            if (writeOutputAborted || Disposing || IsDisposed)
                return;

            if (!Directory.Exists(tbOutputFolder.Text))
                Directory.CreateDirectory(tbOutputFolder.Text);

            UpdateToolStrip("Writing Combat Report...");

            var scraper = new CombatDamageGiven();
            if (writeOutputAborted || Disposing || IsDisposed)
                return;

            // TODO:  Redo the way final csv file is put together.

            //string scrapeResultsTemp = string.Join("\r\n", scrapeResults.ToArray());
            string tempPcapFileNameList = string.Join("\r\n", pcapFileNameList.ToArray());

            string combatHeader = $"Combat Damage Given to a creature from a player \r\n" +
                $"CharName,Attack,WeaponGUID,DamageType,Damage,Creature,Critical\r\n";

            string fileListHeader = "The follwing PCAP files contained combat with your creature search list:";

            string fileToWrite = fileListHeader + "\r\n" + tempPcapFileNameList + "\r\n" + "\r\n" + combatHeader  + scrapeResults;
            

            try
                {
                    scraper.WriteOutput(tbOutputFolder.Text, fileToWrite, fileNameHeading, ref writeOutputAborted);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), scraper.GetType().Name);
                }

            if (Disposing || IsDisposed)
                return;

            if (writeOutputAborted)
                UpdateToolStrip("Writing Output Aborted");
            else
                UpdateToolStrip("Writing Output Complete");

        }
        private void ExportCharacterWeapons(string fileName)
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

            // Start Exporter
            var charWeaponExport = new PlayerWeaponExporter();
            if (searchAborted || Disposing || IsDisposed)
                return;


            var results = charWeaponExport.ProcessFileRecords(fileName, records, characterIDs, wieldedItemIDs, ref searchAborted);

            lock (resultsLockObject)
            {
                totalHits += results.hits;
                totalExceptions += results.messageExceptions;
            }

            numberFilesToProcess = pcapFileNameList.Count;
            UpdateToolStrip("Writing Weenies. Please Wait...");
            lock (resultsLockObject)
            {
                totalHits += results.hits;
                totalExceptions += results.messageExceptions;
            }
            Interlocked.Increment(ref filesProcessed);

            charWeaponExport.WriteOutput(tbOutputFolder.Text, ref searchAborted);
            UpdateToolStrip("Combat Scrape Completed");

        }

        private void UpdateToolStrip(string status = null)
        {
            if (status != null)
                toolStripStatusLabel4.Text = "Status: " + status;

            toolStripStatusLabel1.Text = "Files Processed: " + filesProcessed.ToString("N0") + " of " + numberFilesToProcess.ToString("N0");

            toolStripStatusLabel2.Text = "Total Hits: " + totalHits.ToString("N0");

            toolStripStatusLabel3.Text = "Message Exceptions: " + totalExceptions.ToString("N0");
            // statusStrip1.Refresh();
        }

        private void rdbMagicDamage_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Magic Damage is not supported currently.", "Warning!");
            rdbMeleeDamage.Checked = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateToolStrip();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
