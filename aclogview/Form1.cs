using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using aclogview.Properties;

namespace aclogview
{
    public partial class Form1 : Form
    {
        private ListViewItemComparer comparer = new ListViewItemComparer();
        public List<MessageProcessor> messageProcessors = new List<MessageProcessor>();
        private long curPacket;
        private bool loadedAsMessages;

        private string[] args;

        /// <summary>
        /// Add multiple opcodes to highlight to this list.<para /> 
        /// Each opcode will have a different row highlight color.
        /// </summary>
        private readonly List<int> opCodesToHighlight = new List<int>();

        private StringBuilder strbuilder = new StringBuilder();

        private string pcapFilePath;
        private int currentOpcode;

        public Form1(string[] args)
        {
            InitializeComponent();

            this.args = args;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            
            
            if (args != null && args.Length >= 2)
            {
                int opcode;
                if (int.TryParse(args[1], out opcode))
                    opCodesToHighlight.Add(opcode);
            }
            if (args != null && args.Length >= 1) {
                loadPcap(args[0], true); // Default to "Open As Messages"
            } else
            {
                toolStripStatus.Text = "AC Log View";
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Settings.Default.Save();
        }

        List<PacketRecord> records = new List<PacketRecord>();
        List<ListViewItem> listItems = new List<ListViewItem>();
        
        private void loadPcap(string fileName, bool asMessages, bool dontList = false) {
            Text = "AC Log View - " + Path.GetFileName(fileName);
            pcapFilePath = Path.GetFullPath(fileName);
            toolStripStatus.Text = pcapFilePath;
            loadedAsMessages = asMessages; // This needs to be set as well or you will encounter some fragment issues with some messages.
            if (asMessages) {
                checkBox_useHighlighting.Checked = false;
                checkBox_useHighlighting.Enabled = false;
            } else {
                checkBox_useHighlighting.Checked = true;
                checkBox_useHighlighting.Enabled = true;
            }
            btnHighlight.Enabled = true;
            menuItem_ReOpen.Enabled = true;
            menuItem_ReOpenAsMessages.Enabled = true;
            if (opCodesToHighlight.Count > 0)
            {
                Text += "              Highlighted OpCodes: ";
                foreach (var opcode in opCodesToHighlight)
                    Text += opcode + " (" + opcode.ToString("X4") + "),";
            }

            records.Clear();
            listItems.Clear();

            bool abort = false;
            records = PCapReader.LoadPcap(fileName, asMessages, ref abort);

            if (!dontList)
            {
                foreach (PacketRecord record in records)
                {
                    ListViewItem newItem = new ListViewItem(record.index.ToString());
                    newItem.SubItems.Add(record.isSend ? "Send" : "Recv");
                    newItem.SubItems.Add(record.tsSec.ToString());
                    newItem.SubItems.Add(record.packetHeadersStr);
                    newItem.SubItems.Add(record.packetTypeStr);                   
                    newItem.SubItems.Add(record.data.Length.ToString());
                    newItem.SubItems.Add(record.extraInfo);
                    // This one requires special handling and cannot use function.
                    if (record.opcodes.Count == 0) newItem.SubItems.Add(string.Empty);
                    else newItem.SubItems.Add(record.opcodes[0].ToString("X").Substring(4, 4));
                    listItems.Add(newItem);
                }
            }

            if (!dontList && records.Count > 0)
            {
                listView_Packets.VirtualListSize = records.Count;

                listView_Packets.RedrawItems(0, records.Count - 1, false);
                updateData();
            }
            else
            {
                listView_Packets.VirtualListSize = 0;
            }
        }

        private void listView_Packets_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (comparer.col == e.Column)
            {
                comparer.reverse = !comparer.reverse;
            }
            comparer.col = e.Column;
            listItems.Sort(comparer);
            if (records.Count>0)
                listView_Packets.RedrawItems(0, records.Count - 1, false);
            updateData();
        }

