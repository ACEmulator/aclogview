using aclogview.Properties;
using System;
using System.Security.Policy;
using System.Windows.Forms;

namespace aclogview
{
    public partial class OptionsForm : Form
    {
        private bool _protocolCheckForUpdates;
        private byte _protocolUpdateIntervalDays;

        public OptionsForm()
        {
            InitializeComponent();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            LoadDefaultSettings();
            textBox_ProtocolDays.Text = _protocolUpdateIntervalDays.ToString();
            checkBox_ProtocolUpdates.Checked = _protocolCheckForUpdates;
            if (!_protocolCheckForUpdates)
                textBox_ProtocolDays.Enabled = false;
        }

        private void LoadDefaultSettings()
        {
            Settings.Default.Reload();
            _protocolUpdateIntervalDays = Settings.Default.ProtocolUpdateIntervalDays;
            _protocolCheckForUpdates = Settings.Default.ProtocolCheckForUpdates;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (!FillUserSettings()) return;
            SaveNewSettings();
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool FillUserSettings()
        {
            var result = FillProtocolSettings();
            if (!result) return false;
            // Add new settings here

            return true;
        }

        public void SaveNewSettings()
        {
            Settings.Default.ProtocolUpdateIntervalDays = _protocolUpdateIntervalDays;
            Settings.Default.ProtocolCheckForUpdates = _protocolCheckForUpdates;
            Settings.Default.Save();
        }

        private bool FillProtocolSettings()
        {
            _protocolCheckForUpdates = checkBox_ProtocolUpdates.Checked;
            if (!_protocolCheckForUpdates) return true;
            var isValidInput = ValidateProtocolDaysInput();
            return isValidInput && ParseProtocolDays();
        }

        private bool ValidateProtocolDaysInput()
        {
            if (!string.IsNullOrWhiteSpace(textBox_ProtocolDays.Text)) return true;
            DisplayInvalidDaysTooltip();
            return false;
        }

        private bool ParseProtocolDays()
        {
            var result = byte.TryParse(textBox_ProtocolDays.Text, out _protocolUpdateIntervalDays);
            if (result) return true;
            DisplayByteConversionError(textBox_ProtocolDays.Text);
            return false;
        }

        private void DisplayInvalidDaysTooltip ()
        {
            toolTip_ProtocolDoc.ToolTipTitle = "Invalid Input";
            toolTip_ProtocolDoc.Show("You must enter a valid number of days for the update interval.",
                this,
                textBox_ProtocolDays.Location,
                5000);
        }

        private static void DisplayByteConversionError (string updateDays)
        {
            MessageBox.Show("Could not convert the protocol documentation " +
                        $"update interval string '{updateDays}' to a byte.",
                        "Error:",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void checkBox_ProtocolUpdates_CheckedChanged(object sender, EventArgs e)
        {
            textBox_ProtocolDays.Enabled = checkBox_ProtocolUpdates.Checked;
        }

        private void textBox_ProtocolDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
