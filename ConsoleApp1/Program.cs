using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var files = Directory.GetFiles("output-d14","*.pdf");
            //string filename = "samples/p42.pdf";

            foreach (var file in files)
            {
                string filename = file;

                Console.WriteLine("============================================================");
                Console.WriteLine($"Generating Image from PDF - {file}");
                Console.WriteLine("============================================================");
                Console.WriteLine(PDFConverter.ConvertAsync(filename, "50x50").GetAwaiter().GetResult());
                PDFConverter.ClusterFilesAsync(filename).GetAwaiter().GetResult();
                Console.WriteLine();
            }


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