        class ListViewItemComparer : IComparer<ListViewItem>
        {
            public int col;
            public bool reverse;

            public int Compare(ListViewItem x, ListViewItem y)
            {
                int result = 0;
                if (col == 0 || col == 2 || col == 5)
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
            textBox_PacketData.Clear();

            if (listView_Packets.SelectedIndices.Count > 0)
            {
                PacketRecord record = records[Int32.Parse(listItems[listView_Packets.SelectedIndices[0]].SubItems[0].Text)];
                byte[] data = record.data;

                if (checkBox_useHighlighting.Checked && !loadedAsMessages) {
                    int fragStartPos = 20 + record.optionalHeadersLen;
                    int curFrag = 0;
                    int curLine = 0;
                    int dataConsumed = 0;
                    while (dataConsumed < data.Length)
                    {
                        textBox_PacketData.SelectionColor = Color.Black;
                        textBox_PacketData.AppendText(string.Format("{0:X4}  ", curLine));

                        int lineFragStartPos = fragStartPos;
                        int linecurFrag = curFrag;

                        int hexIndex = 0;
                        for (; hexIndex < Math.Min(16, data.Length - dataConsumed); ++hexIndex)
                        {
                            if (hexIndex == 8)
                            {
                                textBox_PacketData.AppendText(" ");
                            }

                            int dataIndex = dataConsumed + hexIndex;

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

                            // Default color
                            textBox_PacketData.SelectionColor = Color.Red;
                            textBox_PacketData.SelectionBackColor = Color.White;

                            if (dataIndex < 20)
                            {
                                // Protocol header
                                textBox_PacketData.SelectionColor = Color.Blue;
                            }
                            else if (dataIndex < 20 + record.optionalHeadersLen)
                            {
                                // Optional headers
                                textBox_PacketData.SelectionColor = Color.Green;
                            } else if (record.frags.Count > 0) {
                                if (curFrag < record.frags.Count) {
                                    int fragCurPos = dataIndex - fragStartPos;
                                    if (fragCurPos < 16)
                                    {
                                        // Fragment header
                                        textBox_PacketData.SelectionColor = Color.Magenta;
                                    } else if (fragCurPos == (16 + record.frags[curFrag].dat_.Length)) {
                                        // Next fragment
                                        fragStartPos = dataIndex;
                                        curFrag++;
                                        textBox_PacketData.SelectionColor = Color.Magenta;
                                    }
                                    else
                                    {
                                        // Fragment data
                                        textBox_PacketData.SelectionColor = Color.Black;
                                    }

                                    if (selectedIndex == curFrag)
                                    {
                                        textBox_PacketData.SelectionBackColor = Color.LightGray;
                                    }
                                }
                            }

                            textBox_PacketData.AppendText(string.Format("{0:X2} ", data[dataIndex]));
                        }

                        textBox_PacketData.SelectionBackColor = Color.White;
                        StringBuilder spaceAligner = new StringBuilder();
                        spaceAligner.Append(' ', 1 + (16 - hexIndex) * 3 + (hexIndex <= 8 ? 1 : 0));
                        textBox_PacketData.AppendText(spaceAligner.ToString());

                        fragStartPos = lineFragStartPos;
                        curFrag = linecurFrag;

                        for (int i = 0; i < Math.Min(16, data.Length - dataConsumed); ++i) {
                            if (i == 8) {
                                textBox_PacketData.AppendText(" ");
                            }

                            int dataIndex = dataConsumed + i;

                            int selectedIndex = -1;
                            TreeNode selectedNode = treeView_ParsedData.SelectedNode;
                            if (selectedNode != null) {
                                while (selectedNode.Parent != null) {
                                    selectedNode = selectedNode.Parent;
                                }
                                selectedIndex = selectedNode.Index;
                            }

                            // Default color
                            textBox_PacketData.SelectionColor = Color.Red;
                            textBox_PacketData.SelectionBackColor = Color.White;

                            if (dataIndex < 20) {
                                // Protocol header
                                textBox_PacketData.SelectionColor = Color.Blue;
                            } else if (dataIndex < 20 + record.optionalHeadersLen) {
                                // Optional headers
                                textBox_PacketData.SelectionColor = Color.Green;
                            } else if (record.frags.Count > 0) {
                                if (curFrag < record.frags.Count) {
                                    int fragCurPos = dataIndex - fragStartPos;
                                    if (fragCurPos < 16)
                                    {
                                        // Fragment header
                                        textBox_PacketData.SelectionColor = Color.Magenta;
                                    } else if (fragCurPos == (16 + record.frags[curFrag].dat_.Length)) {
                                        // Next fragment
                                        fragStartPos = dataIndex;
                                        curFrag++;
                                        textBox_PacketData.SelectionColor = Color.Magenta;
                                    } else {
                                        // Fragment data
                                        textBox_PacketData.SelectionColor = Color.Black;
                                    }

                                    if (selectedIndex == curFrag) {
                                        textBox_PacketData.SelectionBackColor = Color.LightGray;
                                    }
                                }
                            }

                            char asChar = Convert.ToChar(data[dataIndex]);
                            if (asChar >= ' ' && asChar <= '~') {
                                textBox_PacketData.AppendText(Char.ToString(asChar));
                            } else {
                                textBox_PacketData.AppendText(".");
                            }
                        }

                        textBox_PacketData.AppendText("\n");

                        dataConsumed += 16;
                        curLine++;
                    }
                } else {
                    StringBuilder strBuilder = new StringBuilder();
                    StringBuilder hexBuilder = new StringBuilder();
                    StringBuilder asciiBuilder = new StringBuilder();
                    int wrapCounter = 0;
                    for (int i = 0; i < data.Length; ++i) {
                        if (wrapCounter == 0) {
                            strBuilder.Append(string.Format("{0:X4}  ", i));
                        }

                        hexBuilder.Append(string.Format("{0:X2} ", data[i]));

                        char asChar = Convert.ToChar(data[i]);
                        if (asChar >= ' ' && asChar <= '~') {
                            asciiBuilder.Append(asChar);
                        } else {
                            asciiBuilder.Append('.');
                        }

                        wrapCounter++;
                        if (wrapCounter == 8) {
                            hexBuilder.Append(' ');
                            asciiBuilder.Append(' ');
                        } else if (wrapCounter == 16) {
                            strBuilder.Append(hexBuilder.ToString());
                            hexBuilder.Clear();

                            strBuilder.Append(' ');

                            strBuilder.Append(asciiBuilder.ToString());
                            asciiBuilder.Clear();

                            strBuilder.Append(Environment.NewLine);

                            wrapCounter = 0;
                        }
                    }

                    if (wrapCounter != 0) {
                        int spacesToAppend = (16 - wrapCounter) * 3;
                        if (wrapCounter < 8) {
                            spacesToAppend++;
                        }

                        hexBuilder.Append(' ', spacesToAppend);

                        strBuilder.Append(hexBuilder.ToString());

                        strBuilder.Append(' ');

                        strBuilder.Append(asciiBuilder.ToString());
                    }

                    textBox_PacketData.Text = strBuilder.ToString();
                }
            }
        }

