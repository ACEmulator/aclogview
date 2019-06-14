using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace aclogview.Tools
{
    static class ToolUtil
    {
        public static List<string> GetPcapsInFolder()
        {
            using (FolderBrowserDialog openFolder = new FolderBrowserDialog())
            {
                if (openFolder.ShowDialog() != DialogResult.OK)
                    return null;

                return GetPcapsInFolder(openFolder.SelectedPath);
            }
        }

        public static List<string> GetPcapsInFolder(string root)
        {
            var files = new List<string>();

            files.AddRange(Directory.GetFiles(root, "*.pcap", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(root, "*.pcapng", SearchOption.AllDirectories));

            return files;
        }
    }
}
