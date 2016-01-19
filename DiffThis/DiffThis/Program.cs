using DiffThisUtils;
using DiffThisUtils.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffThis
{
    class Program
    {
        private static string targetFile;
        private static string sourceFile;
        private static string diffFile;

        public static void Main(string[] args)
        {
            bool showHelp = false;
            bool applyDiff = false;
            bool noDisplay = false;

            OptionSet options = new OptionSet(){
                { "t|target=", "Target file to diff", v => SetTargetFile(v)},
                { "s|source=", "Source file to diff", v => SetSourceFile(v)},
                { "d|diff=", "Diff file to place", v => SetDiffFile(v)},
                { "a|apply", "Apply diff file to source", v => applyDiff = (v != null)},
                { "n|nodisplay", "Do not display diff", v => noDisplay = (v != null)},
                { "h|?|help", "Show message and exit", v => showHelp = (v != null)}
            };

            List<string> extra;
            try
            {
                extra = options.Parse(args);

                if (showHelp)
                {
                    ShowHelp(options);
                    return;
                }

                DiffThisCore core = new DiffThisCore();
                if (applyDiff)
                {
                    ValidateFile(sourceFile, "source");
                    ValidateFile(diffFile, "diff");

                    core.ApplyDiff(sourceFile, diffFile, targetFile);
                }
                else
                {
                    ValidateFile(targetFile, "target");
                    string endDiff = null;

                    if(sourceFile == null)
                    {
                        endDiff = core.Diff(targetFile);
                    }
                    else
                    {
                        if(diffFile == null)
                        {
                            endDiff = core.Diff(sourceFile, targetFile);
                        }
                        else
                        {
                            endDiff = core.Diff(sourceFile, targetFile, diffFile);
                        }
                    }

                    Console.WriteLine("DiffFile: {0}", endDiff);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Try '-help' for more information.");
            }


        }

        private static void SetTargetFile(string file)
        {
            ValidateFile(file, "target");

            Program.targetFile = file;
        }

        private static void SetSourceFile(string file)
        {
            ValidateFile(file, "source");

            Program.sourceFile = file;
        }

        private static void SetDiffFile(string file)
        {
            ValidateFile(file, "diff");

            Program.diffFile = file;
        }

        private static void ValidateFile(string file, string paramName)
        {
            if (String.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentNullException(paramName, String.Format("{0} cannot be empty", paramName));
            }

            if (!File.Exists(file))
            {
                throw new FileNotFoundException(paramName, String.Format("{0} file was not found at: {1}", paramName, file));
            }
        }

        public static void ShowHelp(OptionSet options)
        {
            options.WriteOptionDescriptions(Console.Out);
        }
    }
}