        private void updateTree() {
            treeView_ParsedData.Nodes.Clear();

            if (listView_Packets.SelectedIndices.Count > 0) {
                PacketRecord record = records[Int32.Parse(listItems[listView_Packets.SelectedIndices[0]].SubItems[0].Text)];

                if (loadedAsMessages)
                {
                    BinaryReader messageDataReader = new BinaryReader(new MemoryStream(record.data));
                    try {
                        bool handled = false;
                        foreach (MessageProcessor messageProcessor in messageProcessors) {
                            long readerStartPos = messageDataReader.BaseStream.Position;

                            bool accepted = messageProcessor.acceptMessageData(messageDataReader, treeView_ParsedData);

                            if (accepted && handled) {
                                throw new Exception("Multiple message processors are handling the same data!");
                            }

                            if (accepted) {
                                handled = true;
                                if (messageDataReader.BaseStream.Position != messageDataReader.BaseStream.Length) {
                                    treeView_ParsedData.Nodes.Add(new TreeNode("WARNING: Packet not fully read!"));
                                }
                            }

                            messageDataReader.BaseStream.Position = readerStartPos;
                        }

                        if (!handled) {
                            PacketOpcode opcode = Util.readOpcode(messageDataReader);
                            treeView_ParsedData.Nodes.Add(new TreeNode("Unhandled: " + opcode));
                        }
                    } catch (Exception e) {
                        treeView_ParsedData.Nodes.Add(new TreeNode("EXCEPTION: " + e.Message));
                    }
                }
                else
                {
                    foreach (BlobFrag frag in record.frags) {
                        BinaryReader fragDataReader = new BinaryReader(new MemoryStream(frag.dat_));
                        try {
                            bool handled = false;
                            foreach (MessageProcessor messageProcessor in messageProcessors) {
                                long readerStartPos = fragDataReader.BaseStream.Position;

                                bool accepted = messageProcessor.acceptMessageData(fragDataReader, treeView_ParsedData);

                                if (accepted && handled) {
                                    throw new Exception("Multiple message processors are handling the same data!");
                                }

                                if (accepted) {
                                    handled = true;
                                    if (fragDataReader.BaseStream.Position != fragDataReader.BaseStream.Length) {
                                        treeView_ParsedData.Nodes.Add(new TreeNode("WARNING: Prev fragment not fully read!"));
                                    }
                                }

                                fragDataReader.BaseStream.Position = readerStartPos;
                            }

                            if (!handled) {
                                PacketOpcode opcode = Util.readOpcode(fragDataReader);
                                treeView_ParsedData.Nodes.Add(new TreeNode("Unhandled: " + opcode));
                            }
                        } catch (Exception e) {
                            treeView_ParsedData.Nodes.Add(new TreeNode("EXCEPTION: " + e.Message));
                        }
                    }
                }
            }
        }

