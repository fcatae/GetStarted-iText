using System;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Diagnostics;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string filename = "samples/p40.pdf";

            string ghostScriptPath = @"C:\Program Files\gs\gs9.22\bin\gswin64.exe";

            string ImageFileName = @"samples/p40";

            string simpleResult = ReadTextFromPdf(filename);

            Console.WriteLine("============================================================");
            Console.WriteLine("ReadTextFromPdf");
            Console.WriteLine("============================================================");
            Console.WriteLine(simpleResult.Substring(0, 1000));
            Console.WriteLine();

            Console.ReadKey();
            Console.WriteLine();

            Console.WriteLine("============================================================");
            Console.WriteLine("ReadTextFromListener");
            Console.WriteLine("============================================================");
            ReadTextFromListener(filename);
            Console.WriteLine();

            Console.ReadKey();
            Console.WriteLine();

            Console.WriteLine("============================================================");
            Console.WriteLine("ShowLinesFromListener");
            Console.WriteLine("============================================================");
            ShowLinesFromListener(filename);
            Console.WriteLine();

            Console.ReadKey();
            Console.WriteLine();

            Console.WriteLine("============================================================");
            Console.WriteLine("AnalyzeTextFromListener");
            Console.WriteLine("============================================================");
            AnalyzeTextFromListener(filename);
            Console.WriteLine();

            Console.ReadKey();
            Console.WriteLine();

            Console.WriteLine("============================================================");
            Console.WriteLine("PdfToJPG");
            Console.WriteLine("============================================================");
            PdfToJpg(ghostScriptPath, filename, ImageFileName);
            Console.WriteLine();

        }

         public static void PdfToJpg(string ghostScriptPath,string input, string output)
        {

            String ars = "-dNOPAUSE -sDEVICE=jpeg -r300 -o" + output + "-%d.jpg " + input;
            Process proc = new Process();
            proc.StartInfo.FileName = ghostScriptPath;
            proc.StartInfo.Arguments = ars;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
            proc.WaitForExit();

        }


        static string ReadTextFromPdf(string filename)
        {
            using (var pdf = new PdfDocument(new PdfReader(filename)))
            {
                var page = pdf.GetFirstPage();

                return PdfTextExtractor.GetTextFromPage(page);
            }
        }

        static void ReadTextFromListener(string filename)
        {
            using (var pdf = new PdfDocument(new PdfReader(filename)))
            {
                var page = pdf.GetFirstPage();

                var parser = new PdfCanvasProcessor(new UserTextListener());

                parser.ProcessPageContent(page);
            }
        }

        static void ShowLinesFromListener(string filename)
        {
            using (var pdf = new PdfDocument(new PdfReader(filename)))
            {
                var page = pdf.GetFirstPage();

                var parser = new PdfCanvasProcessor(new UserPathListener());

                parser.ProcessPageContent(page);
            }
        }

        static void AnalyzeTextFromListener(string filename)
        {
            using (var pdf = new PdfDocument(new PdfReader(filename)))
            {
                var page = pdf.GetFirstPage();

                var parser = new PdfCanvasProcessor(new AnalyzeTextListener());

                parser.ProcessPageContent(page);
            }
        }
    }
}
