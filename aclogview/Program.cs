using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aclogview {

    public class Globals
    {
        private static bool _useHex;       

        public static bool UseHex
        {
            get
            {
                // Reads are usually simple
                return _useHex;
            }
            set
            {
                // You can add logic here for race conditions,
                // or other measurements
                _useHex = value;
            }
        }
    }

    public class Options
    {
        [Option('f', "file",
          HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }

        [Option('o', "opcode",
          HelpText = "The message opcode to search for.")]
        public int Opcode { get; set; }

        [Option('m', "asmessages", DefaultValue = true,
          HelpText = "Process the file in 'as messages' mode.")]
        public bool AsMessages { get; set; }

        [Option("cst",
          HelpText = "A case-sensitive text search.")]
        public string CSTextToSearch { get; set; }

        [Option("cit",
          HelpText = "A case-insensitive text search.")]
        public string CITextToSearch { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }

    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(args));
        }
    }
}
