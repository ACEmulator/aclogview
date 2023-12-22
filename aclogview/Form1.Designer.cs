namespace aclogview {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            splitContainer_Main = new System.Windows.Forms.SplitContainer();
            splitContainer_Top = new System.Windows.Forms.SplitContainer();
            listView_Packets = new CustomListView();
            lineNumberColumn = new System.Windows.Forms.ColumnHeader();
            sendReceiveColumn = new System.Windows.Forms.ColumnHeader();
            timeColumn = new System.Windows.Forms.ColumnHeader();
            headersColumn = new System.Windows.Forms.ColumnHeader();
            typeColumn = new System.Windows.Forms.ColumnHeader();
            sizeColumn = new System.Windows.Forms.ColumnHeader();
            extraInfoColumn = new System.Windows.Forms.ColumnHeader();
            hexOpcodeColumn = new System.Windows.Forms.ColumnHeader();
            packSeqColumn = new System.Windows.Forms.ColumnHeader();
            queueColumn = new System.Windows.Forms.ColumnHeader();
            iterationColumn = new System.Windows.Forms.ColumnHeader();
            serverPortColumn = new System.Windows.Forms.ColumnHeader();
            listView_CreatedObjects = new System.Windows.Forms.ListView();
            columnHeader8 = new System.Windows.Forms.ColumnHeader();
            columnHeader9 = new System.Windows.Forms.ColumnHeader();
            columnHeader10 = new System.Windows.Forms.ColumnHeader();
            columnHeader11 = new System.Windows.Forms.ColumnHeader();
            columnHeader12 = new System.Windows.Forms.ColumnHeader();
            splitContainer_Bottom = new System.Windows.Forms.SplitContainer();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabHexView = new System.Windows.Forms.TabPage();
            hexBox1 = new Be.Windows.Forms.HexBox();
            hexContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            copyTextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            copyHexMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tabProtocolDocs = new System.Windows.Forms.TabPage();
            protocolWebBrowser = new System.Windows.Forms.WebBrowser();
            treeView_ParsedData = new BufferedTreeView();
            parsedContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            ExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            CollapseAll = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            CopyAll = new System.Windows.Forms.ToolStripMenuItem();
            TeleLoc = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            FindID = new System.Windows.Forms.ToolStripMenuItem();
            listviewContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            copyTimeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            objectsContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            jumpToMessageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            highlightObjectIDMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            textBox_Search = new System.Windows.Forms.TextBox();
            pictureBox_Search = new System.Windows.Forms.PictureBox();
            statusStrip = new System.Windows.Forms.StatusStrip();
            toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            checkBox_HideHeaderOnly = new System.Windows.Forms.CheckBox();
            checkBox_useHighlighting = new System.Windows.Forms.CheckBox();
            checkBoxUseHex = new System.Windows.Forms.CheckBox();
            CmdLock = new System.Windows.Forms.Button();
            cmdforward = new System.Windows.Forms.Button();
            cmdbackward = new System.Windows.Forms.Button();
            lblTracker = new System.Windows.Forms.Label();
            btnHighlight = new System.Windows.Forms.Button();
            checkBox_ShowObjects = new System.Windows.Forms.CheckBox();
            HighlightMode_comboBox = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            columnsContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            sendReceiveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            timeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            headersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            typeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            sizeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            extraInfoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            opcodeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            packSeqMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            queueMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            iterationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            serverPortMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openAsMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openAsFragmentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            reOpenAsMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            reOpenAsFragmentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            previousHighlightedRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            nextHighlightedRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            goToLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            findOpcodeInFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            findTextInFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pcapScraperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            combatScraperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            findBadParsersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            fragDatListToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip2 = new System.Windows.Forms.MenuStrip();
            ((System.ComponentModel.ISupportInitialize)splitContainer_Main).BeginInit();
            splitContainer_Main.Panel1.SuspendLayout();
            splitContainer_Main.Panel2.SuspendLayout();
            splitContainer_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer_Top).BeginInit();
            splitContainer_Top.Panel1.SuspendLayout();
            splitContainer_Top.Panel2.SuspendLayout();
            splitContainer_Top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer_Bottom).BeginInit();
            splitContainer_Bottom.Panel1.SuspendLayout();
            splitContainer_Bottom.Panel2.SuspendLayout();
            splitContainer_Bottom.SuspendLayout();
            tabControl1.SuspendLayout();
            tabHexView.SuspendLayout();
            hexContextMenu.SuspendLayout();
            tabProtocolDocs.SuspendLayout();
            parsedContextMenu.SuspendLayout();
            listviewContextMenu.SuspendLayout();
            objectsContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Search).BeginInit();
            statusStrip.SuspendLayout();
            columnsContextMenu.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer_Main
            // 
            splitContainer_Main.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            splitContainer_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer_Main.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer_Main.Location = new System.Drawing.Point(0, 56);
            splitContainer_Main.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer_Main.Name = "splitContainer_Main";
            splitContainer_Main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Main.Panel1
            // 
            splitContainer_Main.Panel1.Controls.Add(splitContainer_Top);
            splitContainer_Main.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer_Main.Panel2
            // 
            splitContainer_Main.Panel2.Controls.Add(splitContainer_Bottom);
            splitContainer_Main.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            splitContainer_Main.Size = new System.Drawing.Size(1773, 760);
            splitContainer_Main.SplitterDistance = 276;
            splitContainer_Main.SplitterWidth = 5;
            splitContainer_Main.TabIndex = 0;
            // 
            // splitContainer_Top
            // 
            splitContainer_Top.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            splitContainer_Top.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer_Top.Location = new System.Drawing.Point(0, 0);
            splitContainer_Top.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer_Top.Name = "splitContainer_Top";
            // 
            // splitContainer_Top.Panel1
            // 
            splitContainer_Top.Panel1.Controls.Add(listView_Packets);
            // 
            // splitContainer_Top.Panel2
            // 
            splitContainer_Top.Panel2.Controls.Add(listView_CreatedObjects);
            splitContainer_Top.Panel2Collapsed = true;
            splitContainer_Top.Size = new System.Drawing.Size(1773, 276);
            splitContainer_Top.SplitterDistance = 1086;
            splitContainer_Top.SplitterWidth = 5;
            splitContainer_Top.TabIndex = 1;
            // 
            // listView_Packets
            // 
            listView_Packets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { lineNumberColumn, sendReceiveColumn, timeColumn, headersColumn, typeColumn, sizeColumn, extraInfoColumn, hexOpcodeColumn, packSeqColumn, queueColumn, iterationColumn, serverPortColumn });
            listView_Packets.Dock = System.Windows.Forms.DockStyle.Fill;
            listView_Packets.FullRowSelect = true;
            listView_Packets.Location = new System.Drawing.Point(0, 0);
            listView_Packets.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listView_Packets.MultiSelect = false;
            listView_Packets.Name = "listView_Packets";
            listView_Packets.Size = new System.Drawing.Size(1769, 272);
            listView_Packets.TabIndex = 0;
            listView_Packets.UseCompatibleStateImageBehavior = false;
            listView_Packets.View = System.Windows.Forms.View.Details;
            listView_Packets.VirtualMode = true;
            listView_Packets.ColumnClick += listView_Packets_ColumnClick;
            listView_Packets.RetrieveVirtualItem += listView_Packets_RetrieveVirtualItem;
            listView_Packets.SelectedIndexChanged += listView_Packets_SelectedIndexChanged;
            listView_Packets.MouseClick += listView_Packets_MouseClick;
            // 
            // lineNumberColumn
            // 
            lineNumberColumn.Text = "#";
            lineNumberColumn.Width = 50;
            // 
            // sendReceiveColumn
            // 
            sendReceiveColumn.Text = "S/R";
            sendReceiveColumn.Width = 50;
            // 
            // timeColumn
            // 
            timeColumn.Text = "Epoch Time (s)";
            timeColumn.Width = 84;
            // 
            // headersColumn
            // 
            headersColumn.Text = "Headers";
            headersColumn.Width = 220;
            // 
            // typeColumn
            // 
            typeColumn.Text = "Type";
            typeColumn.Width = 309;
            // 
            // sizeColumn
            // 
            sizeColumn.Text = "Size";
            // 
            // extraInfoColumn
            // 
            extraInfoColumn.Text = "Extra Info";
            extraInfoColumn.Width = 66;
            // 
            // hexOpcodeColumn
            // 
            hexOpcodeColumn.Text = "OpCode";
            hexOpcodeColumn.Width = 66;
            // 
            // packSeqColumn
            // 
            packSeqColumn.Text = "Pack. Seq";
            packSeqColumn.Width = 72;
            // 
            // queueColumn
            // 
            queueColumn.Text = "Queue";
            // 
            // iterationColumn
            // 
            iterationColumn.Text = "Iteration";
            // 
            // serverPortColumn
            // 
            serverPortColumn.Text = "Server Port";
            serverPortColumn.Width = 65;
            // 
            // listView_CreatedObjects
            // 
            listView_CreatedObjects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader8, columnHeader9, columnHeader10, columnHeader11, columnHeader12 });
            listView_CreatedObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            listView_CreatedObjects.FullRowSelect = true;
            listView_CreatedObjects.Location = new System.Drawing.Point(0, 0);
            listView_CreatedObjects.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listView_CreatedObjects.MultiSelect = false;
            listView_CreatedObjects.Name = "listView_CreatedObjects";
            listView_CreatedObjects.Size = new System.Drawing.Size(92, 96);
            listView_CreatedObjects.TabIndex = 0;
            listView_CreatedObjects.UseCompatibleStateImageBehavior = false;
            listView_CreatedObjects.View = System.Windows.Forms.View.Details;
            listView_CreatedObjects.VirtualMode = true;
            listView_CreatedObjects.ColumnClick += listView_CreatedObjects_ColumnClick;
            listView_CreatedObjects.RetrieveVirtualItem += listView_CreatedObjects_RetrieveVirtualItem;
            listView_CreatedObjects.MouseClick += listView_CreatedObjects_MouseClick;
            // 
            // columnHeader8
            // 
            columnHeader8.Text = "#";
            columnHeader8.Width = 50;
            // 
            // columnHeader9
            // 
            columnHeader9.Text = "Object ID";
            columnHeader9.Width = 80;
            // 
            // columnHeader10
            // 
            columnHeader10.Text = "Name";
            columnHeader10.Width = 184;
            // 
            // columnHeader11
            // 
            columnHeader11.Text = "WCID";
            columnHeader11.Width = 56;
            // 
            // columnHeader12
            // 
            columnHeader12.Text = "Type";
            columnHeader12.Width = 186;
            // 
            // splitContainer_Bottom
            // 
            splitContainer_Bottom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            splitContainer_Bottom.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer_Bottom.Location = new System.Drawing.Point(0, 0);
            splitContainer_Bottom.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer_Bottom.Name = "splitContainer_Bottom";
            // 
            // splitContainer_Bottom.Panel1
            // 
            splitContainer_Bottom.Panel1.Controls.Add(tabControl1);
            // 
            // splitContainer_Bottom.Panel2
            // 
            splitContainer_Bottom.Panel2.Controls.Add(treeView_ParsedData);
            splitContainer_Bottom.Size = new System.Drawing.Size(1773, 479);
            splitContainer_Bottom.SplitterDistance = 1252;
            splitContainer_Bottom.SplitterWidth = 5;
            splitContainer_Bottom.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabHexView);
            tabControl1.Controls.Add(tabProtocolDocs);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(1248, 475);
            tabControl1.TabIndex = 0;
            tabControl1.Selected += tabControl1_Selected;
            // 
            // tabHexView
            // 
            tabHexView.Controls.Add(hexBox1);
            tabHexView.Location = new System.Drawing.Point(4, 24);
            tabHexView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabHexView.Name = "tabHexView";
            tabHexView.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabHexView.Size = new System.Drawing.Size(1240, 447);
            tabHexView.TabIndex = 0;
            tabHexView.Text = "Hex View";
            tabHexView.UseVisualStyleBackColor = true;
            // 
            // hexBox1
            // 
            hexBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            hexBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            // 
            // 
            // 
            hexBox1.BuiltInContextMenu.CopyMenuItemText = "Copy";
            hexBox1.BuiltInContextMenu.SelectAllMenuItemText = "Select All";
            hexBox1.ContextMenuStrip = hexContextMenu;
            hexBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            hexBox1.GroupSize = 8;
            hexBox1.LineInfoVisible = true;
            hexBox1.Location = new System.Drawing.Point(4, 3);
            hexBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            hexBox1.Name = "hexBox1";
            hexBox1.ReadOnly = true;
            hexBox1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            hexBox1.ShadowSelectionColor = System.Drawing.Color.FromArgb(100, 60, 188, 255);
            hexBox1.Size = new System.Drawing.Size(1231, 436);
            hexBox1.StringViewVisible = true;
            hexBox1.TabIndex = 1;
            hexBox1.UseFixedBytesPerLine = true;
            hexBox1.VScrollBarVisible = true;
            // 
            // hexContextMenu
            // 
            hexContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { copyTextMenuItem, copyHexMenuItem });
            hexContextMenu.Name = "hexContextMenu";
            hexContextMenu.Size = new System.Drawing.Size(215, 48);
            hexContextMenu.Opening += hexContextMenu_Opening;
            hexContextMenu.ItemClicked += hexContextMenu_ItemClicked;
            // 
            // copyTextMenuItem
            // 
            copyTextMenuItem.Name = "copyTextMenuItem";
            copyTextMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            copyTextMenuItem.Size = new System.Drawing.Size(214, 22);
            copyTextMenuItem.Text = "Copy as &Text";
            // 
            // copyHexMenuItem
            // 
            copyHexMenuItem.Name = "copyHexMenuItem";
            copyHexMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.C;
            copyHexMenuItem.Size = new System.Drawing.Size(214, 22);
            copyHexMenuItem.Text = "Copy as &Hex";
            // 
            // tabProtocolDocs
            // 
            tabProtocolDocs.Controls.Add(protocolWebBrowser);
            tabProtocolDocs.Location = new System.Drawing.Point(4, 24);
            tabProtocolDocs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabProtocolDocs.Name = "tabProtocolDocs";
            tabProtocolDocs.Size = new System.Drawing.Size(1240, 446);
            tabProtocolDocs.TabIndex = 1;
            tabProtocolDocs.Text = "Protocol Documentation";
            tabProtocolDocs.UseVisualStyleBackColor = true;
            // 
            // protocolWebBrowser
            // 
            protocolWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            protocolWebBrowser.Location = new System.Drawing.Point(0, 0);
            protocolWebBrowser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            protocolWebBrowser.MinimumSize = new System.Drawing.Size(23, 23);
            protocolWebBrowser.Name = "protocolWebBrowser";
            protocolWebBrowser.Size = new System.Drawing.Size(1240, 446);
            protocolWebBrowser.TabIndex = 0;
            // 
            // treeView_ParsedData
            // 
            treeView_ParsedData.ContextMenuStrip = parsedContextMenu;
            treeView_ParsedData.Dock = System.Windows.Forms.DockStyle.Fill;
            treeView_ParsedData.Location = new System.Drawing.Point(0, 0);
            treeView_ParsedData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            treeView_ParsedData.Name = "treeView_ParsedData";
            treeView_ParsedData.Size = new System.Drawing.Size(512, 475);
            treeView_ParsedData.TabIndex = 0;
            treeView_ParsedData.AfterSelect += treeView_ParsedData_AfterSelect;
            treeView_ParsedData.NodeMouseClick += treeView_ParsedData_NodeMouseClick;
            // 
            // parsedContextMenu
            // 
            parsedContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ExpandAll, CollapseAll, toolStripSeparator1, CopyAll, TeleLoc, toolStripSeparator2, FindID });
            parsedContextMenu.Name = "parsedContextMenu";
            parsedContextMenu.Size = new System.Drawing.Size(184, 126);
            parsedContextMenu.Opening += parsedContextMenu_Opening;
            parsedContextMenu.ItemClicked += parsedContextMenu_ItemClicked;
            // 
            // ExpandAll
            // 
            ExpandAll.Name = "ExpandAll";
            ExpandAll.Size = new System.Drawing.Size(183, 22);
            ExpandAll.Text = "&Expand All";
            // 
            // CollapseAll
            // 
            CollapseAll.Name = "CollapseAll";
            CollapseAll.Size = new System.Drawing.Size(183, 22);
            CollapseAll.Text = "C&ollapse All";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(180, 6);
            // 
            // CopyAll
            // 
            CopyAll.Name = "CopyAll";
            CopyAll.ShowShortcutKeys = false;
            CopyAll.Size = new System.Drawing.Size(183, 22);
            CopyAll.Text = "&Copy All";
            // 
            // TeleLoc
            // 
            TeleLoc.Name = "TeleLoc";
            TeleLoc.Size = new System.Drawing.Size(183, 22);
            TeleLoc.Text = "Copy ACE @teleloc";
            TeleLoc.Visible = false;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(180, 6);
            // 
            // FindID
            // 
            FindID.Name = "FindID";
            FindID.Size = new System.Drawing.Size(183, 22);
            FindID.Text = "&Find ID In Object List";
            // 
            // listviewContextMenu
            // 
            listviewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { copyTimeMenuItem });
            listviewContextMenu.Name = "listviewContextMenu";
            listviewContextMenu.Size = new System.Drawing.Size(156, 26);
            listviewContextMenu.ItemClicked += listviewContextMenu_ItemClicked;
            // 
            // copyTimeMenuItem
            // 
            copyTimeMenuItem.Name = "copyTimeMenuItem";
            copyTimeMenuItem.Size = new System.Drawing.Size(155, 22);
            copyTimeMenuItem.Text = "Copy time field";
            // 
            // objectsContextMenu
            // 
            objectsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { jumpToMessageMenuItem, highlightObjectIDMenuItem });
            objectsContextMenu.Name = "objectsContextMenu";
            objectsContextMenu.Size = new System.Drawing.Size(189, 48);
            objectsContextMenu.Opening += objectsContextMenu_Opening;
            objectsContextMenu.ItemClicked += objectsContextMenu_ItemClicked;
            // 
            // jumpToMessageMenuItem
            // 
            jumpToMessageMenuItem.Name = "jumpToMessageMenuItem";
            jumpToMessageMenuItem.Size = new System.Drawing.Size(188, 22);
            jumpToMessageMenuItem.Text = "&Jump to this message";
            // 
            // highlightObjectIDMenuItem
            // 
            highlightObjectIDMenuItem.Name = "highlightObjectIDMenuItem";
            highlightObjectIDMenuItem.Size = new System.Drawing.Size(188, 22);
            highlightObjectIDMenuItem.Text = "&Highlight Object ID";
            // 
            // textBox_Search
            // 
            textBox_Search.Location = new System.Drawing.Point(720, 26);
            textBox_Search.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox_Search.MaxLength = 6;
            textBox_Search.Name = "textBox_Search";
            textBox_Search.Size = new System.Drawing.Size(192, 23);
            textBox_Search.TabIndex = 5;
            textBox_Search.KeyPress += textBox_Search_KeyPress;
            // 
            // pictureBox_Search
            // 
            pictureBox_Search.Location = new System.Drawing.Point(1042, 27);
            pictureBox_Search.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBox_Search.Name = "pictureBox_Search";
            pictureBox_Search.Size = new System.Drawing.Size(23, 23);
            pictureBox_Search.TabIndex = 3;
            pictureBox_Search.TabStop = false;
            pictureBox_Search.Visible = false;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatus });
            statusStrip.Location = new System.Drawing.Point(0, 816);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStrip.Size = new System.Drawing.Size(1773, 22);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatus
            // 
            toolStripStatus.Name = "toolStripStatus";
            toolStripStatus.Size = new System.Drawing.Size(74, 17);
            toolStripStatus.Text = "AC Log View";
            // 
            // checkBox_HideHeaderOnly
            // 
            checkBox_HideHeaderOnly.Location = new System.Drawing.Point(1072, 29);
            checkBox_HideHeaderOnly.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBox_HideHeaderOnly.Name = "checkBox_HideHeaderOnly";
            checkBox_HideHeaderOnly.Size = new System.Drawing.Size(180, 20);
            checkBox_HideHeaderOnly.TabIndex = 7;
            checkBox_HideHeaderOnly.Text = "Hide Header Only";
            checkBox_HideHeaderOnly.UseVisualStyleBackColor = true;
            checkBox_HideHeaderOnly.Visible = false;
            checkBox_HideHeaderOnly.CheckedChanged += checkBox_HideHeaderOnly_CheckedChanged;
            // 
            // checkBox_useHighlighting
            // 
            checkBox_useHighlighting.Checked = true;
            checkBox_useHighlighting.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBox_useHighlighting.Enabled = false;
            checkBox_useHighlighting.Location = new System.Drawing.Point(1407, 29);
            checkBox_useHighlighting.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBox_useHighlighting.Name = "checkBox_useHighlighting";
            checkBox_useHighlighting.Size = new System.Drawing.Size(192, 20);
            checkBox_useHighlighting.TabIndex = 9;
            checkBox_useHighlighting.Text = "Use Highlighting (Slower!)";
            checkBox_useHighlighting.UseVisualStyleBackColor = true;
            checkBox_useHighlighting.CheckedChanged += checkBox_useHighlighting_CheckedChanged;
            // 
            // checkBoxUseHex
            // 
            checkBoxUseHex.AutoSize = true;
            checkBoxUseHex.Checked = true;
            checkBoxUseHex.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxUseHex.Enabled = false;
            checkBoxUseHex.Location = new System.Drawing.Point(1259, 29);
            checkBoxUseHex.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxUseHex.Name = "checkBoxUseHex";
            checkBoxUseHex.Size = new System.Drawing.Size(129, 19);
            checkBoxUseHex.TabIndex = 8;
            checkBoxUseHex.Text = "Display Data as Hex";
            checkBoxUseHex.UseVisualStyleBackColor = true;
            checkBoxUseHex.CheckedChanged += checkBoxUseHex_CheckedChanged;
            // 
            // CmdLock
            // 
            CmdLock.Location = new System.Drawing.Point(2, 26);
            CmdLock.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CmdLock.Name = "CmdLock";
            CmdLock.Size = new System.Drawing.Size(88, 27);
            CmdLock.TabIndex = 1;
            CmdLock.Text = "Lock";
            CmdLock.UseVisualStyleBackColor = true;
            CmdLock.Click += CmdLock_Click;
            // 
            // cmdforward
            // 
            cmdforward.Location = new System.Drawing.Point(97, 26);
            cmdforward.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmdforward.Name = "cmdforward";
            cmdforward.Size = new System.Drawing.Size(88, 27);
            cmdforward.TabIndex = 2;
            cmdforward.Text = ">";
            cmdforward.UseVisualStyleBackColor = true;
            cmdforward.Click += cmdforward_Click;
            // 
            // cmdbackward
            // 
            cmdbackward.Location = new System.Drawing.Point(191, 26);
            cmdbackward.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmdbackward.Name = "cmdbackward";
            cmdbackward.Size = new System.Drawing.Size(88, 27);
            cmdbackward.TabIndex = 3;
            cmdbackward.Text = "<";
            cmdbackward.UseVisualStyleBackColor = true;
            cmdbackward.Click += cmdbackward_Click;
            // 
            // lblTracker
            // 
            lblTracker.AutoSize = true;
            lblTracker.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblTracker.Location = new System.Drawing.Point(286, 29);
            lblTracker.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTracker.Name = "lblTracker";
            lblTracker.Size = new System.Drawing.Size(86, 17);
            lblTracker.TabIndex = 4;
            lblTracker.Text = "Viewing #0";
            // 
            // btnHighlight
            // 
            btnHighlight.Enabled = false;
            btnHighlight.Location = new System.Drawing.Point(919, 26);
            btnHighlight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnHighlight.Name = "btnHighlight";
            btnHighlight.Size = new System.Drawing.Size(88, 25);
            btnHighlight.TabIndex = 6;
            btnHighlight.Text = "Highlight";
            btnHighlight.UseVisualStyleBackColor = true;
            btnHighlight.Click += btnHighlight_Click;
            // 
            // checkBox_ShowObjects
            // 
            checkBox_ShowObjects.AutoSize = true;
            checkBox_ShowObjects.Enabled = false;
            checkBox_ShowObjects.Location = new System.Drawing.Point(1606, 29);
            checkBox_ShowObjects.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBox_ShowObjects.Name = "checkBox_ShowObjects";
            checkBox_ShowObjects.Size = new System.Drawing.Size(151, 19);
            checkBox_ShowObjects.TabIndex = 10;
            checkBox_ShowObjects.Text = "Display Created Objects";
            checkBox_ShowObjects.UseVisualStyleBackColor = true;
            checkBox_ShowObjects.CheckedChanged += checkBox_ShowObjects_CheckedChanged;
            // 
            // HighlightMode_comboBox
            // 
            HighlightMode_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            HighlightMode_comboBox.Location = new System.Drawing.Point(552, 26);
            HighlightMode_comboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            HighlightMode_comboBox.Name = "HighlightMode_comboBox";
            HighlightMode_comboBox.Size = new System.Drawing.Size(160, 23);
            HighlightMode_comboBox.TabIndex = 0;
            HighlightMode_comboBox.SelectedIndexChanged += HighlightMode_comboBox_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(457, 30);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(94, 15);
            label1.TabIndex = 11;
            label1.Text = "Highlight Mode:";
            // 
            // columnsContextMenu
            // 
            columnsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { sendReceiveMenuItem, timeMenuItem, headersMenuItem, typeMenuItem, sizeMenuItem, extraInfoMenuItem, opcodeMenuItem, packSeqMenuItem, queueMenuItem, iterationMenuItem, serverPortMenuItem });
            columnsContextMenu.Name = "columnsContextMenu";
            columnsContextMenu.Size = new System.Drawing.Size(132, 246);
            columnsContextMenu.ItemClicked += columnsContextMenu_ItemClicked;
            // 
            // sendReceiveMenuItem
            // 
            sendReceiveMenuItem.Checked = true;
            sendReceiveMenuItem.CheckOnClick = true;
            sendReceiveMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            sendReceiveMenuItem.Name = "sendReceiveMenuItem";
            sendReceiveMenuItem.Size = new System.Drawing.Size(131, 22);
            sendReceiveMenuItem.Text = "S/R";
            // 
            // timeMenuItem
            // 
            timeMenuItem.Checked = true;
            timeMenuItem.CheckOnClick = true;
            timeMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            timeMenuItem.Name = "timeMenuItem";
            timeMenuItem.Size = new System.Drawing.Size(131, 22);
            timeMenuItem.Text = "Time";
            // 
            // headersMenuItem
            // 
            headersMenuItem.Checked = true;
            headersMenuItem.CheckOnClick = true;
            headersMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            headersMenuItem.Name = "headersMenuItem";
            headersMenuItem.Size = new System.Drawing.Size(131, 22);
            headersMenuItem.Text = "Headers";
            // 
            // typeMenuItem
            // 
            typeMenuItem.Checked = true;
            typeMenuItem.CheckOnClick = true;
            typeMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            typeMenuItem.Name = "typeMenuItem";
            typeMenuItem.Size = new System.Drawing.Size(131, 22);
            typeMenuItem.Text = "Type";
            // 
            // sizeMenuItem
            // 
            sizeMenuItem.Checked = true;
            sizeMenuItem.CheckOnClick = true;
            sizeMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            sizeMenuItem.Name = "sizeMenuItem";
            sizeMenuItem.Size = new System.Drawing.Size(131, 22);
            sizeMenuItem.Text = "Size";
            // 
            // extraInfoMenuItem
            // 
            extraInfoMenuItem.Checked = true;
            extraInfoMenuItem.CheckOnClick = true;
            extraInfoMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            extraInfoMenuItem.Name = "extraInfoMenuItem";
            extraInfoMenuItem.Size = new System.Drawing.Size(131, 22);
            extraInfoMenuItem.Text = "Extra Info";
            // 
            // opcodeMenuItem
            // 
            opcodeMenuItem.Checked = true;
            opcodeMenuItem.CheckOnClick = true;
            opcodeMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            opcodeMenuItem.Name = "opcodeMenuItem";
            opcodeMenuItem.Size = new System.Drawing.Size(131, 22);
            opcodeMenuItem.Text = "OpCode";
            // 
            // packSeqMenuItem
            // 
            packSeqMenuItem.Checked = true;
            packSeqMenuItem.CheckOnClick = true;
            packSeqMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            packSeqMenuItem.Name = "packSeqMenuItem";
            packSeqMenuItem.Size = new System.Drawing.Size(131, 22);
            packSeqMenuItem.Text = "Pack. Seq";
            // 
            // queueMenuItem
            // 
            queueMenuItem.Checked = true;
            queueMenuItem.CheckOnClick = true;
            queueMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            queueMenuItem.Name = "queueMenuItem";
            queueMenuItem.Size = new System.Drawing.Size(131, 22);
            queueMenuItem.Text = "Queue";
            // 
            // iterationMenuItem
            // 
            iterationMenuItem.Checked = true;
            iterationMenuItem.CheckOnClick = true;
            iterationMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            iterationMenuItem.Name = "iterationMenuItem";
            iterationMenuItem.Size = new System.Drawing.Size(131, 22);
            iterationMenuItem.Text = "Iteration";
            // 
            // serverPortMenuItem
            // 
            serverPortMenuItem.Checked = true;
            serverPortMenuItem.CheckOnClick = true;
            serverPortMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            serverPortMenuItem.Name = "serverPortMenuItem";
            serverPortMenuItem.Size = new System.Drawing.Size(131, 22);
            serverPortMenuItem.Text = "Server Port";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(1773, 24);
            menuStrip1.TabIndex = 12;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { openAsMessagesToolStripMenuItem, openAsFragmentsToolStripMenuItem, reOpenAsMessagesToolStripMenuItem, reOpenAsFragmentsToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openAsMessagesToolStripMenuItem
            // 
            openAsMessagesToolStripMenuItem.Name = "openAsMessagesToolStripMenuItem";
            openAsMessagesToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            openAsMessagesToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            openAsMessagesToolStripMenuItem.Text = "Open As Messages";
            openAsMessagesToolStripMenuItem.Click += openAsMessagesToolStripMenuItem_Click;
            // 
            // openAsFragmentsToolStripMenuItem
            // 
            openAsFragmentsToolStripMenuItem.Name = "openAsFragmentsToolStripMenuItem";
            openAsFragmentsToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            openAsFragmentsToolStripMenuItem.Text = "Open As Fragments";
            openAsFragmentsToolStripMenuItem.Click += openAsFragmentsToolStripMenuItem_Click;
            // 
            // reOpenAsMessagesToolStripMenuItem
            // 
            reOpenAsMessagesToolStripMenuItem.Enabled = false;
            reOpenAsMessagesToolStripMenuItem.Name = "reOpenAsMessagesToolStripMenuItem";
            reOpenAsMessagesToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.O;
            reOpenAsMessagesToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            reOpenAsMessagesToolStripMenuItem.Text = "Re-Open As Messages";
            reOpenAsMessagesToolStripMenuItem.Click += reOpenAsMessagesToolStripMenuItem_Click;
            // 
            // reOpenAsFragmentsToolStripMenuItem
            // 
            reOpenAsFragmentsToolStripMenuItem.Enabled = false;
            reOpenAsFragmentsToolStripMenuItem.Name = "reOpenAsFragmentsToolStripMenuItem";
            reOpenAsFragmentsToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            reOpenAsFragmentsToolStripMenuItem.Text = "Re-Open As Fragments";
            reOpenAsFragmentsToolStripMenuItem.Click += reOpenAsFragmentsToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { previousHighlightedRowToolStripMenuItem, nextHighlightedRowToolStripMenuItem, toolStripSeparator3, goToLineToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // previousHighlightedRowToolStripMenuItem
            // 
            previousHighlightedRowToolStripMenuItem.Name = "previousHighlightedRowToolStripMenuItem";
            previousHighlightedRowToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F3;
            previousHighlightedRowToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
            previousHighlightedRowToolStripMenuItem.Text = "Previous Highlighted Row";
            previousHighlightedRowToolStripMenuItem.Click += previousHighlightedRowToolStripMenuItem_Click;
            // 
            // nextHighlightedRowToolStripMenuItem
            // 
            nextHighlightedRowToolStripMenuItem.Name = "nextHighlightedRowToolStripMenuItem";
            nextHighlightedRowToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            nextHighlightedRowToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
            nextHighlightedRowToolStripMenuItem.Text = "Next Highlighted Row";
            nextHighlightedRowToolStripMenuItem.Click += nextHighlightedRowToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(259, 6);
            // 
            // goToLineToolStripMenuItem
            // 
            goToLineToolStripMenuItem.Enabled = false;
            goToLineToolStripMenuItem.Name = "goToLineToolStripMenuItem";
            goToLineToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G;
            goToLineToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
            goToLineToolStripMenuItem.Text = "Go To Line";
            goToLineToolStripMenuItem.Click += goToLineToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { findOpcodeInFilesToolStripMenuItem, findTextInFilesToolStripMenuItem, pcapScraperToolStripMenuItem, combatScraperToolStripMenuItem, toolStripSeparator4, findBadParsersToolStripMenuItem, fragDatListToolToolStripMenuItem, toolStripSeparator5, optionsToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // findOpcodeInFilesToolStripMenuItem
            // 
            findOpcodeInFilesToolStripMenuItem.Name = "findOpcodeInFilesToolStripMenuItem";
            findOpcodeInFilesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            findOpcodeInFilesToolStripMenuItem.Text = "Find Opcode In Files";
            findOpcodeInFilesToolStripMenuItem.Click += findOpcodeInFilesToolStripMenuItem_Click;
            // 
            // findTextInFilesToolStripMenuItem
            // 
            findTextInFilesToolStripMenuItem.Name = "findTextInFilesToolStripMenuItem";
            findTextInFilesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            findTextInFilesToolStripMenuItem.Text = "Find Text In Files";
            findTextInFilesToolStripMenuItem.Click += findTextInFilesToolStripMenuItem_Click;
            // 
            // pcapScraperToolStripMenuItem
            // 
            pcapScraperToolStripMenuItem.Name = "pcapScraperToolStripMenuItem";
            pcapScraperToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            pcapScraperToolStripMenuItem.Text = "Pcap Scraper";
            pcapScraperToolStripMenuItem.Click += pcapScraperToolStripMenuItem_Click;
            // 
            // combatScraperToolStripMenuItem
            // 
            combatScraperToolStripMenuItem.Name = "combatScraperToolStripMenuItem";
            combatScraperToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            combatScraperToolStripMenuItem.Text = "Combat Scraper";
            combatScraperToolStripMenuItem.Click += combatScraperToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(178, 6);
            // 
            // findBadParsersToolStripMenuItem
            // 
            findBadParsersToolStripMenuItem.Name = "findBadParsersToolStripMenuItem";
            findBadParsersToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            findBadParsersToolStripMenuItem.Text = "Find Bad Parsers";
            findBadParsersToolStripMenuItem.Click += findBadParsersToolStripMenuItem_Click;
            // 
            // fragDatListToolToolStripMenuItem
            // 
            fragDatListToolToolStripMenuItem.Name = "fragDatListToolToolStripMenuItem";
            fragDatListToolToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            fragDatListToolToolStripMenuItem.Text = "Frag Dat List Tool";
            fragDatListToolToolStripMenuItem.Click += fragDatListToolToolStripMenuItem_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(178, 6);
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            optionsToolStripMenuItem.Text = "Options";
            optionsToolStripMenuItem.Click += optionsToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { checkForUpdatesToolStripMenuItem, aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            checkForUpdatesToolStripMenuItem.Text = "Check for updates...";
            checkForUpdatesToolStripMenuItem.Click += checkForUpdatesToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // menuStrip2
            // 
            menuStrip2.AutoSize = false;
            menuStrip2.Location = new System.Drawing.Point(0, 24);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip2.Size = new System.Drawing.Size(1773, 32);
            menuStrip2.TabIndex = 13;
            menuStrip2.Text = "menuStrip2";
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1773, 838);
            Controls.Add(label1);
            Controls.Add(HighlightMode_comboBox);
            Controls.Add(checkBox_ShowObjects);
            Controls.Add(btnHighlight);
            Controls.Add(checkBoxUseHex);
            Controls.Add(lblTracker);
            Controls.Add(cmdbackward);
            Controls.Add(cmdforward);
            Controls.Add(CmdLock);
            Controls.Add(checkBox_useHighlighting);
            Controls.Add(checkBox_HideHeaderOnly);
            Controls.Add(splitContainer_Main);
            Controls.Add(textBox_Search);
            Controls.Add(pictureBox_Search);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip2);
            Controls.Add(menuStrip1);
            DoubleBuffered = true;
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "Form1";
            Text = "AC Log View";
            Load += Form1_Load;
            splitContainer_Main.Panel1.ResumeLayout(false);
            splitContainer_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer_Main).EndInit();
            splitContainer_Main.ResumeLayout(false);
            splitContainer_Top.Panel1.ResumeLayout(false);
            splitContainer_Top.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer_Top).EndInit();
            splitContainer_Top.ResumeLayout(false);
            splitContainer_Bottom.Panel1.ResumeLayout(false);
            splitContainer_Bottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer_Bottom).EndInit();
            splitContainer_Bottom.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabHexView.ResumeLayout(false);
            hexContextMenu.ResumeLayout(false);
            tabProtocolDocs.ResumeLayout(false);
            parsedContextMenu.ResumeLayout(false);
            listviewContextMenu.ResumeLayout(false);
            objectsContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox_Search).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            columnsContextMenu.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer_Main;
        private CustomListView listView_Packets;
        private System.Windows.Forms.SplitContainer splitContainer_Bottom;
        private System.Windows.Forms.TextBox textBox_Search;
        private System.Windows.Forms.PictureBox pictureBox_Search;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ColumnHeader lineNumberColumn;
        private System.Windows.Forms.ColumnHeader timeColumn;
        private System.Windows.Forms.ColumnHeader typeColumn;
        private System.Windows.Forms.ColumnHeader sizeColumn;
        private System.Windows.Forms.ColumnHeader sendReceiveColumn;
        private System.Windows.Forms.ColumnHeader extraInfoColumn;
        private System.Windows.Forms.ColumnHeader headersColumn;
        private BufferedTreeView treeView_ParsedData;
        private System.Windows.Forms.CheckBox checkBox_HideHeaderOnly;
        private System.Windows.Forms.CheckBox checkBox_useHighlighting;
        private System.Windows.Forms.ContextMenuStrip parsedContextMenu;
        private System.Windows.Forms.ToolStripMenuItem CopyAll;
        private System.Windows.Forms.Button CmdLock;
        private System.Windows.Forms.Button cmdforward;
        private System.Windows.Forms.Button cmdbackward;
        private System.Windows.Forms.Label lblTracker;
        private System.Windows.Forms.ColumnHeader hexOpcodeColumn;
        private System.Windows.Forms.CheckBox checkBoxUseHex;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.Button btnHighlight;
        private System.Windows.Forms.ToolStripMenuItem ExpandAll;
        private System.Windows.Forms.ToolStripMenuItem CollapseAll;
        private System.Windows.Forms.SplitContainer splitContainer_Top;
        private System.Windows.Forms.ListView listView_CreatedObjects;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.CheckBox checkBox_ShowObjects;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ComboBox HighlightMode_comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip objectsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem jumpToMessageMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FindID;
        private System.Windows.Forms.ToolStripMenuItem highlightObjectIDMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabHexView;
        private Be.Windows.Forms.HexBox hexBox1;
        private System.Windows.Forms.ContextMenuStrip hexContextMenu;
        private System.Windows.Forms.ToolStripMenuItem copyHexMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyTextMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TabPage tabProtocolDocs;
        private System.Windows.Forms.WebBrowser protocolWebBrowser;
        private System.Windows.Forms.ToolStripMenuItem TeleLoc;
        private System.Windows.Forms.ContextMenuStrip listviewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem copyTimeMenuItem;
        private System.Windows.Forms.ColumnHeader packSeqColumn;
        private System.Windows.Forms.ColumnHeader queueColumn;
        private System.Windows.Forms.ColumnHeader iterationColumn;
        private System.Windows.Forms.ColumnHeader serverPortColumn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem sendReceiveMenuItem;
        internal System.Windows.Forms.ContextMenuStrip columnsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem timeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem headersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem typeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sizeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extraInfoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem opcodeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem packSeqMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queueMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iterationMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverPortMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openAsMessagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openAsFragmentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reOpenAsMessagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reOpenAsFragmentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previousHighlightedRowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextHighlightedRowToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem goToLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findOpcodeInFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findTextInFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pcapScraperToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem combatScraperToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem findBadParsersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fragDatListToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

