using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using aclogview.ACE_Helpers;
using aclogview.Properties;
using aclogview.Tools;

using Be.Windows.Forms;

namespace aclogview
{
    public partial class Form1 : Form
    {
        private ListViewItemComparer comparer = new ListViewItemComparer();
        private ListViewItemComparer comparer2 = new ListViewItemComparer();
        public List<MessageProcessor> messageProcessors = new List<MessageProcessor>();
        private bool loadedAsMessages;

        private string[] args;

        /// <summary>
        /// Add multiple opcodes to highlight to this list.<para /> 
        /// Each opcode will have a different row highlight color.
        /// </summary>
        private readonly List<int> opCodesToHighlight = new List<int>();

        private StringBuilder strbuilder = new StringBuilder();

        private string pcapFilePath;
        private bool isPcapng;
        private int currentOpcode;
        private int currentSelOpcode;
        private int currentSelDocOpcode;
        private bool currentSelOpcodeIsC2S;
        private uint currentUint;
        private string currentHighlightMode;
        private string currentCSText;
        private string currentCIText;
        private string projectDirectory;
        // Highlight mode options
        private string opcodeMode = "Opcode";
        private string textModeCS = "Text (Case-Sensitive)";
        private string textModeCI = "Text (Case-Insensitive)";
        private string uintMode = "UINT32";

        // Default column widths
        private int defSendReceiveWidth;
        private int defTimeWidth;
        private int defHeadersWidth;
        private int defTypeWidth;
        private int defSizeWidth;
        private int defExtraInfoWidth;
        private int defOpcodeWidth;
        private int defPackSeqWidth;
        private int defQueueWidth;
        private int defIterationWidth;
        private int defServerPortWidth;

        public enum SortType
        {
            Uint,
            String
        }

        public enum TimeFormat
        {
            EpochTime,
            LocalTime
        }


        public Form1(string[] args)
        {
            InitializeComponent();

            this.args = args;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            listView_Packets.InitializeHeaderContextMenu(columnsContextMenu);

            Util.initReaders();
            messageProcessors.Add(new CM_Admin());
            messageProcessors.Add(new CM_Advocate());
            messageProcessors.Add(new CM_Allegiance());
            messageProcessors.Add(new CM_Character());
            messageProcessors.Add(new CM_Combat());
            messageProcessors.Add(new CM_Communication());
            messageProcessors.Add(new CM_Death());
            messageProcessors.Add(new CM_Examine());
            messageProcessors.Add(new CM_Fellowship());
            messageProcessors.Add(new CM_Game());
            messageProcessors.Add(new CM_House());
            messageProcessors.Add(new CM_Inventory());
            messageProcessors.Add(new CM_Item());
            messageProcessors.Add(new CM_Login());
            messageProcessors.Add(new CM_Magic());
            messageProcessors.Add(new CM_Misc());
            messageProcessors.Add(new CM_Movement());
            messageProcessors.Add(new CM_Physics());
            messageProcessors.Add(new CM_Qualities());
            messageProcessors.Add(new CM_Social());
            messageProcessors.Add(new CM_Trade());
            messageProcessors.Add(new CM_Train());
            messageProcessors.Add(new CM_Vendor());
            messageProcessors.Add(new CM_Writing());
            messageProcessors.Add(new Proto_UI());

            Globals.UseHex = checkBoxUseHex.Checked;
            projectDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            treeView_ParsedData.ShowNodeToolTips = Settings.Default.ParsedDataTreeviewDisplayTooltips;

            // Initialize our highlight modes
            HighlightMode_comboBox.Items.Add(opcodeMode);
            HighlightMode_comboBox.Items.Add(textModeCS);
            HighlightMode_comboBox.Items.Add(textModeCI);
            HighlightMode_comboBox.Items.Add(uintMode);

            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (options.Opcode > 0)
                {
                    HighlightMode_comboBox.SelectedItem = opcodeMode;
                    opCodesToHighlight.Add(options.Opcode);
                    currentOpcode = options.Opcode;
                    textBox_Search.Text += "0x" + currentOpcode.ToString("X4");
                }
                else if (options.CSTextToSearch != null)
                {
                    HighlightMode_comboBox.SelectedItem = textModeCS;
                    currentCSText = options.CSTextToSearch;
                    textBox_Search.Text = currentCSText;
                }
                else if (options.CITextToSearch != null)
                {
                    HighlightMode_comboBox.SelectedItem = textModeCI;
                    currentCIText = options.CITextToSearch;
                    textBox_Search.Text = currentCIText;
                }
                else
                {
                    currentHighlightMode = opcodeMode;
                    HighlightMode_comboBox.SelectedItem = opcodeMode;
                }

                if (options.InputFile != null)
                    loadPcap(options.InputFile, options.AsMessages);

                if (args.Length == 1 && !args[0].StartsWith("-") && File.Exists(args[0]))
                    loadPcap(args[0], options.AsMessages);
            }
            // Turn on listview double buffering to prevent flickering
            var prop = listView_Packets.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            prop.SetValue(listView_Packets, true, null);
            prop = listView_CreatedObjects.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            prop.SetValue(listView_CreatedObjects, true, null);

            setupTimeColumn();

            fillDefaultColumnWidths();

            var pDocs = new ProtocolDocs();
            if (pDocs.IsTimeForUpdateCheck())
            {
                pDocs.ShowUpToDateMessage = false;
                await pDocs.UpdateIfNeededAsync();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Settings.Default.Save();
        }

        List<PacketRecord> records = new List<PacketRecord>();
        List<ListViewItem> packetListItems = new List<ListViewItem>();
        // For the created objects listview
        List<ListViewItem> createdListItems = new List<ListViewItem>();

