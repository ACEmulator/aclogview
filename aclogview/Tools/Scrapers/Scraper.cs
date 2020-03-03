using System;
using System.Collections.Generic;
using System.IO;

namespace aclogview.Tools.Scrapers
{
    abstract class Scraper
    {
        public abstract string Description { get; }

        public virtual void Reset()
        {
        }

        /// <summary>
        /// This can be called by multiple thread simultaneously
        /// </summary>
        public abstract (int hits, int messageExceptions) ProcessFileRecords(string fileName, List<PacketRecord> records, ref bool searchAborted);

        public abstract void WriteOutput(string destinationRoot, ref bool writeOutputAborted);

        protected string GetFileName(string destinationRoot, string extension = ".txt")
        {
           return Path.Combine(destinationRoot, DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss") + " " + GetType().Name + extension);
        }
    }
}
