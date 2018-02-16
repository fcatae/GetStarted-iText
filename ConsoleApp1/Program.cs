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

            Console.WriteLine("ReadTextFromPdf");
            Console.WriteLine("============================================================");
            Console.WriteLine(simpleResult.Substring(0, 1000));
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

    }
}
