using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aclogview
{
    public partial class GotoLine : Form
    {
        public int lineNumber { get; set; }
        public int lineCount { get; set; }

        public GotoLine(int maxLines)
        {
            lineCount = maxLines;
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            var input = Line_textBox.Text;
            var trimmed = Regex.Replace(input, "[^0-9]", "");
            if (!String.IsNullOrEmpty(trimmed))
            {
                lineNumber = Convert.ToInt32(trimmed);
                if (lineNumber >= 0 && lineNumber <= lineCount - 1)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    toolStripStatusLabel.Text = $"Please enter a number between 0 and {lineCount - 1}.";
                }
            }
            else
            {
                toolStripStatusLabel.Text = $"Please enter a number between 0 and {lineCount - 1}.";
            }
        }
    }
}