        private void listView_Packets_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateData();
        }

        private void listView_Packets_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex < listItems.Count) {
                e.Item = listItems[e.ItemIndex];

                // Apply highlights here
                if (opCodesToHighlight.Count > 0)
                {
                    var record = records[Int32.Parse(e.Item.SubItems[0].Text)];

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


        private void treeView_ParsedData_AfterSelect(object sender, TreeViewEventArgs e)
        {
            updateText();
        }

        private void openPcap(bool asMessages)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.AddExtension = true;
            openFile.Filter = "Packet Captures (*.pcap;*.pcapng)|*.pcap;*.pcapng|All Files (*.*)|*.*";

            if (openFile.ShowDialog() != DialogResult.OK)
                return;

            loadedAsMessages = asMessages;

            loadPcap(openFile.FileName, asMessages);
        }

        private void menuItem_Open_Click(object sender, EventArgs e)
        {
            openPcap(false);
        }

        private void menuItem_OpenAsMessages_Click(object sender, EventArgs e)
        {
            openPcap(true);
        }

        private void mnuItem_EditPreviousHighlightedRow_Click(object sender, EventArgs e)
        {
            if (listView_Packets.TopItem == null)
                return;

            for (int i = listView_Packets.TopItem.Index - 1; i >= 0; i--)
            {
                if (listView_Packets.Items[i].BackColor != SystemColors.Window)
                {
                    listView_Packets.TopItem = listView_Packets.Items[i];
                    listView_Packets.TopItem.Selected = true;
                    listView_Packets.TopItem.Focused = true;
                    break;
                }
            }
        }

        private void mnuItem_EditNextHighlightedRow_Click(object sender, EventArgs e)
        {
            if (listView_Packets.TopItem == null)
                return;

            for (int i = listView_Packets.TopItem.Index + 1; i < listView_Packets.Items.Count; i++)
            {
                if (listView_Packets.Items[i].BackColor != SystemColors.Window)
                {
                    listView_Packets.TopItem = listView_Packets.Items[i];
                    listView_Packets.TopItem.Selected = true;
                    listView_Packets.TopItem.Focused = true;
                    break;
                }
            }
        }


        private void menuItem_ToolCount_Click(object sender, EventArgs e) {
            FolderBrowserDialog openFolder = new FolderBrowserDialog();

            if (openFolder.ShowDialog() != DialogResult.OK)
                return;

            List<string> files = new List<string>();
            files.AddRange(Directory.GetFiles(openFolder.SelectedPath, "*.pcap", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(openFolder.SelectedPath, "*.pcapng", SearchOption.AllDirectories));

            OrderedDictionary opcodeOccurrences = new OrderedDictionary();

            foreach (PacketOpcode opcode in Enum.GetValues(typeof(PacketOpcode)))
                opcodeOccurrences[opcode] = 0;

            foreach (string file in files)
            {
                loadPcap(file, false, true);

                foreach (PacketRecord record in records)
                {
                    foreach (PacketOpcode opcode in record.opcodes)
                    {
                        if (opcodeOccurrences.Contains(opcode))
                            opcodeOccurrences[opcode] = (Int32)opcodeOccurrences[opcode] + 1;
                        else
                            opcodeOccurrences[opcode] = 1;
                    }
                }
            }

            long totalCount = 0;
            StringBuilder occurencesString = new StringBuilder();
            foreach (DictionaryEntry entry in opcodeOccurrences)
            {
                occurencesString.Append(entry.Key);
                occurencesString.Append(" = ");
                occurencesString.Append(entry.Value);
                occurencesString.Append("\r\n");

                totalCount += (Int32)entry.Value;
            }

            occurencesString.Append("\r\n\r\nTotal Count = ");
            occurencesString.Append(totalCount);
            occurencesString.Append("\r\n");

            TextPopup popup = new TextPopup();
            popup.setText(occurencesString.ToString());
            popup.setText(occurencesString.ToString() + "\r\n\r\n" + String.Join("\r\n", files));
            popup.ShowDialog();
        }

        private void menuItem_ToolBad_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFolder = new FolderBrowserDialog();

            if (openFolder.ShowDialog() != DialogResult.OK)
                return;

            List<string> files = new List<string>();
            files.AddRange(Directory.GetFiles(openFolder.SelectedPath, "*.pcap", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(openFolder.SelectedPath, "*.pcapng", SearchOption.AllDirectories));

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

                            BinaryReader fragDataReader = new BinaryReader(new MemoryStream(frag.dat_));

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
                catch (Exception ex)
                {
                    MessageBox.Show("Packet " + curPacket + " Fragment " + curFragment + " EXCEPTION: " + ex.Message);
                    break;
                }
            }
        }

        private void menuItem_ToolHeatmap_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFolder = new FolderBrowserDialog();

            if (openFolder.ShowDialog() != DialogResult.OK)
                return;

            List<string> files = new List<string>();
            files.AddRange(Directory.GetFiles(openFolder.SelectedPath, "*.pcap", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(openFolder.SelectedPath, "*.pcapng", SearchOption.AllDirectories));

            uint packetCount = 0;
            uint messageCount = 0;
            uint[,] heatmap = new uint[256, 256];
            foreach (string file in files)
            {
                loadPcap(file, false, true);

                foreach (PacketRecord record in records)
                {
                    packetCount++;
                    foreach (BlobFrag frag in record.frags)
                    {
                        if (frag.memberHeader_.blobNum == 0)
                            messageCount++;

                        if (frag.dat_.Length > 20)
                        {
                            BinaryReader fragDataReader = new BinaryReader(new MemoryStream(frag.dat_));
                            fragDataReader.ReadUInt32();
                            fragDataReader.ReadUInt32();
                            if ((PacketOpcode)fragDataReader.ReadUInt32() == PacketOpcode.Evt_Movement__AutonomousPosition_ID)
                            {
                                uint objcell_id = fragDataReader.ReadUInt32();
                                uint x = (objcell_id >> 24) & 0xFF;
                                uint y = 255 - ((objcell_id >> 16) & 0xFF);
                                heatmap[x, y] = 1;
                            }
                        }
                    }
                }
            }

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            Stream imageStream = assembly.GetManifestResourceStream("aclogview.map.png");
            Bitmap heatmapImg = new Bitmap(imageStream);
            for (int y = 0; y < 256; ++y)
            {
                for (int x = 0; x < 256; ++x)
                {
                    if (heatmap[x, y] > 0)
                    {
                        Color curColor = heatmapImg.GetPixel(x, y);
                        heatmapImg.SetPixel(x, y, Color.FromArgb(255, Math.Min(255, 200 + curColor.R), curColor.G, curColor.B));
                    }
                }
            }

            ImagePopup popup = new ImagePopup();
            popup.Text = "Coverage Map - " + packetCount + " packets, " + messageCount + " messages";
            popup.ClientSize = new Size(512, 512);
            popup.setImage(heatmapImg);
            popup.ShowDialog();
        }

        private void mnuItem_ToolFindOpcodeInFiles_Click(object sender, EventArgs e)
        {
            var form = new FindOpcodeInFilesForm();
            form.Show(this);
        }

        private void mnuItem_ToolFragDatListTool_Click(object sender, EventArgs e)
        {
            var form = new FragDatListToolForm();
            form.Show(this);
        }

        private void menuItem_About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("aclogview\n\nA program to view and parse Asheron's Call 1 packet capture files (.pcap) generated by aclog.\n\nFor more info and source code, see https://github.com/tfarley/aclogview", "About");
        }


