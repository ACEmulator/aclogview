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
            this.displayNodeTooltips = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.button_OK.Location = new System.Drawing.Point(252, 145);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 1;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(333, 145);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 2;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.displayNodeTooltips);
            this.groupBox2.Location = new System.Drawing.Point(12, 76);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(396, 54);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Treeview";
            // 
            // displayNodeTooltips
            // 
            this.displayNodeTooltips.AutoSize = true;
            this.displayNodeTooltips.Location = new System.Drawing.Point(6, 19);
            this.displayNodeTooltips.Name = "displayNodeTooltips";
            this.displayNodeTooltips.Size = new System.Drawing.Size(123, 17);
            this.displayNodeTooltips.TabIndex = 0;
            this.displayNodeTooltips.Text = "Display node tooltips";
            this.displayNodeTooltips.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(420, 180);
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
    }
}