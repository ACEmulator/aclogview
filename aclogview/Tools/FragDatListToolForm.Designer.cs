namespace aclogview
{
    partial class FragDatListToolForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
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
            this.components = new System.ComponentModel.Container();
            this.groupBoxFragDatListFileBuilder = new System.Windows.Forms.GroupBox();
            this.btnStopBuild = new System.Windows.Forms.Button();
            this.btnChangeSearchPathRoot = new System.Windows.Forms.Button();
            this.btnStartBuild = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearchPathRoot = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxProcessFragDatListFile = new System.Windows.Forms.GroupBox();
            this.btnStopProcess = new System.Windows.Forms.Button();
            this.btnChangeFileToProcess = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFileToProcess = new System.Windows.Forms.TextBox();
            this.btnStartProcess = new System.Windows.Forms.Button();
            this.btnChangeOutputFolder = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.chkCompressOutput = new System.Windows.Forms.CheckBox();
            this.chkIncludeFullPathAndFileName = new System.Windows.Forms.CheckBox();
            this.groupBoxGeneralSettings = new System.Windows.Forms.GroupBox();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBoxFragDatListFileBuilder.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBoxProcessFragDatListFile.SuspendLayout();
            this.groupBoxGeneralSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxFragDatListFileBuilder
            // 
            this.groupBoxFragDatListFileBuilder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFragDatListFileBuilder.Controls.Add(this.chkIncludeFullPathAndFileName);
            this.groupBoxFragDatListFileBuilder.Controls.Add(this.chkCompressOutput);
            this.groupBoxFragDatListFileBuilder.Controls.Add(this.btnStopBuild);
            this.groupBoxFragDatListFileBuilder.Controls.Add(this.btnChangeSearchPathRoot);
            this.groupBoxFragDatListFileBuilder.Controls.Add(this.btnStartBuild);
            this.groupBoxFragDatListFileBuilder.Controls.Add(this.label1);
            this.groupBoxFragDatListFileBuilder.Controls.Add(this.txtSearchPathRoot);
            this.groupBoxFragDatListFileBuilder.Location = new System.Drawing.Point(12, 103);
            this.groupBoxFragDatListFileBuilder.Name = "groupBoxFragDatListFileBuilder";
            this.groupBoxFragDatListFileBuilder.Size = new System.Drawing.Size(760, 77);
            this.groupBoxFragDatListFileBuilder.TabIndex = 0;
            this.groupBoxFragDatListFileBuilder.TabStop = false;
            this.groupBoxFragDatListFileBuilder.Text = "Frag Dat List File Builder";
            // 
            // btnStopBuild
            // 
            this.btnStopBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStopBuild.Enabled = false;
            this.btnStopBuild.Location = new System.Drawing.Point(633, 48);
            this.btnStopBuild.Name = "btnStopBuild";
            this.btnStopBuild.Size = new System.Drawing.Size(121, 23);
            this.btnStopBuild.TabIndex = 9;
            this.btnStopBuild.Text = "Stop Build";
            this.btnStopBuild.UseVisualStyleBackColor = true;
            this.btnStopBuild.Click += new System.EventHandler(this.btnStopBuild_Click);
            // 
            // btnChangeSearchPathRoot
            // 
            this.btnChangeSearchPathRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeSearchPathRoot.Location = new System.Drawing.Point(726, 19);
            this.btnChangeSearchPathRoot.Name = "btnChangeSearchPathRoot";
            this.btnChangeSearchPathRoot.Size = new System.Drawing.Size(28, 23);
            this.btnChangeSearchPathRoot.TabIndex = 8;
            this.btnChangeSearchPathRoot.Text = "F";
            this.btnChangeSearchPathRoot.UseVisualStyleBackColor = true;
            this.btnChangeSearchPathRoot.Click += new System.EventHandler(this.btnChangeSearchPathRoot_Click);
            // 
            // btnStartBuild
            // 
            this.btnStartBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartBuild.Location = new System.Drawing.Point(506, 48);
            this.btnStartBuild.Name = "btnStartBuild";
            this.btnStartBuild.Size = new System.Drawing.Size(121, 23);
            this.btnStartBuild.TabIndex = 7;
            this.btnStartBuild.Text = "Start Build";
            this.btnStartBuild.UseVisualStyleBackColor = true;
            this.btnStartBuild.Click += new System.EventHandler(this.btnStartBuild_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Search Path Root";
            // 
            // txtSearchPathRoot
            // 
            this.txtSearchPathRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchPathRoot.Location = new System.Drawing.Point(104, 22);
            this.txtSearchPathRoot.Name = "txtSearchPathRoot";
            this.txtSearchPathRoot.Size = new System.Drawing.Size(616, 20);
            this.txtSearchPathRoot.TabIndex = 5;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4});
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(89, 17);
            this.toolStripStatusLabel1.Text = "Files Processed:";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Margin = new System.Windows.Forms.Padding(20, 3, 0, 2);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(122, 17);
            this.toolStripStatusLabel2.Text = "Fragments Processed:";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Margin = new System.Windows.Forms.Padding(20, 3, 0, 2);
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(60, 17);
            this.toolStripStatusLabel3.Text = "Total Hits:";
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(760, 36);
            this.label2.TabIndex = 10;
            this.label2.Text = "This tool is designed to run within Visual Studio. There are comments describing " +
    "how you can adjust this tool to perform custom data mining tasks that you might " +
    "need.";
            // 
            // groupBoxProcessFragDatListFile
            // 
            this.groupBoxProcessFragDatListFile.Controls.Add(this.btnStopProcess);
            this.groupBoxProcessFragDatListFile.Controls.Add(this.btnChangeFileToProcess);
            this.groupBoxProcessFragDatListFile.Controls.Add(this.label3);
            this.groupBoxProcessFragDatListFile.Controls.Add(this.txtFileToProcess);
            this.groupBoxProcessFragDatListFile.Controls.Add(this.btnStartProcess);
            this.groupBoxProcessFragDatListFile.Location = new System.Drawing.Point(12, 186);
            this.groupBoxProcessFragDatListFile.Name = "groupBoxProcessFragDatListFile";
            this.groupBoxProcessFragDatListFile.Size = new System.Drawing.Size(760, 339);
            this.groupBoxProcessFragDatListFile.TabIndex = 11;
            this.groupBoxProcessFragDatListFile.TabStop = false;
            this.groupBoxProcessFragDatListFile.Text = "Process Frag Dat List File";
            // 
            // btnStopProcess
            // 
            this.btnStopProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStopProcess.Enabled = false;
            this.btnStopProcess.Location = new System.Drawing.Point(633, 48);
            this.btnStopProcess.Name = "btnStopProcess";
            this.btnStopProcess.Size = new System.Drawing.Size(121, 23);
            this.btnStopProcess.TabIndex = 12;
            this.btnStopProcess.Text = "Stop Process";
            this.btnStopProcess.UseVisualStyleBackColor = true;
            this.btnStopProcess.Click += new System.EventHandler(this.btnStopProcess_Click);
            // 
            // btnChangeFileToProcess
            // 
            this.btnChangeFileToProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeFileToProcess.Location = new System.Drawing.Point(726, 19);
            this.btnChangeFileToProcess.Name = "btnChangeFileToProcess";
            this.btnChangeFileToProcess.Size = new System.Drawing.Size(28, 23);
            this.btnChangeFileToProcess.TabIndex = 11;
            this.btnChangeFileToProcess.Text = "F";
            this.btnChangeFileToProcess.UseVisualStyleBackColor = true;
            this.btnChangeFileToProcess.Click += new System.EventHandler(this.btnChangeFileToProcess_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "File To Process";
            // 
            // txtFileToProcess
            // 
            this.txtFileToProcess.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileToProcess.Location = new System.Drawing.Point(104, 21);
            this.txtFileToProcess.Name = "txtFileToProcess";
            this.txtFileToProcess.Size = new System.Drawing.Size(616, 20);
            this.txtFileToProcess.TabIndex = 9;
            // 
            // btnStartProcess
            // 
            this.btnStartProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartProcess.Location = new System.Drawing.Point(506, 47);
            this.btnStartProcess.Name = "btnStartProcess";
            this.btnStartProcess.Size = new System.Drawing.Size(121, 23);
            this.btnStartProcess.TabIndex = 8;
            this.btnStartProcess.Text = "Start Process";
            this.btnStartProcess.UseVisualStyleBackColor = true;
            this.btnStartProcess.Click += new System.EventHandler(this.btnStartProcess_Click);
            // 
            // btnChangeOutputFolder
            // 
            this.btnChangeOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeOutputFolder.Location = new System.Drawing.Point(726, 19);
            this.btnChangeOutputFolder.Name = "btnChangeOutputFolder";
            this.btnChangeOutputFolder.Size = new System.Drawing.Size(28, 23);
            this.btnChangeOutputFolder.TabIndex = 12;
            this.btnChangeOutputFolder.Text = "F";
            this.btnChangeOutputFolder.UseVisualStyleBackColor = true;
            this.btnChangeOutputFolder.Click += new System.EventHandler(this.btnChangeOutputFolder_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Output Folder";
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFolder.Location = new System.Drawing.Point(104, 22);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(616, 20);
            this.txtOutputFolder.TabIndex = 10;
            // 
            // chkCompressOutput
            // 
            this.chkCompressOutput.AutoSize = true;
            this.chkCompressOutput.Checked = true;
            this.chkCompressOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCompressOutput.Location = new System.Drawing.Point(104, 48);
            this.chkCompressOutput.Name = "chkCompressOutput";
            this.chkCompressOutput.Size = new System.Drawing.Size(107, 17);
            this.chkCompressOutput.TabIndex = 13;
            this.chkCompressOutput.Text = "Compress Output";
            this.chkCompressOutput.UseVisualStyleBackColor = true;
            // 
            // chkIncludeFullPathAndFileName
            // 
            this.chkIncludeFullPathAndFileName.AutoSize = true;
            this.chkIncludeFullPathAndFileName.Location = new System.Drawing.Point(217, 48);
            this.chkIncludeFullPathAndFileName.Name = "chkIncludeFullPathAndFileName";
            this.chkIncludeFullPathAndFileName.Size = new System.Drawing.Size(158, 17);
            this.chkIncludeFullPathAndFileName.TabIndex = 14;
            this.chkIncludeFullPathAndFileName.Text = "Include Full Path+File Name";
            this.chkIncludeFullPathAndFileName.UseVisualStyleBackColor = true;
            // 
            // groupBoxGeneralSettings
            // 
            this.groupBoxGeneralSettings.Controls.Add(this.btnChangeOutputFolder);
            this.groupBoxGeneralSettings.Controls.Add(this.txtOutputFolder);
            this.groupBoxGeneralSettings.Controls.Add(this.label4);
            this.groupBoxGeneralSettings.Location = new System.Drawing.Point(12, 48);
            this.groupBoxGeneralSettings.Name = "groupBoxGeneralSettings";
            this.groupBoxGeneralSettings.Size = new System.Drawing.Size(760, 49);
            this.groupBoxGeneralSettings.TabIndex = 12;
            this.groupBoxGeneralSettings.TabStop = false;
            this.groupBoxGeneralSettings.Text = "General Settings";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Margin = new System.Windows.Forms.Padding(20, 3, 0, 2);
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(92, 17);
            this.toolStripStatusLabel4.Text = "Frag Exceptions:";
            // 
            // FragDatListToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.groupBoxGeneralSettings);
            this.Controls.Add(this.groupBoxProcessFragDatListFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBoxFragDatListFileBuilder);
            this.Name = "FragDatListToolForm";
            this.Text = "Frag Dat List Tool";
            this.groupBoxFragDatListFileBuilder.ResumeLayout(false);
            this.groupBoxFragDatListFileBuilder.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBoxProcessFragDatListFile.ResumeLayout(false);
            this.groupBoxProcessFragDatListFile.PerformLayout();
            this.groupBoxGeneralSettings.ResumeLayout(false);
            this.groupBoxGeneralSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxFragDatListFileBuilder;
        private System.Windows.Forms.Button btnChangeSearchPathRoot;
        private System.Windows.Forms.Button btnStartBuild;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearchPathRoot;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.Button btnStopBuild;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBoxProcessFragDatListFile;
        private System.Windows.Forms.Button btnStopProcess;
        private System.Windows.Forms.Button btnChangeFileToProcess;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFileToProcess;
        private System.Windows.Forms.Button btnStartProcess;
        private System.Windows.Forms.Button btnChangeOutputFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.CheckBox chkCompressOutput;
        private System.Windows.Forms.CheckBox chkIncludeFullPathAndFileName;
        private System.Windows.Forms.GroupBox groupBoxGeneralSettings;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
    }
}