        private void loadPcap(string fileName, bool asMessages, bool dontList = false)
        {
            Cursor.Current = Cursors.WaitCursor;
            Text = "AC Log View - " + Path.GetFileName(fileName);
            pcapFilePath = Path.GetFullPath(fileName);
            toolStripStatus.Text = pcapFilePath;
            loadedAsMessages = asMessages; // This needs to be set as well or you will encounter some fragment issues with some messages.
            btnHighlight.Enabled = true;
            menuItem_ReOpenAsFragments.Enabled = true;
            menuItem_ReOpenAsMessages.Enabled = true;
            checkBoxUseHex.Enabled = true;
            checkBox_ShowObjects.Enabled = true;
            menuItem_gotoLine.Enabled = false;
            records.Clear();
            packetListItems.Clear();
            // This code needs to come after records.Clear(); so that the
            // hex display does not try to update while still loading the pcap.
            if (asMessages)
            {
                checkBox_useHighlighting.Checked = false;
                checkBox_useHighlighting.Enabled = false;
            }
            else
            {
                checkBox_useHighlighting.Checked = true;
                checkBox_useHighlighting.Enabled = true;
            }

            bool abort = false;
            records = PCapReader.LoadPcap(fileName, asMessages, ref abort, out isPcapng);

            if (!dontList)
            {
                int hits = 0;
                foreach (PacketRecord record in records)
                {
                    ListViewItem newItem = new ListViewItem(record.index.ToString());
                    newItem.SubItems.Add(record.isSend ? "Send" : "Recv");
                    // Time column is processed in listView_Packets_RetrieveVirtualItem
                    newItem.SubItems.Add(string.Empty);
                    newItem.SubItems.Add(record.packetHeadersStr);
                    newItem.SubItems.Add(record.packetTypeStr);                   
                    newItem.SubItems.Add(record.data.Length.ToString());
                    newItem.SubItems.Add(record.extraInfo);
                    // This one requires special handling and cannot use function.
                    if (record.opcodes.Count == 0) newItem.SubItems.Add(string.Empty);
                    else newItem.SubItems.Add(record.opcodes[0].ToString("X").Substring(4, 4));

                    newItem.SubItems.Add(record.Seq.ToString());
                    newItem.SubItems.Add(((MessageQueue)record.Queue).ToString());
                    newItem.SubItems.Add(record.Iteration.ToString());
                    newItem.SubItems.Add(record.ServerPort.ToString());

                    // Process highlighting modes
                    if (currentHighlightMode == opcodeMode && opCodesToHighlight.Count > 0)
                    {
                        for (int i = 0; i < opCodesToHighlight.Count; i++)
                        {
                            if (record.opcodes.Contains((PacketOpcode)opCodesToHighlight[i]))
                                hits++;
                            // Opcode highlighting is applied in listView_Packets_RetrieveVirtualItem
                        }
                    }
                    else if (currentHighlightMode == textModeCS && textBox_Search.Text.Length != 0)
                    {
                        var result = SearchForText(record, currentCSText, caseSensitive: true);
                        if (result > 0)
                        {
                            newItem.BackColor = Color.LightBlue;
                            hits++;
                        }
                    }
                    else if (currentHighlightMode == textModeCI && textBox_Search.Text.Length != 0)
                    {
                        var result = SearchForText(record, currentCIText, caseSensitive: false);
                        if (result > 0)
                        {
                            newItem.BackColor = Color.LightBlue;
                            hits++;
                        }
                    }
                    else if (currentHighlightMode == uintMode && textBox_Search.Text.Length != 0)
                    {
                        byte[] bytes = BitConverter.GetBytes(currentUint);
                        int result = SearchBytePattern(bytes, record.data);
                        if (result > 0)
                        {
                            newItem.BackColor = Color.LightBlue;
                            hits++;
                        }
                    }
                    packetListItems.Add(newItem);
                }
                if ( (currentHighlightMode == textModeCS || currentHighlightMode == textModeCI) )
                {
                    string searchText = "";
                    if (currentHighlightMode == textModeCS)
                        searchText = currentCSText;
                    else if (currentHighlightMode == textModeCI)
                        searchText = currentCIText;
                    if (hits > 0)
                        Text = "AC Log View - " + Path.GetFileName(pcapFilePath) + $"              Highlighted {hits} message(s) containing text: {searchText}";
                    else
                        Text = "AC Log View - " + Path.GetFileName(pcapFilePath) + $"              No message(s) found containing text: {searchText}";
                }
                else if (currentHighlightMode == opcodeMode && opCodesToHighlight.Count > 0)
                {
                    if (hits > 0)
                    {
                        Text += $"              Highlighted {hits} message(s) containing Opcode: ";
                    }
                    else
                    {
                        Text += $"              No message(s) found containing Opcode: ";
                    }
                    foreach (var opcode in opCodesToHighlight)
                        Text += " 0x" + opcode.ToString("X4") + " (" + opcode + ")";
                }
                else if (currentHighlightMode == uintMode)
                {
                    if (hits > 0)
                        Text = "AC Log View - " + Path.GetFileName(pcapFilePath) + $"              Highlighted {hits} message(s) containing UINT32: {textBox_Search.Text}";
                    else
                        Text = "AC Log View - " + Path.GetFileName(pcapFilePath) + $"              No message(s) found containing UINT32: {textBox_Search.Text}";
                }
            }

            if (!dontList && records.Count > 0)
            {
                listView_Packets.VirtualListSize = records.Count;

                listView_Packets.RedrawItems(0, records.Count - 1, false);
                menuItem_gotoLine.Enabled = true;
                updateData();
            }
            else
            {
                listView_Packets.VirtualListSize = 0;
            }
            Cursor.Current = Cursors.Default;
        }

