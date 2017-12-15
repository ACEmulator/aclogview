using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aclogview {
    public partial class TextPopup : Form {
        public TextPopup() {
            InitializeComponent();
        }

        public void setText(String text) {
            textBox1.Text = text;
        }
    }
}