        private void checkBox_HideHeaderOnly_CheckedChanged(object sender, EventArgs e)
        {
            listView_Packets.RedrawItems(0, records.Count - 1, false);
            updateData();
        }

        private void checkBox_useHighlighting_CheckedChanged(object sender, EventArgs e)
        {
            updateText();
        }

        private void parsedContextMenu_Click(object sender, EventArgs e)
        {

            if (treeView_ParsedData.Nodes.Count > 0)
            {
                strbuilder.Clear();
                foreach (var node in GetTreeNodes(treeView_ParsedData.Nodes))
                {
                    strbuilder.AppendLine(node.Text);
                }
                Clipboard.SetText(strbuilder.ToString());
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
            updateData();
            if (listView_Packets.FocusedItem != null)
            {
                 listView_Packets.Focus();
            }
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

        private void textBox_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnHighlight_Click(this, new EventArgs());
            }
        }

        private void btnHighlight_Click(object sender, EventArgs e)
        {
            var searchString = textBox_Search.Text;

            if (searchString.Length == 0)
            {
                return;
            }
            else if (searchString.Length == 6)
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
            else if(CHexTest(searchString))
            {
                currentOpcode = Int32.Parse(searchString.Remove(0,2), System.Globalization.NumberStyles.HexNumber);
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
            } else
            {
                toolStripStatus.Text = "Invalid hex code.";
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

        private void menuItem_ReOpen_Click(object sender, EventArgs e)
        {
            loadPcap(pcapFilePath, false);
        }

        private void menuItem_ReOpenAsMessages_Click(object sender, EventArgs e)
        {
            loadPcap(pcapFilePath, true);
        }

        private void textBox_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // ENTER KEY
                btnHighlight.PerformClick();
        }
    }
}