        private void listView_Packets_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lineNumberColumn.Index || e.Column == timeColumn.Index
                    || e.Column == sizeColumn.Index || e.Column == packSeqColumn.Index
                    || e.Column == iterationColumn.Index)
                comparer.sortType = SortType.Uint;
            else
                comparer.sortType = SortType.String;
            if (comparer.col == e.Column || comparer.col == 0 && e.Column == timeColumn.Index)
            {
                comparer.reverse = !comparer.reverse;
            }
            comparer.col = e.Column == timeColumn.Index ? 0 : e.Column;
            Cursor.Current = Cursors.WaitCursor;
            packetListItems.Sort(comparer);
            Cursor.Current = Cursors.Default;
            if (records.Count>0)
                listView_Packets.RedrawItems(0, records.Count - 1, false);
            updateData();
        }

        class ListViewItemComparer : IComparer<ListViewItem>
        {
            public int col;
            public bool reverse;
            public SortType sortType;

            public int Compare(ListViewItem x, ListViewItem y)
            {
                int result = 0;
                if (sortType == SortType.Uint)
                {
                    result = CompareUInt(x.SubItems[col].Text, y.SubItems[col].Text);
                }
                else
                {
                    result = CompareString(x.SubItems[col].Text, y.SubItems[col].Text);
                }

                if (reverse)
                {
                    result = -result;
                }

                return result;
            }

            public int CompareUInt(string x, string y)
            {
                return UInt32.Parse(x).CompareTo(UInt32.Parse(y));
            }

            public int CompareString(string x, string y)
            {
                return String.Compare(x, y);
            }
        }

        private void updateData()
        {
            updateText();
            updateTree();
            if (listView_Packets.FocusedItem != null)
            {
                lblTracker.Text = "Viewing #" + listView_Packets.FocusedItem.Index;
            }
        }

        private void updateText()
        {
            hexBox1.ByteProvider = new DynamicByteProvider(new byte[] { });

            if (listView_Packets.SelectedIndices.Count > 0)
            {
                PacketRecord record = records[Int32.Parse(packetListItems[listView_Packets.SelectedIndices[0]].SubItems[0].Text)];
                byte[] data = record.data;

                if (record.opcodes.Count > 0) currentSelOpcode = (int)record.opcodes[0];

                if (checkBox_useHighlighting.Checked && !loadedAsMessages)
                {
                    hexBox1.SuspendLayout();
                    int dataConsumed = 0;
                    int fragStartPos = 20 + record.optionalHeadersLen;
                    int curFrag = 0;

                    int selectedIndex = -1;
                    TreeNode selectedNode = treeView_ParsedData.SelectedNode;
                    if (selectedNode != null)
                    {
                        while (selectedNode.Parent != null)
                        {
                            selectedNode = selectedNode.Parent;
                        }
                        selectedIndex = selectedNode.Index;
                    }

                    var consumedDiff = 1;
                    while (dataConsumed < data.Length && consumedDiff > 0)
                    {
                        consumedDiff = dataConsumed;
                        if (dataConsumed < 20)
                        {
                            // Protocol header
                            var protocolHeader = new byte[20];
                            Array.Copy(data, protocolHeader, 20);
                            hexBox1.ByteProvider.InsertBytes(dataConsumed, protocolHeader, Color.Blue, Color.White);
                            dataConsumed += 20;
                        }
                        else if (dataConsumed < (20 + record.optionalHeadersLen))
                        {
                            // Optional headers
                            int headerSize = record.optionalHeadersLen;
                            if (20 + record.optionalHeadersLen >= record.data.Length)
                                headerSize = record.data.Length - 20;
                            var optionalHeaders = new byte[headerSize];
                            Array.Copy(data, 20, optionalHeaders, 0, headerSize);
                            hexBox1.ByteProvider.InsertBytes(dataConsumed, optionalHeaders, Color.Green, Color.White);
                            dataConsumed += headerSize;
                        }
                        else if (record.frags.Count > 0)
                        {
                            if (curFrag < record.frags.Count)
                            {
                                int fragCurPos = dataConsumed - fragStartPos;
                                if (fragCurPos < 16)
                                {
                                    // Fragment header
                                    if (selectedIndex == curFrag)
                                    {
                                        hexBox1.ByteProvider.InsertBytes(dataConsumed, new byte[] { data[dataConsumed] }, Color.Magenta, Color.LightGray);
                                        hexBox1.Select(dataConsumed, 0);
                                        hexBox1.ScrollByteIntoView();
                                    } 
                                    else
                                        hexBox1.ByteProvider.InsertBytes(dataConsumed, new byte[] { data[dataConsumed] }, Color.Magenta, Color.White);
                                    dataConsumed++;
                                }
                                else if (fragCurPos == (16 + record.frags[curFrag].dat_.Length))
                                {
                                    // Next fragment
                                    fragStartPos = dataConsumed;
                                    curFrag++;
                                    if (selectedIndex == curFrag)
                                        hexBox1.ByteProvider.InsertBytes(dataConsumed, new byte[] { data[dataConsumed] }, Color.Magenta, Color.LightGray);
                                    else
                                        hexBox1.ByteProvider.InsertBytes(dataConsumed, new byte[] { data[dataConsumed] }, Color.Magenta, Color.White);
                                    dataConsumed++;
                                }
                                else
                                {
                                    // Fragment data
                                    if (selectedIndex == curFrag)
                                        hexBox1.ByteProvider.InsertBytes(dataConsumed, new byte[] { data[dataConsumed] }, Color.Black, Color.LightGray);
                                    else
                                        hexBox1.ByteProvider.InsertBytes(dataConsumed, new byte[] { data[dataConsumed] }, Color.Black, Color.White);
                                    dataConsumed++;
                                }
                            }
                        }
                        consumedDiff = dataConsumed - consumedDiff;
                    }
                    hexBox1.ResumeLayout();
                }
                else
                {
                    hexBox1.ByteProvider = new DynamicByteProvider(data);
                }
            }
        }

        private void updateTree()
        {
            treeView_ParsedData.Nodes.Clear();
            ContextInfo.Reset();

            if (listView_Packets.SelectedIndices.Count > 0) {
                PacketRecord record = records[Int32.Parse(packetListItems[listView_Packets.SelectedIndices[0]].SubItems[0].Text)];

                if (loadedAsMessages)
                {
                    using (BinaryReader messageDataReader = new BinaryReader(new MemoryStream(record.data)))
                    {
                        try
                        {
                            bool handled = false;
                            foreach (MessageProcessor messageProcessor in messageProcessors)
                            {
                                long readerStartPos = messageDataReader.BaseStream.Position;

                                bool accepted = messageProcessor.acceptMessageData(messageDataReader, treeView_ParsedData);

                                if (accepted && handled)
                                {
                                    throw new Exception("Multiple message processors are handling the same data!");
                                }

                                if (accepted)
                                {
                                    handled = true;
                                    if (messageDataReader.BaseStream.Position != messageDataReader.BaseStream.Length)
                                    {
                                        treeView_ParsedData.Nodes.Add(new TreeNode("WARNING: Packet not fully read!"));
                                    }
                                }

                                messageDataReader.BaseStream.Position = readerStartPos;
                            }

                            if (!handled)
                            {
                                PacketOpcode opcode = Util.readOpcode(messageDataReader);
                                treeView_ParsedData.Nodes.Add(new TreeNode("Unhandled: " + opcode));
                            }
                        }
                        catch (Exception e)
                        {
                            treeView_ParsedData.Nodes.Add(new TreeNode("EXCEPTION: " + e.Message));
                        }
                    }
                }
                else
                {
                    foreach (BlobFrag frag in record.frags) {
                        using (BinaryReader fragDataReader = new BinaryReader(new MemoryStream(frag.dat_)))
                        {
                            try
                            {
                                bool handled = false;
                                foreach (MessageProcessor messageProcessor in messageProcessors)
                                {
                                    long readerStartPos = fragDataReader.BaseStream.Position;

                                    bool accepted = messageProcessor.acceptMessageData(fragDataReader, treeView_ParsedData);

                                    if (accepted && handled)
                                    {
                                        throw new Exception("Multiple message processors are handling the same data!");
                                    }

                                    if (accepted)
                                    {
                                        handled = true;
                                        if (fragDataReader.BaseStream.Position != fragDataReader.BaseStream.Length)
                                        {
                                            treeView_ParsedData.Nodes.Add(new TreeNode("WARNING: Prev fragment not fully read!"));
                                        }
                                    }

                                    fragDataReader.BaseStream.Position = readerStartPos;
                                }

                                if (!handled)
                                {
                                    PacketOpcode opcode = Util.readOpcode(fragDataReader);
                                    treeView_ParsedData.Nodes.Add(new TreeNode("Unhandled: " + opcode));
                                }
                            }
                            catch (Exception e)
                            {
                                treeView_ParsedData.Nodes.Add(new TreeNode("EXCEPTION: " + e.Message));
                            }
                        }
                    }
                }
                // Give each treeview node a unique identifier and context info tooltip
                int i = 0;
                foreach (var node in GetTreeNodes(treeView_ParsedData.Nodes))
                {
                    node.Tag = i;
                    bool indexIsPresent = ContextInfo.contextList.TryGetValue(i, out ContextInfo c);
                    if (indexIsPresent)
                    {
                        node.ToolTipText = $"Data Type: {c.DataType}";
                    }
                    i++;
                }
                // Handle protocol documentation
                if (record.opcodes.Count == 0)
                {
                    protocolWebBrowser.DocumentText = "";
                    currentSelOpcode = 0;
                    return;
                }
                bool isClientToServer = record.isSend;
                if (tabControl1.SelectedTab == tabProtocolDocs)
                    navigateToDocPage((int)record.opcodes[0], isClientToServer);
            }
        }

        private void listView_Packets_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateData();
        }

        private void listView_Packets_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex < packetListItems.Count) {
                e.Item = packetListItems[e.ItemIndex];
                var record = records[Int32.Parse(e.Item.SubItems[0].Text)];
                e.Item.SubItems[2].Text = GetTimestampString(record);
                // Apply highlights here
                if ( (currentHighlightMode == opcodeMode && opCodesToHighlight.Count > 0) )
                {
                    for (int i = 0 ; i < opCodesToHighlight.Count ; i++)
                    {
                        if (record.opcodes.Contains((PacketOpcode)opCodesToHighlight[i]))
                        {
                            if (i == 0) e.Item.BackColor = Color.LightBlue;
                            else if (i == 1) e.Item.BackColor = Color.LightPink;
                            else if (i == 2) e.Item.BackColor = Color.LightGreen;
                            else if (i == 3) e.Item.BackColor = Color.GreenYellow;
                            else e.Item.BackColor = Color.MediumVioletRed;
                            break;
                        }
                    }
                }
            }
        }

        private string GetTimestampString(PacketRecord record)
        {
            if (isPcapng)
            {
                long microseconds = record.tsHigh;
                microseconds = (microseconds << 32) | record.tsLow;

                if (Settings.Default.PacketsListviewTimeFormat == (byte)TimeFormat.LocalTime)
                    return Utility.EpochTimeToLocalTime(microseconds);

                var time = $"{microseconds / (decimal) 1000000:F6}";
                return time;
            }
            else
            {
                if (Settings.Default.PacketsListviewTimeFormat == (byte)TimeFormat.LocalTime)
                    return Utility.EpochTimeToLocalTime(record.tsSec, record.tsUsec);

                return $"{record.tsSec}." + $"{record.tsUsec:D6}";
            }
        }


        private void treeView_ParsedData_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // No need to reload the hex data if we are in messages mode.
            // In fragments mode selection highlighting is applied in updateText()
            // so we do need to call it.
            if (!loadedAsMessages)
                updateText();
            if (ContextInfo.contextList.Count > 0 && loadedAsMessages)
            {
                if (e.Node != null)
                {
                    int selectedNodeIndex = Convert.ToInt32(e.Node.Tag);
                    bool indexIsPresent = ContextInfo.contextList.TryGetValue(selectedNodeIndex, out ContextInfo c);
                    if (indexIsPresent)
                        hexBox1.Select(c.StartPosition, c.Length);
                }
            }
        }

        private void openPcap(bool asMessages)
        {
            using (OpenFileDialog openFile = new OpenFileDialog())
            {
                openFile.AddExtension = true;
                openFile.Filter = "Packet Captures (*.pcap;*.pcapng)|*.pcap;*.pcapng|All Files (*.*)|*.*";

                if (openFile.ShowDialog() != DialogResult.OK)
                    return;
                checkBox_ShowObjects.Checked = false;
                loadedAsMessages = asMessages;

                loadPcap(openFile.FileName, asMessages);
            }
        }

        private void menuItem_OpenAsFragments_Click(object sender, EventArgs e)
        {
            openPcap(false);
        }

        private void menuItem_OpenAsMessages_Click(object sender, EventArgs e)
        {
            openPcap(true);
        }

        private void menuItem_EditPreviousHighlightedRow_Click(object sender, EventArgs e)
        {
            if (listView_Packets.TopItem == null)
                return;
            if (Text.Contains("Highlighted") == false)
                return;
            if (listView_Packets.SelectedIndices.Count == 0)
            {
                listView_Packets.TopItem.Selected = true;
                listView_Packets.TopItem.Focused = true;
            }
            else
            {
                listView_Packets.EnsureVisible(listView_Packets.SelectedIndices[0]);
                listView_Packets.Items[listView_Packets.SelectedIndices[0]].Focused = true;
            }

            for (int i = listView_Packets.SelectedIndices[0] - 1; i >= 0; i--)
            {
                if (listView_Packets.Items[i].BackColor != SystemColors.Window)
                {
                    listView_Packets.TopItem = listView_Packets.Items[i];
                    listView_Packets.Items[i].Selected = true;
                    listView_Packets.Items[i].Focused = true;
                    listView_Packets.Focus();
                    break;
                }
            }
            lblTracker.Text = "Viewing #" + listView_Packets.FocusedItem.Index;
        }

        private void menuItem_EditNextHighlightedRow_Click(object sender, EventArgs e)
        {
            if (listView_Packets.TopItem == null)
                return;
            if (Text.Contains("Highlighted") == false)
                return;

            if (listView_Packets.SelectedIndices.Count == 0)
            { 
                listView_Packets.TopItem.Selected = true;
                listView_Packets.TopItem.Focused = true;
                listView_Packets.Focus();
                if (listView_Packets.TopItem.BackColor != SystemColors.Window)
                    return;
            }
            else
            {
                listView_Packets.EnsureVisible(listView_Packets.SelectedIndices[0]);
                listView_Packets.Items[listView_Packets.SelectedIndices[0]].Focused = true;
            }

            for (int i = listView_Packets.SelectedIndices[0] + 1; i < listView_Packets.Items.Count; i++)
            {
                if (listView_Packets.Items[i].BackColor != SystemColors.Window)
                {
                    listView_Packets.TopItem = listView_Packets.Items[i];
                    listView_Packets.Items[i].Selected = true;
                    listView_Packets.Items[i].Focused = true;
                    listView_Packets.Focus();
                    break;
                }
            }
            lblTracker.Text = "Viewing #" + listView_Packets.FocusedItem.Index;
        }


        private void menuItem_ToolFindOpcodeInFiles_Click(object sender, EventArgs e)
        {
            var form = new FindOpcodeInFilesForm();
            form.Show(this);
        }

        private void menuItem_ToolFindTextInFiles_Click(object sender, EventArgs e)
        {
            var form = new FindTextInFilesForm();
            form.Show(this);
        }

        private void menuItem_ToolPcapScraper_Click(object sender, EventArgs e)
        {
            var form = new PcapScraperForm();
            form.Show(this);
        }

        private void menuItem_ToolBad_Click(object sender, EventArgs e)
        {
            var files = ToolUtil.GetPcapsInFolder();

            OrderedDictionary opcodeOccurrences = new OrderedDictionary();

            foreach (PacketOpcode opcode in Enum.GetValues(typeof(PacketOpcode)))
                opcodeOccurrences[opcode] = 0;

            foreach (string file in files)
            {
                loadPcap(file, false);

                int curPacket = 0;
                int curFragment = 0;
                try
                {
                    for (curPacket = 0; curPacket < records.Count; ++curPacket)
                    {
                        PacketRecord record = records[curPacket];
                        for (curFragment = 0; curFragment < record.frags.Count; ++curFragment)
                        {
                            BlobFrag frag = record.frags[curFragment];
                            if (frag.memberHeader_.numFrags > 0)
                                continue;

                            using (BinaryReader fragDataReader = new BinaryReader(new MemoryStream(frag.dat_)))
                            {

                                bool handled = false;
                                foreach (MessageProcessor messageProcessor in messageProcessors)
                                {
                                    long readerStartPos = fragDataReader.BaseStream.Position;

                                    bool accepted = messageProcessor.acceptMessageData(fragDataReader, treeView_ParsedData);

                                    if (accepted && handled)
                                        throw new Exception("Multiple message processors are handling the same data!");

                                    if (accepted)
                                        handled = true;

                                    fragDataReader.BaseStream.Position = readerStartPos;
                                }

                                /*if (!handled) {
                                    PacketOpcode opcode = Util.readOpcode(fragDataReader);
                                    treeView_ParsedData.Nodes.Add(new TreeNode("Unhandled: " + opcode));
                                }*/
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Packet " + curPacket + " Fragment " + curFragment + " EXCEPTION: " + ex.Message);
                    break;
                }
            }
        }

        private void menuItem_ToolFragDatListTool_Click(object sender, EventArgs e)
        {
            var form = new FragDatListToolForm();
            form.Show(this);
        }

        private void menuItem_Options_Click(object sender, EventArgs e)
        {
            using (var form = new OptionsForm())
            {
                form.ShowDialog();
                listView_Packets.BeginUpdate();
                setupTimeColumn();
                listView_Packets.EndUpdate();
                if (treeView_ParsedData.Nodes.Count == 0) return;
                // Treeview gets redrawn when toggling tooltips so save and restore our state
                var savedExpansionState = treeView_ParsedData.Nodes.GetExpansionState();
                var savedTopNode = treeView_ParsedData.GetTopNode();
                treeView_ParsedData.BeginUpdate();
                treeView_ParsedData.ShowNodeToolTips = Settings.Default.ParsedDataTreeviewDisplayTooltips;
                treeView_ParsedData.Nodes.SetExpansionState(savedExpansionState);
                treeView_ParsedData.SetTopNode(savedTopNode);
                treeView_ParsedData.EndUpdate();
            }
        }

        private async void menuItem_CheckUpdates_Click(object sender, EventArgs e)
        {
            var pDocs = new ProtocolDocs { ShowUpToDateMessage = true };
            await pDocs.UpdateIfNeededAsync();
        }

        private void menuItem_About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("aclogview - ACE edition\n\nA program to view and parse Asheron's Call 1 packet capture files (.pcap) generated by aclog.\n\nFor more info and source code, see https://github.com/ACEmulator/aclogview\n\nOriginally based on aclogview by tfarley (https://github.com/tfarley/aclogview)", "About");
        }


        private void checkBox_HideHeaderOnly_CheckedChanged(object sender, EventArgs e)
        {
            listView_Packets.RedrawItems(0, records.Count - 1, false);
            updateData();
        }

        private void checkBox_useHighlighting_CheckedChanged(object sender, EventArgs e)
        {
            if (records.Count > 0)
                updateText();
        }

        private void parsedContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (treeView_ParsedData.Nodes.Count > 0) {
                var clickedItem = e.ClickedItem.Name;
                switch (clickedItem) {
                    case "ExpandAll":
                        {
                            var currentNodeIndex = treeView_ParsedData.SelectedNode ?? treeView_ParsedData.Nodes[0];
                            treeView_ParsedData.BeginUpdate();
                            treeView_ParsedData.Nodes[0].ExpandAll();
                            treeView_ParsedData.TopNode = currentNodeIndex;
                            currentNodeIndex.EnsureVisible();
                            treeView_ParsedData.EndUpdate();
                            break;
                        }
                    case "CollapseAll":
                        {
                            treeView_ParsedData.BeginUpdate();
                            treeView_ParsedData.Nodes[0].Collapse(false);
                            treeView_ParsedData.SelectedNode = null;
                            treeView_ParsedData.SelectedNode = treeView_ParsedData.TopNode;
                            treeView_ParsedData.EndUpdate();
                            break;
                        }
                    case "CopyAll":
                        {
                            strbuilder.Clear();
                            foreach (var node in GetTreeNodes(treeView_ParsedData.Nodes))
                            {
                                strbuilder.AppendLine($"{node.Text}".PadLeft(
                                    node.Level * Settings.Default.TreeviewCopyAllPadding + node.Text.Length));
                            }
                            Clipboard.SetText(strbuilder.ToString());
                            break;
                        }
                    case "FindID":
                        {
                            for (int i = 0; i < createdListItems.Count; i++)
                            {
                                if (treeView_ParsedData.SelectedNode.Text.Contains(createdListItems[i].SubItems[1].Text))
                                {
                                    listView_CreatedObjects.Items[i].Selected = true;
                                    listView_CreatedObjects.TopItem = createdListItems[i];
                                    System.Media.SystemSounds.Asterisk.Play();
                                    break;
                                }
                            }
                            break;
                        }
                    case "TeleLoc":
                        {
                            if (ACETeleloc.MessageContainsPosition(currentSelOpcode))
                            {
                                PacketRecord record = records[Int32.Parse(packetListItems[listView_Packets.SelectedIndices[0]].SubItems[0].Text)];
                                ACETeleloc.CopyACETelelocToClipboard(record);
                            }
                            break;
                        }
                }
            }
        }

        IEnumerable<TreeNode> GetTreeNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                yield return node;
                foreach (var child in GetTreeNodes(node.Nodes))
                    yield return child;
            }
        }

        private void checkBoxUseHex_CheckedChanged(object sender, EventArgs e)
        {
            Globals.UseHex = checkBoxUseHex.Checked;
            
            Cursor.Current = Cursors.WaitCursor;
            if (treeView_ParsedData.TopNode != null)
            {
                var savedExpansionState = treeView_ParsedData.Nodes.GetExpansionState();
                var savedTopNode = treeView_ParsedData.GetTopNode();
                treeView_ParsedData.BeginUpdate();
                updateTree();
                treeView_ParsedData.Nodes.SetExpansionState(savedExpansionState);
                treeView_ParsedData.SetTopNode(savedTopNode);
                treeView_ParsedData.EndUpdate();
            }
            if (listView_Packets.FocusedItem != null)
            {
                 listView_Packets.Focus();
            }
            if (splitContainer_Top.Panel2Collapsed == false)
            {
                if (listView_CreatedObjects.VirtualListSize != 0)
                {
                    for (int i = 0; i < listView_CreatedObjects.VirtualListSize; i++)
                    {
                        if (Globals.UseHex == false)
                        {
                            string temp = createdListItems[i].SubItems[1].Text;
                            createdListItems[i].SubItems[1].Text = UInt32.Parse(temp.Remove(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier).ToString();
                        }
                        else
                        {
                            uint temp = UInt32.Parse(createdListItems[i].SubItems[1].Text);
                            createdListItems[i].SubItems[1].Text = "0x" + temp.ToString("X");
                        }
                    }
                    listView_CreatedObjects.RedrawItems(0, listView_CreatedObjects.VirtualListSize - 1, false);
                }
            }
            Cursor.Current = Cursors.Default;
        }


        private void CmdLock_Click(object sender, EventArgs e)
        {
            if (CmdLock.Text == "Lock")
            {
                CmdLock.Text = "UnLock";
                listView_Packets.Enabled = false;
            }
            else
            {
                CmdLock.Text = "Lock";
                listView_Packets.Enabled = true;
            }

        }

        private void cmdforward_Click(object sender, EventArgs e)
        {
            if (listView_Packets.SelectedIndices.Count > 0)
            {
                int nextRow = listView_Packets.SelectedIndices[0] + 1;
                if (nextRow < listView_Packets.Items.Count)
                {
                    listView_Packets.Items[nextRow].Selected = true;
                    listView_Packets.Items[nextRow].Focused = true;
                    listView_Packets.EnsureVisible(nextRow);
                    updateData();
                }
            }
        }

        private void cmdbackward_Click(object sender, EventArgs e)
        {
            if (listView_Packets.SelectedIndices.Count > 0)
            {
                int prevRow = listView_Packets.SelectedIndices[0] - 1;
                if (prevRow >= 0)
                {
                    listView_Packets.Items[prevRow].Selected = true;
                    listView_Packets.Items[prevRow].Focused = true;
                    listView_Packets.EnsureVisible(prevRow);
                    updateData();
                }
            }
        }

        private void btnHighlight_Click(object sender, EventArgs e)
        {
            var searchString = textBox_Search.Text;

            if (searchString.Length == 0)
            {
                return;
            }

            if ((string)HighlightMode_comboBox.SelectedItem == opcodeMode)
            {
                if (searchString.Length == 6)
                {
                    if (searchString.Substring(0, 2).ToLower() == "0x")
                    {
                        var opcodeString = searchString.Substring(2, 4);
                        if (HexTest(opcodeString))
                        {
                            currentOpcode = Int32.Parse(opcodeString, System.Globalization.NumberStyles.HexNumber);
                        }
                    }
                }
                // decimal
                if (int.TryParse(searchString, out currentOpcode))
                {
                    // do nothing currently, currentOpcode should be set
                }
                // hex
                else if (HexTest(searchString))
                {
                    currentOpcode = Int32.Parse(searchString, System.Globalization.NumberStyles.HexNumber);
                }
                // c-style hex check
                else if (CHexTest(searchString))
                {
                    currentOpcode = Int32.Parse(searchString.Remove(0, 2), System.Globalization.NumberStyles.HexNumber);
                }
                // reset
                else
                {
                    textBox_Search.Clear();
                }

                if (currentOpcode != 0)
                {
                    textBox_Search.Text = "0x";
                    for (int i = currentOpcode.ToString("X").Length; i < 4; i++)
                    {
                        textBox_Search.Text += "0";
                    }
                    textBox_Search.Text += currentOpcode.ToString("X");
                    opCodesToHighlight.Clear();
                    opCodesToHighlight.Add(currentOpcode);
                    loadPcap(pcapFilePath, loadedAsMessages);
                }
                else
                {
                    toolStripStatus.Text = "Invalid hex code.";
                }
            }
            else if ((string)HighlightMode_comboBox.SelectedItem == textModeCS)
            {
                currentCSText = searchString;
                ClearHighlighting();
                loadPcap(pcapFilePath, loadedAsMessages);
            }
            else if ((string)HighlightMode_comboBox.SelectedItem == textModeCI)
            {
                currentCIText = searchString;
                ClearHighlighting();
                loadPcap(pcapFilePath, loadedAsMessages);
            }
            else if ((string)HighlightMode_comboBox.SelectedItem == uintMode)
            {
                // decimal
                if (uint.TryParse(searchString, out currentUint))
                {
                    // do nothing currently, currentObjectID should be set
                }
                // hex
                else if (HexTest(searchString))
                {
                    currentUint = UInt32.Parse(searchString, System.Globalization.NumberStyles.HexNumber);
                }
                // c-style hex check
                else if (CHexTest(searchString))
                {
                    currentUint = UInt32.Parse(searchString.Remove(0, 2), System.Globalization.NumberStyles.HexNumber);
                }
                // reset
                else
                {
                    textBox_Search.Clear();
                }

                if (currentUint != 0)
                {
                    textBox_Search.Text = "0x";
                    for (int i = currentUint.ToString("X").Length; i < 8; i++)
                    {
                        textBox_Search.Text += "0";
                    }
                    textBox_Search.Text += currentUint.ToString("X");
                    loadPcap(pcapFilePath, loadedAsMessages);
                }
                else
                {
                    toolStripStatus.Text = "Invalid hex code.";
                }
            }
        }

        public bool CHexTest(string test)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z");
        }

        public bool HexTest(string test)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        private void menuItem_ReOpenAsFragments_Click(object sender, EventArgs e)
        {
            checkBox_ShowObjects.Checked = false;
            loadPcap(pcapFilePath, false);
        }

        private void menuItem_ReOpenAsMessages_Click(object sender, EventArgs e)
        {
            checkBox_ShowObjects.Checked = false;
            loadPcap(pcapFilePath, true);
        }

        private void textBox_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // ENTER KEY
                btnHighlight.PerformClick();
        }

        private void checkBox_ShowObjects_CheckedChanged(object sender, EventArgs e)
        { 
            if (checkBox_ShowObjects.Checked == true)
            {
                splitContainer_Top.Panel2Collapsed = false;
                Cursor.Current = Cursors.WaitCursor;
                if (packetListItems.Count > 0)
                    ProcessCreatedObjects(pcapFilePath);
                Cursor.Current = Cursors.Default;
            }
            else
            {
                splitContainer_Top.Panel2Collapsed = true;
                listView_CreatedObjects.VirtualListSize = 0;
                createdListItems.Clear();
            }
        }

        private void ProcessCreatedObjects(string fileName)
        {
            int hits = 0;
            int exceptions = 0;
            bool searchAborted = false;

            var records = PCapReader.LoadPcap(fileName, true, ref searchAborted, out isPcapng);

            foreach (PacketRecord record in records)
            {
                try
                {
                    if (record.data.Length <= 4)
                        continue;

                    using (BinaryReader fragDataReader = new BinaryReader(new MemoryStream(record.data)))
                    {
                        var messageCode = fragDataReader.ReadUInt32();

                        if (messageCode == (uint)PacketOpcode.Evt_Physics__CreateObject_ID)
                        {
                            hits++;
                            var parsed = CM_Physics.CreateObject.read(fragDataReader);
                            ListViewItem lvi = new ListViewItem();
                            lvi.Text = record.index.ToString();
                            lvi.SubItems.Add(Utility.FormatHex(parsed.object_id));
                            lvi.SubItems.Add(parsed.wdesc._name.ToString());
                            lvi.SubItems.Add(parsed.wdesc._wcid.ToString());
                            lvi.SubItems.Add(parsed.wdesc._type.ToString());
                            createdListItems.Add(lvi);
                        }
                    }
                }
                catch
                {
                    // Do something with the exception maybe
                    exceptions++;

                    //Interlocked.Increment(ref totalExceptions);
                }
            }
            listView_CreatedObjects.VirtualListSize = hits;
            if (hits > 0)
                listView_CreatedObjects.RedrawItems(0, hits - 1, false);
        }

        private void listView_CreatedObjects_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 0 || e.Column == 3)
            {
                comparer2.sortType = SortType.Uint;
            }
            else
            {
                comparer2.sortType = SortType.String;
            }
            if (comparer2.col == e.Column)
            {
                comparer2.reverse = !comparer2.reverse;
            }
            comparer2.col = e.Column;
            createdListItems.Sort(comparer2);
            if (listView_CreatedObjects.VirtualListSize > 0)
                listView_CreatedObjects.RedrawItems(0, listView_CreatedObjects.VirtualListSize - 1, false);
        }

        private void HighlightMode_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentHighlightMode == (string)HighlightMode_comboBox.SelectedItem)
                return;

            if ((string)HighlightMode_comboBox.SelectedItem == opcodeMode)
            {
                currentHighlightMode = opcodeMode;
                Text = "AC Log View - " + Path.GetFileName(pcapFilePath);
                textBox_Search.Clear();
                ClearHighlighting();
                textBox_Search.MaxLength = 6;
            }
            else if ((string)HighlightMode_comboBox.SelectedItem == textModeCS)
            {
                currentHighlightMode = textModeCS;
                Text = "AC Log View - " + Path.GetFileName(pcapFilePath);
                textBox_Search.Clear();
                opCodesToHighlight.Clear();
                ClearHighlighting();
                textBox_Search.MaxLength = 256;
            }
            else if ((string)HighlightMode_comboBox.SelectedItem == textModeCI)
            {
                currentHighlightMode = textModeCI;
                Text = "AC Log View - " + Path.GetFileName(pcapFilePath);
                textBox_Search.Clear();
                opCodesToHighlight.Clear();
                ClearHighlighting();
                textBox_Search.MaxLength = 256;
            }
            else if ((string)HighlightMode_comboBox.SelectedItem == uintMode)
            {
                currentHighlightMode = uintMode;
                Text = "AC Log View - " + Path.GetFileName(pcapFilePath);
                textBox_Search.Clear();
                opCodesToHighlight.Clear();
                ClearHighlighting();
                textBox_Search.MaxLength = 10;
            }
        }

        private void ClearHighlighting()
        {
            for (int i = 0; i < listView_Packets.VirtualListSize; i++)
            {
                listView_Packets.Items[i].BackColor = SystemColors.Window;
            }
        }

        private int SearchForText(PacketRecord record, string textToSearch, bool caseSensitive = true)
        {
            int exceptions = 0;
            int hits = 0;

            try
            {
                if (record.data.Length <= 4 || (textToSearch.Length > record.data.Length) )
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

        private void listView_CreatedObjects_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex < createdListItems.Count)
            {
                e.Item = createdListItems[e.ItemIndex];
            }
        }

        private void objectsContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == jumpToMessageMenuItem)
            {
                var selected = Int32.Parse(createdListItems[listView_CreatedObjects.SelectedIndices[0]].Text);
                listView_Packets.TopItem = listView_Packets.Items[selected];
                listView_Packets.Items[selected].Selected = true;
                lblTracker.Text = "Viewing #" + listView_Packets.Items[selected].Index;
            }
            else if (e.ClickedItem == highlightObjectIDMenuItem)
            {
                if (currentHighlightMode != uintMode)
                {
                    // Set highlight mode so we don't need to wait on the event handler 
                    // HighlightMode_comboBox_SelectedIndexChanged to finish
                    currentHighlightMode = uintMode;
                    HighlightMode_comboBox.SelectedItem = uintMode;
                }
                textBox_Search.Text = createdListItems[listView_CreatedObjects.SelectedIndices[0]].SubItems[1].Text;
                btnHighlight.PerformClick();
            }
        }

        private void treeView_ParsedData_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeView_ParsedData.SelectedNode == null) return;
            if (e.Button == MouseButtons.Right)
                treeView_ParsedData.SelectedNode = e.Node;
            // Left mouse click is already handled
            if (ContextInfo.contextList.Count == 0 || !loadedAsMessages) return;
            int selectedNodeIndex = Convert.ToInt32(treeView_ParsedData.SelectedNode.Tag);
            bool indexIsPresent = ContextInfo.contextList.TryGetValue(selectedNodeIndex, out ContextInfo c);
            if (indexIsPresent && hexboxSelectionChanged(c))
                hexBox1.Select(c.StartPosition, c.Length);
        }

        private bool hexboxSelectionChanged(ContextInfo c)
        {
            return hexBox1.SelectionStart != c.StartPosition & hexBox1.SelectionLength != c.Length;
        }

        private void parsedContextMenu_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = (treeView_ParsedData.Nodes.Count == 0);
            // Only display "Find ID in Object List" option if the list is open
            if (treeView_ParsedData.SelectedNode != null && createdListItems.Count > 0)
                FindID.Visible = true;
            else
                FindID.Visible = false;

            TeleLoc.Visible = ACETeleloc.MessageContainsPosition(currentSelOpcode);
        }

        private void objectsContextMenu_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = (createdListItems.Count == 0 || listView_CreatedObjects.SelectedIndices.Count == 0);
        }

        private void hexContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == copyHexMenuItem)
            {
                hexBox1.CopyHex();
            }
            else if (e.ClickedItem == copyTextMenuItem)
            {
                hexBox1.CopyText();
            }
        }

        private void hexContextMenu_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = (hexBox1.SelectionLength == 0);
        }

        private void menuItem_gotoLine_Click(object sender, EventArgs e)
        {
            using (var form = new GotoLine(listView_Packets.Items.Count))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    listView_Packets.TopItem = listView_Packets.Items[form.lineNumber];
                    listView_Packets.Items[form.lineNumber].Selected = true;
                    lblTracker.Text = "Viewing #" + listView_Packets.Items[form.lineNumber].Index;
                }
            }
        }

        private void navigateToDocPage(int opcode, bool isClientToServer)
        {
            if (opcode == currentSelDocOpcode && currentSelOpcodeIsC2S == isClientToServer) return;
            currentSelDocOpcode = opcode;
            currentSelOpcodeIsC2S = isClientToServer;
            var currentOpcodeString = "0x" + opcode.ToString("X8").Substring(4, 4);
            var direction = isClientToServer ? "C2S" : "S2C";
            protocolWebBrowser.DocumentText =
                $"<!DOCTYPE HTML>" +
                $"<html>" +
                $"<head>" +
                $"<title>AC Protocol</title>" +
                $"<link type = \"text/css\" rel = \"stylesheet\" href = \"file:///{projectDirectory}/Protocol Documentation/Classic.css\"/>" +
                $"</head>" +
                $"<frameset cols = \"*, *\" frameborder = \"yes\" framespacing = \"2\">" +
                $"<frame name = \"frameMsg\" scrolling = \"yes\" src = \"file:///{projectDirectory}/Protocol Documentation/Messages/{currentOpcodeString}-{direction}.html\"/>" +
                "<frame name = \"frameType\" scrolling = \"yes\" src = \"" +
                $"<head><link type='text/css' rel='stylesheet' href='file:///{projectDirectory}/Protocol Documentation/Classic.css'/></head>" +
                "<body><p class='Prompt'>Click a type to display the definition in this window.</p></body>\"/>" +
                $"</frameset>" +
                $"</html> ";
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedTab != tabProtocolDocs) return;
            if (listView_Packets.SelectedIndices.Count <= 0) return;
            var record = records[int.Parse(packetListItems[listView_Packets.SelectedIndices[0]].SubItems[0].Text)];
            if (record.opcodes.Count > 0)
            {
                var isClientToServer = record.isSend;
                navigateToDocPage((int)record.opcodes[0], isClientToServer);
            }
            else
            {
                protocolWebBrowser.DocumentText = "";
                currentSelOpcode = 0;
            }
        }

        private void setupTimeColumn()
        {
            var timeFormat = Settings.Default.PacketsListviewTimeFormat;
            if (timeFormat == (byte)TimeFormat.EpochTime)
            {
                timeColumn.Text = "Epoch Time (s)";
                if (timeMenuItem.Checked)
                    timeColumn.Width = 120;
                defTimeWidth = 120;
            }
            else if (timeFormat == (byte)TimeFormat.LocalTime)
            {
                timeColumn.Text = "Local Time";
                if (timeMenuItem.Checked)
                    timeColumn.Width = 175;
                defTimeWidth = 175;
            }
        }

        private void listviewContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var clickedItem = e.ClickedItem;

            if (clickedItem == copyTimeMenuItem)
            {
                Clipboard.SetText(packetListItems[listView_Packets.SelectedIndices[0]].SubItems[2].Text);
            }
        }

        private void listView_Packets_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView_Packets.SelectedIndices.Count == 0)
                return;

            if (e.Button == MouseButtons.Right)
            {
                var location = listView_Packets.PointToScreen(new Point(e.X, e.Y));
                listviewContextMenu.Show(location);
            }
        }

        private void fillDefaultColumnWidths()
        {
            defSendReceiveWidth = sendReceiveColumn.Width;
            defTimeWidth = timeColumn.Width;
            defHeadersWidth = headersColumn.Width;
            defTypeWidth = typeColumn.Width;
            defSizeWidth = sizeColumn.Width;
            defExtraInfoWidth = extraInfoColumn.Width;
            defOpcodeWidth = hexOpcodeColumn.Width;
            defPackSeqWidth = packSeqColumn.Width;
            defQueueWidth = queueColumn.Width;
            defIterationWidth = iterationColumn.Width;
            defServerPortWidth = serverPortColumn.Width;
        }

        private void columnsContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == sendReceiveMenuItem)
            {
                sendReceiveColumn.Width = sendReceiveMenuItem.CheckState == CheckState.Unchecked ? defSendReceiveWidth : 0;
            }
            else if (e.ClickedItem == timeMenuItem)
            {
                timeColumn.Width = timeMenuItem.CheckState == CheckState.Unchecked ? defTimeWidth : 0;
            }
            else if (e.ClickedItem == headersMenuItem)
            {
                headersColumn.Width = headersMenuItem.CheckState == CheckState.Unchecked ? defHeadersWidth : 0;
            }
            else if (e.ClickedItem == typeMenuItem)
            {
                typeColumn.Width = typeMenuItem.CheckState == CheckState.Unchecked ? defTypeWidth : 0;
            }
            else if (e.ClickedItem == sizeMenuItem)
            {
                sizeColumn.Width = sizeMenuItem.CheckState == CheckState.Unchecked ? defSizeWidth : 0;
            }
            else if (e.ClickedItem == extraInfoMenuItem)
            {
                extraInfoColumn.Width = extraInfoMenuItem.CheckState == CheckState.Unchecked ? defExtraInfoWidth : 0;
            }
            else if (e.ClickedItem == opcodeMenuItem)
            {
                hexOpcodeColumn.Width = opcodeMenuItem.CheckState == CheckState.Unchecked ? defOpcodeWidth : 0;
            }
            else if (e.ClickedItem == packSeqMenuItem)
            {
                packSeqColumn.Width = packSeqMenuItem.CheckState == CheckState.Unchecked ? defPackSeqWidth : 0;
            }
            else if (e.ClickedItem == queueMenuItem)
            {
                queueColumn.Width = queueMenuItem.CheckState == CheckState.Unchecked ? defQueueWidth : 0;
            }
            else if (e.ClickedItem == iterationMenuItem)
            {
                iterationColumn.Width = iterationMenuItem.CheckState == CheckState.Unchecked ? defIterationWidth : 0;
            }
            else if (e.ClickedItem == serverPortMenuItem)
            {
                serverPortColumn.Width = serverPortMenuItem.CheckState == CheckState.Unchecked ? defServerPortWidth : 0;
            }
        }

        private void listView_CreatedObjects_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView_CreatedObjects.SelectedIndices.Count == 0)
                return;

            if (e.Button == MouseButtons.Right)
            {
                var location = listView_CreatedObjects.PointToScreen(new Point(e.X, e.Y));
                objectsContextMenu.Show(location);
            }
        }
    }
}

