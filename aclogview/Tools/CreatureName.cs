using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aclogview.Tools
{
    public partial class CreatureName : Form
    {
        public string creatureName { get; set; }
        public CreatureName()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (tbCreatureName.Text =="")
                MessageBox.Show("Creature Name is blank", "Warning!");
            creatureName = tbCreatureName.Text;
        }
    }
}
