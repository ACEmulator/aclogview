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
        private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.splitContainer_Main = new System.Windows.Forms.SplitContainer();
			this.splitContainer_Top = new System.Windows.Forms.SplitContainer();
			this.listView_Packets = new CustomListView();
			this.lineNumberColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.sendReceiveColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.timeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headersColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.typeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.sizeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.extraInfoColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.hexOpcodeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.packSeqColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.queueColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.iterationColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.serverPortColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.listView_CreatedObjects = new System.Windows.Forms.ListView();
			this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.splitContainer_Bottom = new System.Windows.Forms.SplitContainer();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabHexView = new System.Windows.Forms.TabPage();
			this.hexBox1 = new Be.Windows.Forms.HexBox();
			this.hexContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyTextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyHexMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabProtocolDocs = new System.Windows.Forms.TabPage();
			this.protocolWebBrowser = new System.Windows.Forms.WebBrowser();
			this.treeView_ParsedData = new BufferedTreeView();
			this.parsedContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.ExpandAll = new System.Windows.Forms.ToolStripMenuItem();
			this.CollapseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.CopyAll = new System.Windows.Forms.ToolStripMenuItem();
			this.TeleLoc = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.FindID = new System.Windows.Forms.ToolStripMenuItem();
			this.listviewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyTimeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.objectsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.jumpToMessageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.highlightObjectIDMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.textBox_Search = new System.Windows.Forms.TextBox();
			this.pictureBox_Search = new System.Windows.Forms.PictureBox();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.checkBox_HideHeaderOnly = new System.Windows.Forms.CheckBox();
			this.checkBox_useHighlighting = new System.Windows.Forms.CheckBox();
			this.checkBoxUseHex = new System.Windows.Forms.CheckBox();
			this.CmdLock = new System.Windows.Forms.Button();
			this.cmdforward = new System.Windows.Forms.Button();
			this.cmdbackward = new System.Windows.Forms.Button();
			this.lblTracker = new System.Windows.Forms.Label();
			this.btnHighlight = new System.Windows.Forms.Button();
			this.checkBox_ShowObjects = new System.Windows.Forms.CheckBox();
			this.HighlightMode_comboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.columnsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.sendReceiveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.timeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.headersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.typeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sizeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.extraInfoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.opcodeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.packSeqMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.queueMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.iterationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.serverPortMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.menuStrip2 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openAsMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openAsFragmentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.reOpenAsMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.reOpenAsFragmentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.previousHighlightedRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nextHighlightedRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.goToLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.findOpcodeInFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.findTextInFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pcapScraperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.combatScraperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.findBadParsersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fragDatListToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).BeginInit();
			this.splitContainer_Main.Panel1.SuspendLayout();
			this.splitContainer_Main.Panel2.SuspendLayout();
			this.splitContainer_Main.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer_Top)).BeginInit();
			this.splitContainer_Top.Panel1.SuspendLayout();
			this.splitContainer_Top.Panel2.SuspendLayout();
			this.splitContainer_Top.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer_Bottom)).BeginInit();
			this.splitContainer_Bottom.Panel1.SuspendLayout();
			this.splitContainer_Bottom.Panel2.SuspendLayout();
			this.splitContainer_Bottom.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabHexView.SuspendLayout();
			this.hexContextMenu.SuspendLayout();
			this.tabProtocolDocs.SuspendLayout();
			this.parsedContextMenu.SuspendLayout();
			this.listviewContextMenu.SuspendLayout();
			this.objectsContextMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox_Search)).BeginInit();
			this.statusStrip.SuspendLayout();
			this.columnsContextMenu.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer_Main
			// 
			this.splitContainer_Main.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer_Main.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer_Main.Location = new System.Drawing.Point(0, 48);
			this.splitContainer_Main.Name = "splitContainer_Main";
			this.splitContainer_Main.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer_Main.Panel1
			// 
			this.splitContainer_Main.Panel1.Controls.Add(this.splitContainer_Top);
			this.splitContainer_Main.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			// 
			// splitContainer_Main.Panel2
			// 
			this.splitContainer_Main.Panel2.Controls.Add(this.splitContainer_Bottom);
			this.splitContainer_Main.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.splitContainer_Main.Size = new System.Drawing.Size(1520, 656);
			this.splitContainer_Main.SplitterDistance = 238;
			this.splitContainer_Main.TabIndex = 0;
			// 
			// splitContainer_Top
			// 
			this.splitContainer_Top.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer_Top.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer_Top.Location = new System.Drawing.Point(0, 0);
			this.splitContainer_Top.Name = "splitContainer_Top";
			// 
			// splitContainer_Top.Panel1
			// 
			this.splitContainer_Top.Panel1.Controls.Add(this.listView_Packets);
			// 
			// splitContainer_Top.Panel2
			// 
			this.splitContainer_Top.Panel2.Controls.Add(this.listView_CreatedObjects);
			this.splitContainer_Top.Panel2Collapsed = true;
			this.splitContainer_Top.Size = new System.Drawing.Size(1520, 238);
			this.splitContainer_Top.SplitterDistance = 931;
			this.splitContainer_Top.TabIndex = 1;
			// 
			// listView_Packets
			// 
			this.listView_Packets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lineNumberColumn,
            this.sendReceiveColumn,
            this.timeColumn,
            this.headersColumn,
            this.typeColumn,
            this.sizeColumn,
            this.extraInfoColumn,
            this.hexOpcodeColumn,
            this.packSeqColumn,
            this.queueColumn,
            this.iterationColumn,
            this.serverPortColumn});
			this.listView_Packets.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView_Packets.FullRowSelect = true;
			this.listView_Packets.HideSelection = false;
			this.listView_Packets.Location = new System.Drawing.Point(0, 0);
			this.listView_Packets.MultiSelect = false;
			this.listView_Packets.Name = "listView_Packets";
			this.listView_Packets.Size = new System.Drawing.Size(1516, 234);
			this.listView_Packets.TabIndex = 0;
			this.listView_Packets.UseCompatibleStateImageBehavior = false;
			this.listView_Packets.View = System.Windows.Forms.View.Details;
			this.listView_Packets.VirtualMode = true;
			this.listView_Packets.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_Packets_ColumnClick);
			this.listView_Packets.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listView_Packets_RetrieveVirtualItem);
			this.listView_Packets.SelectedIndexChanged += new System.EventHandler(this.listView_Packets_SelectedIndexChanged);
			this.listView_Packets.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_Packets_MouseClick);
			// 
			// lineNumberColumn
			// 
			this.lineNumberColumn.Text = "#";
			this.lineNumberColumn.Width = 50;
			// 
			// sendReceiveColumn
			// 
			this.sendReceiveColumn.Text = "S/R";
			this.sendReceiveColumn.Width = 50;
			// 
			// timeColumn
			// 
			this.timeColumn.Text = "Epoch Time (s)";
			this.timeColumn.Width = 84;
			// 
			// headersColumn
			// 
			this.headersColumn.Text = "Headers";
			this.headersColumn.Width = 220;
			// 
			// typeColumn
			// 
			this.typeColumn.Text = "Type";
			this.typeColumn.Width = 309;
			// 
			// sizeColumn
			// 
			this.sizeColumn.Text = "Size";
			// 
			// extraInfoColumn
			// 
			this.extraInfoColumn.Text = "Extra Info";
			this.extraInfoColumn.Width = 66;
			// 
			// hexOpcodeColumn
			// 
			this.hexOpcodeColumn.Text = "OpCode";
			this.hexOpcodeColumn.Width = 66;
			// 
			// packSeqColumn
			// 
			this.packSeqColumn.Text = "Pack. Seq";
			this.packSeqColumn.Width = 72;
			// 
			// queueColumn
			// 
			this.queueColumn.Text = "Queue";
			// 
			// iterationColumn
			// 
			this.iterationColumn.Text = "Iteration";
			// 
			// serverPortColumn
			// 
			this.serverPortColumn.Text = "Server Port";
			this.serverPortColumn.Width = 65;
			// 
			// listView_CreatedObjects
			// 
			this.listView_CreatedObjects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12});
			this.listView_CreatedObjects.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView_CreatedObjects.FullRowSelect = true;
			this.listView_CreatedObjects.HideSelection = false;
			this.listView_CreatedObjects.Location = new System.Drawing.Point(0, 0);
			this.listView_CreatedObjects.MultiSelect = false;
			this.listView_CreatedObjects.Name = "listView_CreatedObjects";
			this.listView_CreatedObjects.Size = new System.Drawing.Size(92, 96);
			this.listView_CreatedObjects.TabIndex = 0;
			this.listView_CreatedObjects.UseCompatibleStateImageBehavior = false;
			this.listView_CreatedObjects.View = System.Windows.Forms.View.Details;
			this.listView_CreatedObjects.VirtualMode = true;
			this.listView_CreatedObjects.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_CreatedObjects_ColumnClick);
			this.listView_CreatedObjects.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listView_CreatedObjects_RetrieveVirtualItem);
			this.listView_CreatedObjects.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_CreatedObjects_MouseClick);
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "#";
			this.columnHeader8.Width = 50;
			// 
			// columnHeader9
			// 
			this.columnHeader9.Text = "Object ID";
			this.columnHeader9.Width = 80;
			// 
			// columnHeader10
			// 
			this.columnHeader10.Text = "Name";
			this.columnHeader10.Width = 184;
			// 
			// columnHeader11
			// 
			this.columnHeader11.Text = "WCID";
			this.columnHeader11.Width = 56;
			// 
			// columnHeader12
			// 
			this.columnHeader12.Text = "Type";
			this.columnHeader12.Width = 186;
			// 
			// splitContainer_Bottom
			// 
			this.splitContainer_Bottom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer_Bottom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer_Bottom.Location = new System.Drawing.Point(0, 0);
			this.splitContainer_Bottom.Name = "splitContainer_Bottom";
			// 
			// splitContainer_Bottom.Panel1
			// 
			this.splitContainer_Bottom.Panel1.Controls.Add(this.tabControl1);
			// 
			// splitContainer_Bottom.Panel2
			// 
			this.splitContainer_Bottom.Panel2.Controls.Add(this.treeView_ParsedData);
			this.splitContainer_Bottom.Size = new System.Drawing.Size(1520, 414);
			this.splitContainer_Bottom.SplitterDistance = 1074;
			this.splitContainer_Bottom.TabIndex = 0;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabHexView);
			this.tabControl1.Controls.Add(this.tabProtocolDocs);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1070, 410);
			this.tabControl1.TabIndex = 0;
			this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
			// 
			// tabHexView
			// 
			this.tabHexView.Controls.Add(this.hexBox1);
			this.tabHexView.Location = new System.Drawing.Point(4, 22);
			this.tabHexView.Name = "tabHexView";
			this.tabHexView.Padding = new System.Windows.Forms.Padding(3);
			this.tabHexView.Size = new System.Drawing.Size(1062, 384);
			this.tabHexView.TabIndex = 0;
			this.tabHexView.Text = "Hex View";
			this.tabHexView.UseVisualStyleBackColor = true;
			// 
			// hexBox1
			// 
			this.hexBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.hexBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			// 
			// 
			// 
			this.hexBox1.BuiltInContextMenu.CopyMenuItemText = "Copy";
			this.hexBox1.BuiltInContextMenu.SelectAllMenuItemText = "Select All";
			this.hexBox1.ContextMenuStrip = this.hexContextMenu;
			this.hexBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.hexBox1.GroupSize = 8;
			this.hexBox1.LineInfoVisible = true;
			this.hexBox1.Location = new System.Drawing.Point(3, 3);
			this.hexBox1.Name = "hexBox1";
			this.hexBox1.ReadOnly = true;
			this.hexBox1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			this.hexBox1.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
			this.hexBox1.Size = new System.Drawing.Size(1056, 378);
			this.hexBox1.StringViewVisible = true;
			this.hexBox1.TabIndex = 1;
			this.hexBox1.UseFixedBytesPerLine = true;
			this.hexBox1.VScrollBarVisible = true;
			// 
			// hexContextMenu
			// 
			this.hexContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyTextMenuItem,
            this.copyHexMenuItem});
			this.hexContextMenu.Name = "hexContextMenu";
			this.hexContextMenu.Size = new System.Drawing.Size(215, 48);
			this.hexContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.hexContextMenu_Opening);
			this.hexContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.hexContextMenu_ItemClicked);
			// 
			// copyTextMenuItem
			// 
			this.copyTextMenuItem.Name = "copyTextMenuItem";
			this.copyTextMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyTextMenuItem.Size = new System.Drawing.Size(214, 22);
			this.copyTextMenuItem.Text = "Copy as &Text";
			// 
			// copyHexMenuItem
			// 
			this.copyHexMenuItem.Name = "copyHexMenuItem";
			this.copyHexMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
			this.copyHexMenuItem.Size = new System.Drawing.Size(214, 22);
			this.copyHexMenuItem.Text = "Copy as &Hex";
			// 
			// tabProtocolDocs
			// 
			this.tabProtocolDocs.Controls.Add(this.protocolWebBrowser);
			this.tabProtocolDocs.Location = new System.Drawing.Point(4, 22);
			this.tabProtocolDocs.Name = "tabProtocolDocs";
			this.tabProtocolDocs.Size = new System.Drawing.Size(1059, 380);
			this.tabProtocolDocs.TabIndex = 1;
			this.tabProtocolDocs.Text = "Protocol Documentation";
			this.tabProtocolDocs.UseVisualStyleBackColor = true;
			// 
			// protocolWebBrowser
			// 
			this.protocolWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.protocolWebBrowser.Location = new System.Drawing.Point(0, 0);
			this.protocolWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.protocolWebBrowser.Name = "protocolWebBrowser";
			this.protocolWebBrowser.Size = new System.Drawing.Size(1059, 380);
			this.protocolWebBrowser.TabIndex = 0;
			// 
			// treeView_ParsedData
			// 
			this.treeView_ParsedData.ContextMenuStrip = this.parsedContextMenu;
			this.treeView_ParsedData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView_ParsedData.Location = new System.Drawing.Point(0, 0);
			this.treeView_ParsedData.Name = "treeView_ParsedData";
			this.treeView_ParsedData.Size = new System.Drawing.Size(438, 410);
			this.treeView_ParsedData.TabIndex = 0;
			this.treeView_ParsedData.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_ParsedData_AfterSelect);
			this.treeView_ParsedData.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_ParsedData_NodeMouseClick);
			// 
			// parsedContextMenu
			// 
			this.parsedContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExpandAll,
            this.CollapseAll,
            this.toolStripSeparator1,
            this.CopyAll,
            this.TeleLoc,
            this.toolStripSeparator2,
            this.FindID});
			this.parsedContextMenu.Name = "parsedContextMenu";
			this.parsedContextMenu.Size = new System.Drawing.Size(184, 126);
			this.parsedContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.parsedContextMenu_Opening);
			this.parsedContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.parsedContextMenu_ItemClicked);
			// 
			// ExpandAll
			// 
			this.ExpandAll.Name = "ExpandAll";
			this.ExpandAll.Size = new System.Drawing.Size(183, 22);
			this.ExpandAll.Text = "&Expand All";
			// 
			// CollapseAll
			// 
			this.CollapseAll.Name = "CollapseAll";
			this.CollapseAll.Size = new System.Drawing.Size(183, 22);
			this.CollapseAll.Text = "C&ollapse All";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(180, 6);
			// 
			// CopyAll
			// 
			this.CopyAll.Name = "CopyAll";
			this.CopyAll.ShowShortcutKeys = false;
			this.CopyAll.Size = new System.Drawing.Size(183, 22);
			this.CopyAll.Text = "&Copy All";
			// 
			// TeleLoc
			// 
			this.TeleLoc.Name = "TeleLoc";
			this.TeleLoc.Size = new System.Drawing.Size(183, 22);
			this.TeleLoc.Text = "Copy ACE @teleloc";
			this.TeleLoc.Visible = false;
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(180, 6);
			// 
			// FindID
			// 
			this.FindID.Name = "FindID";
			this.FindID.Size = new System.Drawing.Size(183, 22);
			this.FindID.Text = "&Find ID In Object List";
			// 
			// listviewContextMenu
			// 
			this.listviewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyTimeMenuItem});
			this.listviewContextMenu.Name = "listviewContextMenu";
			this.listviewContextMenu.Size = new System.Drawing.Size(156, 26);
			this.listviewContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.listviewContextMenu_ItemClicked);
			// 
			// copyTimeMenuItem
			// 
			this.copyTimeMenuItem.Name = "copyTimeMenuItem";
			this.copyTimeMenuItem.Size = new System.Drawing.Size(155, 22);
			this.copyTimeMenuItem.Text = "Copy time field";
			// 
			// objectsContextMenu
			// 
			this.objectsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jumpToMessageMenuItem,
            this.highlightObjectIDMenuItem});
			this.objectsContextMenu.Name = "objectsContextMenu";
			this.objectsContextMenu.Size = new System.Drawing.Size(189, 48);
			this.objectsContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.objectsContextMenu_Opening);
			this.objectsContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.objectsContextMenu_ItemClicked);
			// 
			// jumpToMessageMenuItem
			// 
			this.jumpToMessageMenuItem.Name = "jumpToMessageMenuItem";
			this.jumpToMessageMenuItem.Size = new System.Drawing.Size(188, 22);
			this.jumpToMessageMenuItem.Text = "&Jump to this message";
			// 
			// highlightObjectIDMenuItem
			// 
			this.highlightObjectIDMenuItem.Name = "highlightObjectIDMenuItem";
			this.highlightObjectIDMenuItem.Size = new System.Drawing.Size(188, 22);
			this.highlightObjectIDMenuItem.Text = "&Highlight Object ID";
			// 
			// textBox_Search
			// 
			this.textBox_Search.Location = new System.Drawing.Point(617, 23);
			this.textBox_Search.MaxLength = 6;
			this.textBox_Search.Name = "textBox_Search";
			this.textBox_Search.Size = new System.Drawing.Size(165, 20);
			this.textBox_Search.TabIndex = 5;
			this.textBox_Search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_Search_KeyPress);
			// 
			// pictureBox_Search
			// 
			this.pictureBox_Search.Location = new System.Drawing.Point(893, 24);
			this.pictureBox_Search.Name = "pictureBox_Search";
			this.pictureBox_Search.Size = new System.Drawing.Size(20, 20);
			this.pictureBox_Search.TabIndex = 3;
			this.pictureBox_Search.TabStop = false;
			this.pictureBox_Search.Visible = false;
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatus});
			this.statusStrip.Location = new System.Drawing.Point(0, 704);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(1520, 22);
			this.statusStrip.TabIndex = 1;
			this.statusStrip.Text = "statusStrip1";
			// 
			// toolStripStatus
			// 
			this.toolStripStatus.Name = "toolStripStatus";
			this.toolStripStatus.Size = new System.Drawing.Size(74, 17);
			this.toolStripStatus.Text = "AC Log View";
			// 
			// checkBox_HideHeaderOnly
			// 
			this.checkBox_HideHeaderOnly.Location = new System.Drawing.Point(919, 26);
			this.checkBox_HideHeaderOnly.Name = "checkBox_HideHeaderOnly";
			this.checkBox_HideHeaderOnly.Size = new System.Drawing.Size(154, 17);
			this.checkBox_HideHeaderOnly.TabIndex = 7;
			this.checkBox_HideHeaderOnly.Text = "Hide Header Only";
			this.checkBox_HideHeaderOnly.UseVisualStyleBackColor = true;
			this.checkBox_HideHeaderOnly.Visible = false;
			this.checkBox_HideHeaderOnly.CheckedChanged += new System.EventHandler(this.checkBox_HideHeaderOnly_CheckedChanged);
			// 
			// checkBox_useHighlighting
			// 
			this.checkBox_useHighlighting.Checked = true;
			this.checkBox_useHighlighting.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox_useHighlighting.Enabled = false;
			this.checkBox_useHighlighting.Location = new System.Drawing.Point(1206, 26);
			this.checkBox_useHighlighting.Name = "checkBox_useHighlighting";
			this.checkBox_useHighlighting.Size = new System.Drawing.Size(165, 17);
			this.checkBox_useHighlighting.TabIndex = 9;
			this.checkBox_useHighlighting.Text = "Use Highlighting (Slower!)";
			this.checkBox_useHighlighting.UseVisualStyleBackColor = true;
			this.checkBox_useHighlighting.CheckedChanged += new System.EventHandler(this.checkBox_useHighlighting_CheckedChanged);
			// 
			// checkBoxUseHex
			// 
			this.checkBoxUseHex.AutoSize = true;
			this.checkBoxUseHex.Checked = true;
			this.checkBoxUseHex.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxUseHex.Enabled = false;
			this.checkBoxUseHex.Location = new System.Drawing.Point(1079, 26);
			this.checkBoxUseHex.Name = "checkBoxUseHex";
			this.checkBoxUseHex.Size = new System.Drawing.Size(122, 17);
			this.checkBoxUseHex.TabIndex = 8;
			this.checkBoxUseHex.Text = "Display Data as Hex";
			this.checkBoxUseHex.UseVisualStyleBackColor = true;
			this.checkBoxUseHex.CheckedChanged += new System.EventHandler(this.checkBoxUseHex_CheckedChanged);
			// 
			// CmdLock
			// 
			this.CmdLock.Location = new System.Drawing.Point(2, 23);
			this.CmdLock.Name = "CmdLock";
			this.CmdLock.Size = new System.Drawing.Size(75, 23);
			this.CmdLock.TabIndex = 1;
			this.CmdLock.Text = "Lock";
			this.CmdLock.UseVisualStyleBackColor = true;
			this.CmdLock.Click += new System.EventHandler(this.CmdLock_Click);
			// 
			// cmdforward
			// 
			this.cmdforward.Location = new System.Drawing.Point(83, 23);
			this.cmdforward.Name = "cmdforward";
			this.cmdforward.Size = new System.Drawing.Size(75, 23);
			this.cmdforward.TabIndex = 2;
			this.cmdforward.Text = ">";
			this.cmdforward.UseVisualStyleBackColor = true;
			this.cmdforward.Click += new System.EventHandler(this.cmdforward_Click);
			// 
			// cmdbackward
			// 
			this.cmdbackward.Location = new System.Drawing.Point(164, 23);
			this.cmdbackward.Name = "cmdbackward";
			this.cmdbackward.Size = new System.Drawing.Size(75, 23);
			this.cmdbackward.TabIndex = 3;
			this.cmdbackward.Text = "<";
			this.cmdbackward.UseVisualStyleBackColor = true;
			this.cmdbackward.Click += new System.EventHandler(this.cmdbackward_Click);
			// 
			// lblTracker
			// 
			this.lblTracker.AutoSize = true;
			this.lblTracker.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTracker.Location = new System.Drawing.Point(245, 26);
			this.lblTracker.Name = "lblTracker";
			this.lblTracker.Size = new System.Drawing.Size(86, 17);
			this.lblTracker.TabIndex = 4;
			this.lblTracker.Text = "Viewing #0";
			// 
			// btnHighlight
			// 
			this.btnHighlight.Enabled = false;
			this.btnHighlight.Location = new System.Drawing.Point(788, 23);
			this.btnHighlight.Name = "btnHighlight";
			this.btnHighlight.Size = new System.Drawing.Size(75, 22);
			this.btnHighlight.TabIndex = 6;
			this.btnHighlight.Text = "Highlight";
			this.btnHighlight.UseVisualStyleBackColor = true;
			this.btnHighlight.Click += new System.EventHandler(this.btnHighlight_Click);
			// 
			// checkBox_ShowObjects
			// 
			this.checkBox_ShowObjects.AutoSize = true;
			this.checkBox_ShowObjects.Enabled = false;
			this.checkBox_ShowObjects.Location = new System.Drawing.Point(1377, 26);
			this.checkBox_ShowObjects.Name = "checkBox_ShowObjects";
			this.checkBox_ShowObjects.Size = new System.Drawing.Size(139, 17);
			this.checkBox_ShowObjects.TabIndex = 10;
			this.checkBox_ShowObjects.Text = "Display Created Objects";
			this.checkBox_ShowObjects.UseVisualStyleBackColor = true;
			this.checkBox_ShowObjects.CheckedChanged += new System.EventHandler(this.checkBox_ShowObjects_CheckedChanged);
			// 
			// HighlightMode_comboBox
			// 
			this.HighlightMode_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.HighlightMode_comboBox.Location = new System.Drawing.Point(473, 23);
			this.HighlightMode_comboBox.Name = "HighlightMode_comboBox";
			this.HighlightMode_comboBox.Size = new System.Drawing.Size(138, 21);
			this.HighlightMode_comboBox.TabIndex = 0;
			this.HighlightMode_comboBox.SelectedIndexChanged += new System.EventHandler(this.HighlightMode_comboBox_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(392, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 13);
			this.label1.TabIndex = 11;
			this.label1.Text = "Highlight Mode:";
			// 
			// columnsContextMenu
			// 
			this.columnsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendReceiveMenuItem,
            this.timeMenuItem,
            this.headersMenuItem,
            this.typeMenuItem,
            this.sizeMenuItem,
            this.extraInfoMenuItem,
            this.opcodeMenuItem,
            this.packSeqMenuItem,
            this.queueMenuItem,
            this.iterationMenuItem,
            this.serverPortMenuItem});
			this.columnsContextMenu.Name = "columnsContextMenu";
			this.columnsContextMenu.Size = new System.Drawing.Size(132, 246);
			this.columnsContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.columnsContextMenu_ItemClicked);
			// 
			// sendReceiveMenuItem
			// 
			this.sendReceiveMenuItem.Checked = true;
			this.sendReceiveMenuItem.CheckOnClick = true;
			this.sendReceiveMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.sendReceiveMenuItem.Name = "sendReceiveMenuItem";
			this.sendReceiveMenuItem.Size = new System.Drawing.Size(131, 22);
			this.sendReceiveMenuItem.Text = "S/R";
			// 
			// timeMenuItem
			// 
			this.timeMenuItem.Checked = true;
			this.timeMenuItem.CheckOnClick = true;
			this.timeMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.timeMenuItem.Name = "timeMenuItem";
			this.timeMenuItem.Size = new System.Drawing.Size(131, 22);
			this.timeMenuItem.Text = "Time";
			// 
			// headersMenuItem
			// 
			this.headersMenuItem.Checked = true;
			this.headersMenuItem.CheckOnClick = true;
			this.headersMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.headersMenuItem.Name = "headersMenuItem";
			this.headersMenuItem.Size = new System.Drawing.Size(131, 22);
			this.headersMenuItem.Text = "Headers";
			// 
			// typeMenuItem
			// 
			this.typeMenuItem.Checked = true;
			this.typeMenuItem.CheckOnClick = true;
			this.typeMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.typeMenuItem.Name = "typeMenuItem";
			this.typeMenuItem.Size = new System.Drawing.Size(131, 22);
			this.typeMenuItem.Text = "Type";
			// 
			// sizeMenuItem
			// 
			this.sizeMenuItem.Checked = true;
			this.sizeMenuItem.CheckOnClick = true;
			this.sizeMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.sizeMenuItem.Name = "sizeMenuItem";
			this.sizeMenuItem.Size = new System.Drawing.Size(131, 22);
			this.sizeMenuItem.Text = "Size";
			// 
			// extraInfoMenuItem
			// 
			this.extraInfoMenuItem.Checked = true;
			this.extraInfoMenuItem.CheckOnClick = true;
			this.extraInfoMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.extraInfoMenuItem.Name = "extraInfoMenuItem";
			this.extraInfoMenuItem.Size = new System.Drawing.Size(131, 22);
			this.extraInfoMenuItem.Text = "Extra Info";
			// 
			// opcodeMenuItem
			// 
			this.opcodeMenuItem.Checked = true;
			this.opcodeMenuItem.CheckOnClick = true;
			this.opcodeMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.opcodeMenuItem.Name = "opcodeMenuItem";
			this.opcodeMenuItem.Size = new System.Drawing.Size(131, 22);
			this.opcodeMenuItem.Text = "OpCode";
			// 
			// packSeqMenuItem
			// 
			this.packSeqMenuItem.Checked = true;
			this.packSeqMenuItem.CheckOnClick = true;
			this.packSeqMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.packSeqMenuItem.Name = "packSeqMenuItem";
			this.packSeqMenuItem.Size = new System.Drawing.Size(131, 22);
			this.packSeqMenuItem.Text = "Pack. Seq";
			// 
			// queueMenuItem
			// 
			this.queueMenuItem.Checked = true;
			this.queueMenuItem.CheckOnClick = true;
			this.queueMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.queueMenuItem.Name = "queueMenuItem";
			this.queueMenuItem.Size = new System.Drawing.Size(131, 22);
			this.queueMenuItem.Text = "Queue";
			// 
			// iterationMenuItem
			// 
			this.iterationMenuItem.Checked = true;
			this.iterationMenuItem.CheckOnClick = true;
			this.iterationMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.iterationMenuItem.Name = "iterationMenuItem";
			this.iterationMenuItem.Size = new System.Drawing.Size(131, 22);
			this.iterationMenuItem.Text = "Iteration";
			// 
			// serverPortMenuItem
			// 
			this.serverPortMenuItem.Checked = true;
			this.serverPortMenuItem.CheckOnClick = true;
			this.serverPortMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.serverPortMenuItem.Name = "serverPortMenuItem";
			this.serverPortMenuItem.Size = new System.Drawing.Size(131, 22);
			this.serverPortMenuItem.Text = "Server Port";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1520, 24);
			this.menuStrip1.TabIndex = 12;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// menuStrip2
			// 
			this.menuStrip2.Location = new System.Drawing.Point(0, 24);
			this.menuStrip2.Name = "menuStrip2";
			this.menuStrip2.Size = new System.Drawing.Size(1520, 24);
			this.menuStrip2.TabIndex = 13;
			this.menuStrip2.Text = "menuStrip2";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openAsMessagesToolStripMenuItem,
            this.openAsFragmentsToolStripMenuItem,
            this.reOpenAsMessagesToolStripMenuItem,
            this.reOpenAsFragmentsToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.previousHighlightedRowToolStripMenuItem,
            this.nextHighlightedRowToolStripMenuItem,
            this.toolStripSeparator3,
            this.goToLineToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findOpcodeInFilesToolStripMenuItem,
            this.findTextInFilesToolStripMenuItem,
            this.pcapScraperToolStripMenuItem,
            this.combatScraperToolStripMenuItem,
            this.toolStripSeparator4,
            this.findBadParsersToolStripMenuItem,
            this.fragDatListToolToolStripMenuItem,
            this.toolStripSeparator5,
            this.optionsToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
			this.toolsToolStripMenuItem.Text = "Tools";
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForUpdatesToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// openAsMessagesToolStripMenuItem
			// 
			this.openAsMessagesToolStripMenuItem.Name = "openAsMessagesToolStripMenuItem";
			this.openAsMessagesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openAsMessagesToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
			this.openAsMessagesToolStripMenuItem.Text = "Open As Messages";
			this.openAsMessagesToolStripMenuItem.Click += new System.EventHandler(this.openAsMessagesToolStripMenuItem_Click);
			// 
			// openAsFragmentsToolStripMenuItem
			// 
			this.openAsFragmentsToolStripMenuItem.Name = "openAsFragmentsToolStripMenuItem";
			this.openAsFragmentsToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
			this.openAsFragmentsToolStripMenuItem.Text = "Open As Fragments";
			this.openAsFragmentsToolStripMenuItem.Click += new System.EventHandler(this.openAsFragmentsToolStripMenuItem_Click);
			// 
			// reOpenAsMessagesToolStripMenuItem
			// 
			this.reOpenAsMessagesToolStripMenuItem.Enabled = false;
			this.reOpenAsMessagesToolStripMenuItem.Name = "reOpenAsMessagesToolStripMenuItem";
			this.reOpenAsMessagesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
			this.reOpenAsMessagesToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
			this.reOpenAsMessagesToolStripMenuItem.Text = "Re-Open As Messages";
			this.reOpenAsMessagesToolStripMenuItem.Click += new System.EventHandler(this.reOpenAsMessagesToolStripMenuItem_Click);
			// 
			// reOpenAsFragmentsToolStripMenuItem
			// 
			this.reOpenAsFragmentsToolStripMenuItem.Enabled = false;
			this.reOpenAsFragmentsToolStripMenuItem.Name = "reOpenAsFragmentsToolStripMenuItem";
			this.reOpenAsFragmentsToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
			this.reOpenAsFragmentsToolStripMenuItem.Text = "Re-Open As Fragments";
			this.reOpenAsFragmentsToolStripMenuItem.Click += new System.EventHandler(this.reOpenAsFragmentsToolStripMenuItem_Click);
			// 
			// previousHighlightedRowToolStripMenuItem
			// 
			this.previousHighlightedRowToolStripMenuItem.Name = "previousHighlightedRowToolStripMenuItem";
			this.previousHighlightedRowToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F3)));
			this.previousHighlightedRowToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
			this.previousHighlightedRowToolStripMenuItem.Text = "Previous Highlighted Row";
			this.previousHighlightedRowToolStripMenuItem.Click += new System.EventHandler(this.previousHighlightedRowToolStripMenuItem_Click);
			// 
			// nextHighlightedRowToolStripMenuItem
			// 
			this.nextHighlightedRowToolStripMenuItem.Name = "nextHighlightedRowToolStripMenuItem";
			this.nextHighlightedRowToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.nextHighlightedRowToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
			this.nextHighlightedRowToolStripMenuItem.Text = "Next Highlighted Row";
			this.nextHighlightedRowToolStripMenuItem.Click += new System.EventHandler(this.nextHighlightedRowToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(208, 6);
			// 
			// goToLineToolStripMenuItem
			// 
			this.goToLineToolStripMenuItem.Enabled = false;
			this.goToLineToolStripMenuItem.Name = "goToLineToolStripMenuItem";
			this.goToLineToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
			this.goToLineToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
			this.goToLineToolStripMenuItem.Text = "Go To Line";
			this.goToLineToolStripMenuItem.Click += new System.EventHandler(this.goToLineToolStripMenuItem_Click);
			// 
			// findOpcodeInFilesToolStripMenuItem
			// 
			this.findOpcodeInFilesToolStripMenuItem.Name = "findOpcodeInFilesToolStripMenuItem";
			this.findOpcodeInFilesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.findOpcodeInFilesToolStripMenuItem.Text = "Find Opcode In Files";
			this.findOpcodeInFilesToolStripMenuItem.Click += new System.EventHandler(this.findOpcodeInFilesToolStripMenuItem_Click);
			// 
			// findTextInFilesToolStripMenuItem
			// 
			this.findTextInFilesToolStripMenuItem.Name = "findTextInFilesToolStripMenuItem";
			this.findTextInFilesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.findTextInFilesToolStripMenuItem.Text = "Find Text In Files";
			this.findTextInFilesToolStripMenuItem.Click += new System.EventHandler(this.findTextInFilesToolStripMenuItem_Click);
			// 
			// pcapScraperToolStripMenuItem
			// 
			this.pcapScraperToolStripMenuItem.Name = "pcapScraperToolStripMenuItem";
			this.pcapScraperToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.pcapScraperToolStripMenuItem.Text = "Pcap Scraper";
			this.pcapScraperToolStripMenuItem.Click += new System.EventHandler(this.pcapScraperToolStripMenuItem_Click);
			// 
			// combatScraperToolStripMenuItem
			// 
			this.combatScraperToolStripMenuItem.Name = "combatScraperToolStripMenuItem";
			this.combatScraperToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.combatScraperToolStripMenuItem.Text = "Combat Scraper";
			this.combatScraperToolStripMenuItem.Click += new System.EventHandler(this.combatScraperToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(178, 6);
			// 
			// findBadParsersToolStripMenuItem
			// 
			this.findBadParsersToolStripMenuItem.Name = "findBadParsersToolStripMenuItem";
			this.findBadParsersToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.findBadParsersToolStripMenuItem.Text = "Find Bad Parsers";
			this.findBadParsersToolStripMenuItem.Click += new System.EventHandler(this.findBadParsersToolStripMenuItem_Click);
			// 
			// fragDatListToolToolStripMenuItem
			// 
			this.fragDatListToolToolStripMenuItem.Name = "fragDatListToolToolStripMenuItem";
			this.fragDatListToolToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.fragDatListToolToolStripMenuItem.Text = "Frag Dat List Tool";
			this.fragDatListToolToolStripMenuItem.Click += new System.EventHandler(this.fragDatListToolToolStripMenuItem_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(178, 6);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.optionsToolStripMenuItem.Text = "Options";
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
			// 
			// checkForUpdatesToolStripMenuItem
			// 
			this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
			this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.checkForUpdatesToolStripMenuItem.Text = "Check for updates...";
			this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1520, 726);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.HighlightMode_comboBox);
			this.Controls.Add(this.checkBox_ShowObjects);
			this.Controls.Add(this.btnHighlight);
			this.Controls.Add(this.checkBoxUseHex);
			this.Controls.Add(this.lblTracker);
			this.Controls.Add(this.cmdbackward);
			this.Controls.Add(this.cmdforward);
			this.Controls.Add(this.CmdLock);
			this.Controls.Add(this.checkBox_useHighlighting);
			this.Controls.Add(this.checkBox_HideHeaderOnly);
			this.Controls.Add(this.splitContainer_Main);
			this.Controls.Add(this.textBox_Search);
			this.Controls.Add(this.pictureBox_Search);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.menuStrip2);
			this.Controls.Add(this.menuStrip1);
			this.DoubleBuffered = true;
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "AC Log View";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.splitContainer_Main.Panel1.ResumeLayout(false);
			this.splitContainer_Main.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).EndInit();
			this.splitContainer_Main.ResumeLayout(false);
			this.splitContainer_Top.Panel1.ResumeLayout(false);
			this.splitContainer_Top.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer_Top)).EndInit();
			this.splitContainer_Top.ResumeLayout(false);
			this.splitContainer_Bottom.Panel1.ResumeLayout(false);
			this.splitContainer_Bottom.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer_Bottom)).EndInit();
			this.splitContainer_Bottom.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabHexView.ResumeLayout(false);
			this.hexContextMenu.ResumeLayout(false);
			this.tabProtocolDocs.ResumeLayout(false);
			this.parsedContextMenu.ResumeLayout(false);
			this.listviewContextMenu.ResumeLayout(false);
			this.objectsContextMenu.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox_Search)).EndInit();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.columnsContextMenu.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

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

