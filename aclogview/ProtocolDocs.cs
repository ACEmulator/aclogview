using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using aclogview.Properties;
using System.Net.Http;

namespace aclogview
{
    public class ProtocolDocs
    {
        private static string _projectDirectory;
        private static string _documentationDirectory;
        private static string _localReleaseVersionFile = "releaseversion.txt";
        private static string _localReleaseVersionFilePath;
        private static string _localReleaseVersion;
        private static bool _isLocalReleaseValid;
        private static string _latestReleasePageUrl;
        private static string _latestReleaseFileUrl;
        private static string _latestReleaseFileName;
        private static string _latestReleaseVersion;
        public bool ShowUpToDateMessage { get; set; }

        public ProtocolDocs()
        {
            _projectDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\";
            _documentationDirectory = _projectDirectory + @"Protocol Documentation\";
            _localReleaseVersionFilePath = _documentationDirectory + @"\" + _localReleaseVersionFile;
            _latestReleasePageUrl = @"https://github.com/ACEmulator/acemulator.github.io/releases/latest";
        }

        public bool IsTimeForUpdateCheck()
        {
            return Settings.Default.ProtocolCheckForUpdates && IsTimeForUpdate();
        }

        private static bool IsTimeForUpdate()
        {
            var lastUpdateCheck = Settings.Default.ProtocolLastUpdateCheck;
            var updateInterval = Settings.Default.ProtocolUpdateIntervalDays;
            return lastUpdateCheck.AddDays(updateInterval) < DateTime.Now;
        }

        public async Task UpdateIfNeededAsync()
        {
            try
            {
                var updateIsAvailable = await IsUpdateAvailable();
                if (!updateIsAvailable)
                {
                    if (ShowUpToDateMessage)
                        DisplayYouHaveLatestDocs();
                    return;
                }
                if (DisplayConfirmUpdateDialog() == DialogResult.Yes)
                    await InstallDocumentation();
            }
            catch (Exception e)
            {
                DisplayUpdateError(e);
            }
        }

        private static void DisplayYouHaveLatestDocs()
        {
            MessageBox.Show($"You have the latest protocol documentation release ({_latestReleaseVersion}).",
                "AC Log View",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private static void DisplayUpdateError(Exception e)
        {
            MessageBox.Show(e.Message,
                "Protocol Documentation Update Error:",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private static async Task<bool> IsUpdateAvailable()
        {
            FillLocalReleaseInfo();
            await FillLatestReleaseInfo();
            Settings.Default.ProtocolLastUpdateCheck = DateTime.Now;
            if (!_isLocalReleaseValid) return true;
            return _localReleaseVersion != _latestReleaseVersion;
        }

        private static void FillLocalReleaseInfo()
        {
            _isLocalReleaseValid = IsLocalReleaseValid();
        }

        private static bool IsLocalReleaseValid()
        {
            if (!File.Exists(_localReleaseVersionFilePath)) return false;
            _localReleaseVersion = File.ReadAllText(_localReleaseVersionFilePath).Trim();
            return (!string.IsNullOrEmpty(_localReleaseVersion));
        }

        private static async Task FillLatestReleaseInfo()
        {
            var html = await GetWebDocumentAsString(_latestReleasePageUrl);
            var pattern = @"a href=\S(.+(Protocol_Documentation-(.+)\.zip))";
            var match = Regex.Match(html, pattern);
            if (!match.Success)
                throw new Exception("Could not retrieve the latest protocol\n" +
                    $"documentation release from: {_latestReleasePageUrl}");
            _latestReleaseFileUrl = "https://github.com" + match.Groups[1].Value;
            _latestReleaseFileName = match.Groups[2].Value;
            _latestReleaseVersion = match.Groups[3].Value;
        }

        private static async Task<string> GetWebDocumentAsString(string url)
        {
            string document;
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(url))
            using (var content = response.Content)
            {
                document = await content.ReadAsStringAsync();
            }
            return document;
        }

        private static async Task InstallDocumentation()
        {
            try
            {
                await Task.Run(() => DownloadDocs());
                await Task.Run(() => ExtractDocsAndCleanup());
                DisplayUpdateSuccessful();
            }
            catch (Exception e)
            {
                DisplayUpdateError(e);
            }
        }

        private static void DisplayUpdateSuccessful()
        {
            MessageBox.Show("Protocol documentation update completed successfully.",
                "AC Log View",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private static void DownloadDocs()
        {
            if (string.IsNullOrWhiteSpace(_latestReleaseFileUrl) 
                || string.IsNullOrWhiteSpace(_latestReleaseFileName))
                throw new Exception("Latest release information missing." +
                    " Fetching and parsing the latest release page must have failed.");
            using (var webClient = new WebClient())
            {
                var destinationFilePath = _projectDirectory + _latestReleaseFileName;
                webClient.DownloadFile(_latestReleaseFileUrl, destinationFilePath);
            }
        }

        private static void ExtractDocsAndCleanup()
        {
            VerifyExtractionVariables();
            var documentationZipfile = _projectDirectory + _latestReleaseFileName;
                
            using (var zipArchive = ZipFile.OpenRead(documentationZipfile))
            {
                if (!Directory.Exists(_documentationDirectory))
                    Directory.CreateDirectory(_documentationDirectory);
                ExtractZipAndOverwrite(zipArchive, _documentationDirectory);
            }

            File.Delete(_projectDirectory + _latestReleaseFileName);
        }

        private static void VerifyExtractionVariables()
        {
            if (string.IsNullOrWhiteSpace(_latestReleaseFileName))
                throw new Exception("Latest release information missing." +
                    " Fetching and parsing the latest release page must have failed.");
            var documentationZipfile = _projectDirectory + _latestReleaseFileName;
            if (!File.Exists(documentationZipfile))
                throw new Exception($"Latest release file {documentationZipfile} is missing." +
                    " Downloading the latest release must have failed.");
        }

        private static void ExtractZipAndOverwrite(ZipArchive archive, string destinationDirectory)
        {
            foreach (var entry in archive.Entries)
            {
                var fullPathToFile = destinationDirectory + entry.FullName;
                var pathToCheck = Path.GetDirectoryName(fullPathToFile);
                if (!Directory.Exists(pathToCheck))
                    Directory.CreateDirectory(pathToCheck ?? throw new InvalidOperationException());
                entry.ExtractToFile(fullPathToFile, overwrite: true);
            }
        }

        private static DialogResult DisplayConfirmUpdateDialog()
        {
            return _isLocalReleaseValid ? DisplayNewVersionAvailable() : DisplayDocsMissing();
        }

        private static DialogResult DisplayDocsMissing()
        {
            return MessageBox.Show("Protocol documentation files appear to be missing.\n" + 
                "Would you like to download and install them now?",
                "AC Log View",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);
        }

        private static DialogResult DisplayNewVersionAvailable()
        {
            return MessageBox.Show("A new version of protocol documentation is available.\n\n" +
                "Current version:   " + _localReleaseVersion + "\n" +
                "Latest version:      " + _latestReleaseVersion + "\n\n" +
                "Would you like to download and install the latest version?",
                "AC Log View",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);
        }

    }
}
