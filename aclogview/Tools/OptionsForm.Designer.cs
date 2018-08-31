namespace aclogview
{
    partial class OptionsForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_ProtocolDays = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_ProtocolUpdates = new System.Windows.Forms.CheckBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.toolTip_ProtocolDoc = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.copyAllPadding = new System.Windows.Forms.TextBox();
            this.displayNodeTooltips = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbACEStyleHeaders = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_TimeFormat = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_ProtocolDays);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.checkBox_ProtocolUpdates);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(396, 58);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Updates";
            // 
            // textBox_ProtocolDays
            // 
            this.textBox_ProtocolDays.Location = new System.Drawing.Point(322, 17);
            this.textBox_ProtocolDays.MaxLength = 2;
            this.textBox_ProtocolDays.Name = "textBox_ProtocolDays";
            this.textBox_ProtocolDays.Size = new System.Drawing.Size(33, 20);
            this.textBox_ProtocolDays.TabIndex = 3;
            this.textBox_ProtocolDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_ProtocolDays_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(356, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Days";
            // 
            // checkBox_ProtocolUpdates
            // 
            this.checkBox_ProtocolUpdates.AutoSize = true;
            this.checkBox_ProtocolUpdates.Location = new System.Drawing.Point(6, 19);
            this.checkBox_ProtocolUpdates.Name = "checkBox_ProtocolUpdates";
            this.checkBox_ProtocolUpdates.Size = new System.Drawing.Size(320, 17);
            this.checkBox_ProtocolUpdates.TabIndex = 0;
            this.checkBox_ProtocolUpdates.Text = "Automatically check for protocol documentation updates every";
            this.checkBox_ProtocolUpdates.UseVisualStyleBackColor = true;
            this.checkBox_ProtocolUpdates.CheckedChanged += new System.EventHandler(this.checkBox_ProtocolUpdates_CheckedChanged);
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(252, 278);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 24);
            this.button_OK.TabIndex = 1;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(333, 278);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 24);
            this.button_Cancel.TabIndex = 2;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.displayNodeTooltips);
            this.groupBox2.Location = new System.Drawing.Point(12, 158);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(396, 89);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Treeview";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.copyAllPadding);
            this.groupBox4.Location = new System.Drawing.Point(208, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(179, 52);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Pad \"Copy All\" output";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Pad with";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(104, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Spaces";
            // 
            // copyAllPadding
            // 
            this.copyAllPadding.Location = new System.Drawing.Point(65, 22);
            this.copyAllPadding.Name = "copyAllPadding";
            this.copyAllPadding.Size = new System.Drawing.Size(33, 20);
            this.copyAllPadding.TabIndex = 1;
            // 
            // displayNodeTooltips
            // 
            this.displayNodeTooltips.AutoSize = true;
            this.displayNodeTooltips.Location = new System.Drawing.Point(6, 37);
            this.displayNodeTooltips.Name = "displayNodeTooltips";
            this.displayNodeTooltips.Size = new System.Drawing.Size(123, 17);
            this.displayNodeTooltips.TabIndex = 0;
            this.displayNodeTooltips.Text = "Display node tooltips";
            this.displayNodeTooltips.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbACEStyleHeaders);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.comboBox_TimeFormat);
            this.groupBox3.Location = new System.Drawing.Point(12, 76);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(396, 76);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Packets ListView";
            // 
            // cbACEStyleHeaders
            // 
            this.cbACEStyleHeaders.AutoSize = true;
            this.cbACEStyleHeaders.Location = new System.Drawing.Point(9, 46);
            this.cbACEStyleHeaders.Name = "cbACEStyleHeaders";
            this.cbACEStyleHeaders.Size = new System.Drawing.Size(359, 17);
            this.cbACEStyleHeaders.TabIndex = 1;
            this.cbACEStyleHeaders.Text = "ACE style packet header flags (requires reload of file or program restart)";
            this.cbACEStyleHeaders.UseVisualStyleBackColor = true;
            this.cbACEStyleHeaders.CheckedChanged += new System.EventHandler(this.cbACEStyleHeaders_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Time column display format:";
            // 
            // comboBox_TimeFormat
            // 
            this.comboBox_TimeFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_TimeFormat.FormattingEnabled = true;
            this.comboBox_TimeFormat.Items.AddRange(new object[] {
            "Epoch Time (seconds)",
            "Local Time"});
            this.comboBox_TimeFormat.Location = new System.Drawing.Point(149, 19);
            this.comboBox_TimeFormat.Name = "comboBox_TimeFormat";
            this.comboBox_TimeFormat.Size = new System.Drawing.Size(134, 21);
            this.comboBox_TimeFormat.TabIndex = 0;
            this.comboBox_TimeFormat.SelectedIndexChanged += new System.EventHandler(this.comboBox_TimeFormat_SelectedIndexChanged);
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(419, 318);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_ProtocolUpdates;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.ToolTip toolTip_ProtocolDoc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_ProtocolDays;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox displayNodeTooltips;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_TimeFormat;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox copyAllPadding;
        private System.Windows.Forms.CheckBox cbACEStyleHeaders;
    }
}
