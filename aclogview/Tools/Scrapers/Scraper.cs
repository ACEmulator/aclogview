using System;
using System.Collections.Generic;
using System.IO;

namespace aclogview.Tools.Scrapers
{
    abstract class Scraper
    {
        public virtual void Reset()
        {
        }

        /// <summary>
        /// This can be called by multiple thread simultaneously
        /// </summary>
        public abstract void ProcessFileRecords(string fileName, List<PacketRecord> records, ref bool searchAborted);

        public abstract void WriteOutput(string destinationRoot);

        protected string GetFileName(string destinationRoot, string extension = ".txt")
        {
           return Path.Combine(destinationRoot, DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss") + " " + GetType().Name + extension);
        }
    }
}
