using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class PDFConverter
    {

        private const string GS_PATH = @"C:\Program Files\gs\gs9.22\bin\gswin64.exe";

        /// <summary>
        /// Convert the 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static async Task<int> ConvertAsync(string input, string ratio = "102.4")
        {

            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException("is null or empty", "input");
            if (string.IsNullOrWhiteSpace(ratio)) throw new ArgumentException("is null or empty", "ratio");

            if (!File.Exists(input)) throw new ArgumentException("File not found", "input");

            if (input.IndexOf("-parser-output") != -1 ||
                input.IndexOf("-parser-errors") != -1)
                return -1;

            var filename = new FileInfo(input).Name.Replace(".pdf", "");
            var outdir = new FileInfo(input).FullName.Replace(".pdf", "");
            Directory.CreateDirectory(outdir);

            var pageLayout = $"{filename}-pagelayout.txt";

            String args = $"-dNOPAUSE -sDEVICE=jpeg -r{ratio} -o\"{outdir}\\{filename}_%d.jpg\" \"{input}\"";
            Process proc = new Process();
            proc.StartInfo.FileName = GS_PATH;
            proc.StartInfo.Arguments = args;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
            proc.WaitForExit();

            return proc.ExitCode;
        }

        public static async Task<bool> ClusterFilesAsync(string input)
        {

            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException("is null or empty", "input");
            if (!File.Exists(input)) throw new ArgumentException("File not found!", "input");

            if (input.IndexOf("-parser-output") != -1 ||
                input.IndexOf("-parser-errors") != -1)
                return false;

            var fileInfo = new FileInfo(input);
            var filename = fileInfo.Name.Replace(".pdf", "");
            var inputRoot = fileInfo.DirectoryName;
            var imagesRoot = $"{inputRoot}\\{filename}";

            var pageLayoutFile = $"{inputRoot}\\{filename}-pagelayout.txt";

            if (!File.Exists(pageLayoutFile))
            {
                Console.WriteLine($"{pageLayoutFile} not found!");
                return false;
            }

            var line = string.Empty;


            using (var sr = new StreamReader(pageLayoutFile))
            {
                var page = 1;
                do
                {
                    line = await sr.ReadLineAsync();

                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var parts = line.Split(":");
                        if (page == int.Parse(parts[0]))
                        {
                            var layout = "null";
                            if (string.IsNullOrWhiteSpace(parts[1]))
                                Console.WriteLine($"PageLayout for page {page} is null!");
                            else
                                layout = parts[1];

                            var destDir = $"{imagesRoot}\\{layout}";
                            Directory.CreateDirectory(destDir);

                            var jpgFile = $"{filename}_{page}.jpg";
                            var source = $"{imagesRoot}\\{jpgFile}";
                            var dest = $"{destDir}\\{jpgFile}";

                            if (File.Exists(source))
                            {
                                File.Move(source, dest);
                            }
                            else
                            {
                                Console.WriteLine($"Something is wrong! I couldn't find the file {source}");
                            }
                        }
                        else
                        {
                            throw new Exception("Page numbers doesn't match!");
                        }
                        page++;
                    }

                } while (!string.IsNullOrWhiteSpace(line));
            }

            return true;

        }



    }
}
