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
        private bool _parsedDataTreeviewDisplayTooltips;
        private byte _packetsListviewTimeFormat;
        private byte _treeviewCopyAllPadding;

        public OptionsForm()
        {
            InitializeComponent();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            LoadDefaultSettings();
            // Now update the form with our settings
            textBox_ProtocolDays.Text = _protocolUpdateIntervalDays.ToString();
            checkBox_ProtocolUpdates.Checked = _protocolCheckForUpdates;
            if (!_protocolCheckForUpdates)
                textBox_ProtocolDays.Enabled = false;
            displayNodeTooltips.Checked = _parsedDataTreeviewDisplayTooltips;
            comboBox_TimeFormat.SelectedIndex = _packetsListviewTimeFormat;
            copyAllPadding.Text = _treeviewCopyAllPadding.ToString();
        }

        private void LoadDefaultSettings()
        {
            Settings.Default.Reload();
            _protocolUpdateIntervalDays = Settings.Default.ProtocolUpdateIntervalDays;
            _protocolCheckForUpdates = Settings.Default.ProtocolCheckForUpdates;
            _parsedDataTreeviewDisplayTooltips = Settings.Default.ParsedDataTreeviewDisplayTooltips;
            _packetsListviewTimeFormat = Settings.Default.PacketsListviewTimeFormat;
            _treeviewCopyAllPadding = Settings.Default.TreeviewCopyAllPadding;
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
            result = FillTreeviewSettings();
            if (!result) return false;

            return true;
        }

        public void SaveNewSettings()
        {
            Settings.Default.ProtocolUpdateIntervalDays = _protocolUpdateIntervalDays;
            Settings.Default.ProtocolCheckForUpdates = _protocolCheckForUpdates;
            Settings.Default.ParsedDataTreeviewDisplayTooltips = _parsedDataTreeviewDisplayTooltips;
            Settings.Default.PacketsListviewTimeFormat = _packetsListviewTimeFormat;
            Settings.Default.TreeviewCopyAllPadding = _treeviewCopyAllPadding;
            Settings.Default.Save();
        }

        private bool FillTreeviewSettings()
        {
            _parsedDataTreeviewDisplayTooltips = displayNodeTooltips.Checked;
            var isValidInput = ValidateCopyAllPadding();
            return isValidInput && ParseCopyAllPadding();
        }

        private bool ValidateCopyAllPadding()
        {
            if (!string.IsNullOrWhiteSpace(copyAllPadding.Text)) return true;
            DisplayInvalidPaddingTooltip();
            return false;
        }

        private bool ParseCopyAllPadding()
        {
            var result = byte.TryParse(copyAllPadding.Text, out _treeviewCopyAllPadding);
            if (result) return true;
            DisplayByteConversionError("Pad \"Copy All\" output field");
            return false;
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
            DisplayByteConversionError("protocol documentation update interval");
            return false;
        }

        private void DisplayInvalidDaysTooltip()
        {
            toolTip_ProtocolDoc.ToolTipTitle = "Invalid Input";
            toolTip_ProtocolDoc.Show("You must enter a valid number of days for the update interval.",
                this,
                textBox_ProtocolDays.Location,
                5000);
        }

        private void DisplayInvalidPaddingTooltip()
        {
            toolTip_ProtocolDoc.ToolTipTitle = "Invalid Input";
            toolTip_ProtocolDoc.Show("You must enter a valid number padding spaces.",
                this,
                copyAllPadding.Location,
                5000);
        }

        private static void DisplayByteConversionError(string field)
        {
            MessageBox.Show($"The {field} must be a number between 0-255.",
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

        private void comboBox_TimeFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            _packetsListviewTimeFormat = (byte)comboBox_TimeFormat.SelectedIndex;
        }
    }
}
