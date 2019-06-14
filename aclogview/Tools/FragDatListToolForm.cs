using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using aclogview.Properties;
using aclogview.Tools;

namespace aclogview
{
    public partial class FragDatListToolForm : Form
    {
        public FragDatListToolForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            txtOutputFolder.Text = Settings.Default.FragDatFileOutputFolder;
            txtSearchPathRoot.Text = Settings.Default.FindOpcodeInFilesRoot;
            txtFileToProcess.Text = Settings.Default.FragDatFileToProcess;

            // Center to our owner, if we have one
            if (Owner != null)
                Location = new Point(Owner.Location.X + Owner.Width / 2 - Width / 2, Owner.Location.Y + Owner.Height / 2 - Height / 2);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            searchAborted = true;

            Settings.Default.FragDatFileOutputFolder = txtOutputFolder.Text;
            Settings.Default.FindOpcodeInFilesRoot = txtSearchPathRoot.Text;
            Settings.Default.FragDatFileToProcess = txtFileToProcess.Text;

            base.OnClosing(e);
        }

        private void btnChangeOutputFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog openFolder = new FolderBrowserDialog())
            {
                if (openFolder.ShowDialog() == DialogResult.OK)
                    txtOutputFolder.Text = openFolder.SelectedPath;
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

        private void btnChangeFileToProcess_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFile = new OpenFileDialog())
            {
                openFile.Filter = "Frag Dat List (*.frags)|*.frags";
                openFile.DefaultExt = ".frags";

                if (openFile.ShowDialog() == DialogResult.OK)
                    txtFileToProcess.Text = openFile.FileName;
            }
        }


        private List<string> filesToProcess = new List<string>();
        private int filesProcessed;
        private int fragmentsProcessed;
        private int totalHits;
        private int totalExceptions;
        private bool searchAborted;

        private void ResetVariables()
        {
            filesToProcess.Clear();
            filesProcessed = 0;
            fragmentsProcessed = 0;
            totalHits = 0;
            totalExceptions = 0;
            searchAborted = false;
        }


        private void btnStartBuild_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtOutputFolder.Text))
            {
                MessageBox.Show("Output folder does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                btnStartBuild.Enabled = false;
                groupBoxGeneralSettings.Enabled = false;
                groupBoxProcessFragDatListFile.Enabled = false;

                ResetVariables();

                filesToProcess = ToolUtil.GetPcapsInFolder(txtSearchPathRoot.Text);

                txtSearchPathRoot.Enabled = false;
                btnChangeSearchPathRoot.Enabled = false;
                chkCompressOutput.Enabled = false;
                chkIncludeFullPathAndFileName.Enabled = false;
                btnStopBuild.Enabled = true;

                timer1.Start();

				ThreadPool.QueueUserWorkItem((state) =>
                {
                    // Do the actual work here
                    DoBuild();

                    if (!Disposing && !IsDisposed)
                        btnStopBuild.BeginInvoke((Action)(() => btnStopBuild_Click(null, null)));
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                btnStopBuild_Click(null, null);
            }
        }

        private void btnStopBuild_Click(object sender, EventArgs e)
        {
            searchAborted = true;

            timer1.Stop();

            timer1_Tick(null, null);

            txtSearchPathRoot.Enabled = true;
            btnChangeSearchPathRoot.Enabled = true;
            chkCompressOutput.Enabled = true;
            chkIncludeFullPathAndFileName.Enabled = true;
            btnStartBuild.Enabled = true;
            btnStopBuild.Enabled = false;

            groupBoxGeneralSettings.Enabled = true;
            groupBoxProcessFragDatListFile.Enabled = true;
        }


        // ********************************************************************
        // *************************** Sample Files *************************** 
        // ********************************************************************
        private readonly FragDatListFile allFragDatFile = new FragDatListFile();
        private readonly FragDatListFile createObjectFragDatFile = new FragDatListFile();

        private void DoBuild()
        {
            // ********************************************************************
            // ************************ Adjust These Paths ************************ 
            // ********************************************************************
            allFragDatFile.CreateFile(Path.Combine(txtOutputFolder.Text, "All.frags"), chkCompressOutput.Checked ? FragDatListFile.CompressionType.DeflateStream : FragDatListFile.CompressionType.None);
            createObjectFragDatFile.CreateFile(Path.Combine(txtOutputFolder.Text, "CreateObject.frags"), chkCompressOutput.Checked ? FragDatListFile.CompressionType.DeflateStream : FragDatListFile.CompressionType.None);

            // Do not parallel this search
            foreach (var currentFile in filesToProcess)
            {
                if (searchAborted || Disposing || IsDisposed)
                    break;

                try
                {
                    ProcessFileForBuild(currentFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("File failed to process with exception: " + Environment.NewLine + ex, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // ********************************************************************
            // ****************************** Cleanup ***************************** 
            // ********************************************************************
            allFragDatFile.CloseFile();
            createObjectFragDatFile.CloseFile();
        }

        private void ProcessFileForBuild(string fileName)
        {
            // NOTE: If you want to get fully constructed/merged messages instead of fragments:
            // Pass true below and use record.data as the full message, instead of individual record.frags
            var records = PCapReader.LoadPcap(fileName, false, ref searchAborted, out var isPcapng);

            // Temperorary objects
            var allFrags = new List<FragDatListFile.FragDatInfo>();
            var createObjectFrags = new List<FragDatListFile.FragDatInfo>();

            foreach (var record in records)
            {
                if (searchAborted || Disposing || IsDisposed)
                    return;

                // ********************************************************************
                // ************************ Custom Search Code ************************ 
                // ********************************************************************
                foreach (BlobFrag frag in record.frags)
                {
                    try
                    {
                        if (frag.dat_.Length <= 4)
                            continue;

                        Interlocked.Increment(ref fragmentsProcessed);

                        FragDatListFile.PacketDirection packetDirection = (record.isSend ? FragDatListFile.PacketDirection.ClientToServer : FragDatListFile.PacketDirection.ServerToClient);

                        // Write to emperorary object
                        allFrags.Add(new FragDatListFile.FragDatInfo(packetDirection, record.index, frag.dat_));

						//BinaryReader fragDataReader = new BinaryReader(new MemoryStream(frag.dat_));
						//var messageCode = fragDataReader.ReadUInt32();
						int messageCode = BitConverter.ToInt32(frag.dat_, 0);

						// Write to emperorary object
						if (messageCode == 0xF745) // Create Object
						{
							Interlocked.Increment(ref totalHits);

							createObjectFrags.Add(new FragDatListFile.FragDatInfo(packetDirection, record.index, frag.dat_));
						}
                    }
                    catch
                    {
                        // Do something with the exception maybe
                        Interlocked.Increment(ref totalExceptions);
                    }
                }
            }

            string outputFileName = (chkIncludeFullPathAndFileName.Checked ? fileName : (Path.GetFileName(fileName)));

            // ********************************************************************
            // ************************* Write The Output ************************* 
            // ********************************************************************
            allFragDatFile.Write(new KeyValuePair<string, IList<FragDatListFile.FragDatInfo>>(outputFileName, allFrags));
            createObjectFragDatFile.Write(new KeyValuePair<string, IList<FragDatListFile.FragDatInfo>>(outputFileName, createObjectFrags));

            Interlocked.Increment(ref filesProcessed);
        }


        private void btnStartProcess_Click(object sender, EventArgs e)
        {
            try
            {
                btnStartProcess.Enabled = false;
                groupBoxGeneralSettings.Enabled = false;
                groupBoxFragDatListFileBuilder.Enabled = false;

                ResetVariables();

                filesToProcess.Add(txtFileToProcess.Text);

                txtFileToProcess.Enabled = false;
                btnChangeFileToProcess.Enabled = false;
                btnStopProcess.Enabled = true;

                timer1.Start();

                ThreadPool.QueueUserWorkItem((state) =>
                {
                    // Do the actual work here
                    DoProcess();

                    if (!Disposing && !IsDisposed)
                        btnStopProcess.BeginInvoke((Action)(() => btnStopProcess_Click(null, null)));
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                btnStopProcess_Click(null, null);
            }
        }

        private void btnStopProcess_Click(object sender, EventArgs e)
        {
            searchAborted = true;

            timer1.Stop();

            timer1_Tick(null, null);

            txtFileToProcess.Enabled = true;
            btnChangeFileToProcess.Enabled = true;
            btnStartProcess.Enabled = true;
            btnStopProcess.Enabled = false;

            groupBoxGeneralSettings.Enabled = true;
            groupBoxFragDatListFileBuilder.Enabled = true;
        }

        private void DoProcess()
        {
            // Do not parallel this search
            foreach (var currentFile in filesToProcess)
            {
                if (searchAborted || Disposing || IsDisposed)
                    break;

                try
                {
                    ProcessFileForExamination(currentFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("File failed to process with exception: " + Environment.NewLine + ex, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ProcessFileForExamination(string fileName)
        {
            var fragDatListFile = new FragDatListFile();

            if (!fragDatListFile.OpenFile(fileName))
                return;

            var itemTypesToParse = new List<ITEM_TYPE>();

            var itemTypeKeys = new Dictionary<ITEM_TYPE, List<string>>();
            var itemTypeStreamWriters = new Dictionary<ITEM_TYPE, StreamWriter>();

            // If you only want to output a single item_type, you can change this code
            foreach (ITEM_TYPE itemType in Enum.GetValues(typeof(ITEM_TYPE)))
            {
                itemTypesToParse.Add(itemType);
                itemTypeKeys[itemType] = new List<string>();
                itemTypeStreamWriters[itemType] = new StreamWriter(Path.Combine(txtOutputFolder.Text, itemType + ".csv.temp"));
            }

            try
            {
                TreeView treeView = new TreeView();

                while (true)
                {
                    if (searchAborted || Disposing || IsDisposed)
                        return;

                    KeyValuePair<string, List<FragDatListFile.FragDatInfo>> kvp;

                    if (!fragDatListFile.TryReadNext(out kvp))
                        break;

                    foreach (var frag in kvp.Value)
                    {
                        fragmentsProcessed++;

                        try
                        {
                            // ********************************************************************
                            // ********************** CUSTOM PROCESSING CODE ********************** 
                            // ********************************************************************
                            if (frag.Data.Length <= 4)
                                continue;

							using (BinaryReader fragDataReader = new BinaryReader(new MemoryStream(frag.Data)))
							{
								var messageCode = fragDataReader.ReadUInt32();

								if (messageCode == 0xF745) // Create Object
								{
									var parsed = CM_Physics.CreateObject.read(fragDataReader);

									if (!itemTypesToParse.Contains(parsed.wdesc._type))
										continue;

									totalHits++;

									// This bit of trickery uses the existing tree view parser code to create readable output, which we can then convert to csv
									treeView.Nodes.Clear();
									parsed.contributeToTreeView(treeView);
									if (treeView.Nodes.Count == 1)
									{
										var lineItems = new string[256];
										int lineItemCount = 0;

										ProcessNode(treeView.Nodes[0], itemTypeKeys[parsed.wdesc._type], null, lineItems, ref lineItemCount);

										var sb = new StringBuilder();

										for (int i = 0; i < lineItemCount; i++)
										{
											if (i > 0)
												sb.Append(',');

											var output = lineItems[i];

											// Format the value for CSV output, if needed.
											// We only do this for certain columns. This is very time consuming
											if (output != null && itemTypeKeys[parsed.wdesc._type][i].EndsWith("name"))
											{
												if (output.Contains(",") || output.Contains("\"") || output.Contains("\r") || output.Contains("\n"))
												{
													var sb2 = new StringBuilder();
													sb2.Append("\"");
													foreach (char nextChar in output)
													{
														sb2.Append(nextChar);
														if (nextChar == '"')
															sb2.Append("\"");
													}
													sb2.Append("\"");
													output = sb2.ToString();
												}

											}

											if (output != null)
												sb.Append(output);
										}

										itemTypeStreamWriters[parsed.wdesc._type].WriteLine(sb.ToString());
									}
								}
							}
                        }
                        catch (EndOfStreamException) // This can happen when a frag is incomplete and we try to parse it
                        {
                            totalExceptions++;
                        }
                    }
                }
            }
            finally
            {
                foreach (var streamWriter in itemTypeStreamWriters.Values)
                    streamWriter.Close();

                fragDatListFile.CloseFile();

                Interlocked.Increment(ref filesProcessed);
            }

            // Read in the temp file and save it to a new file with the column headers
            foreach (var kvp in itemTypeKeys)
            {
                if (kvp.Value.Count > 0)
                {
                    using (var writer = new StreamWriter(Path.Combine(txtOutputFolder.Text, kvp.Key + ".csv")))
                    {
                        var sb = new StringBuilder();

                        for (int i = 0; i < kvp.Value.Count; i++)
                        {
                            if (i > 0)
                                sb.Append(',');

                            sb.Append(kvp.Value[i] ?? String.Empty);
                        }

                        writer.WriteLine(sb.ToString());

                        using (var reader = new StreamReader(Path.Combine(txtOutputFolder.Text, kvp.Key + ".csv.temp")))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                                writer.WriteLine(line);
                        }
                    }
                }

                File.Delete(Path.Combine(txtOutputFolder.Text, kvp.Key + ".csv.temp"));
            }
        }

        private void ProcessNode(TreeNode node, List<string> keys, string prefix, string[] lineItems, ref int lineItemCount)
        {
            var kvp = ConvertNodeTextToKVP(node.Text);

            var nodeKey = (prefix == null ? kvp.Key : (prefix + "." + kvp.Key));

            // ********************************************************************
            // ***************** YOU CAN OMIT CERTAIN NODES HERE ****************** 
            // ********************************************************************
            //if (nodeKey.StartsWith("physicsdesc.timestamps")) return;

            if (node.Nodes.Count == 0)
            {
                if (!keys.Contains(nodeKey))
                    keys.Add(nodeKey);

                var keyIndex = keys.IndexOf(nodeKey);

                if (keyIndex >= lineItems.Length)
                    MessageBox.Show("Increase the lineItems array size");

                lineItems[keyIndex] = kvp.Value;

                if (keyIndex + 1 > lineItemCount)
                    lineItemCount = keyIndex + 1;
            }
            else
            {
                foreach (TreeNode child in node.Nodes)
                    ProcessNode(child, keys, nodeKey, lineItems, ref lineItemCount);
            }
        }

        private static KeyValuePair<string, string> ConvertNodeTextToKVP(string nodeText)
        {
            string key = null;
            string value = null;

            var indexOfEquals = nodeText.IndexOf('=');

            if (indexOfEquals == -1)
                value = nodeText;
            else
            {
                key = nodeText.Substring(0, indexOfEquals).Trim();

                if (nodeText.Length > indexOfEquals + 1)
                    value = nodeText.Substring(indexOfEquals + 1, nodeText.Length - indexOfEquals - 1).Trim();
            }

            return new KeyValuePair<string, string>(key, value);
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Files Processed: " + filesProcessed.ToString("N0") + " of " + filesToProcess.Count.ToString("N0");

            toolStripStatusLabel2.Text = "Fragments Processed: " + fragmentsProcessed.ToString("N0");

            toolStripStatusLabel3.Text = "Total Hits: " + totalHits.ToString("N0");

            toolStripStatusLabel4.Text = "Frag Exceptions: " + totalExceptions.ToString("N0");
        }
    }
}
