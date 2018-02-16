using System;
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

            string filename = "samples/p40.pdf";

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
            Console.WriteLine("ReadLinesFromListener");
            Console.WriteLine("============================================================");
            ReadLinesFromListener(filename);
            Console.WriteLine();

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

        static void ReadLinesFromListener(string filename)
        {
            using (var pdf = new PdfDocument(new PdfReader(filename)))
            {
                var page = pdf.GetFirstPage();

                var parser = new PdfCanvasProcessor(new UserPathListener());

                parser.ProcessPageContent(page);
            }
        }

    }
}
