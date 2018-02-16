using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class AnalyzeTextListener : IEventListener
    {
        public void EventOccurred(IEventData data, EventType type)
        {
            if(data is TextRenderInfo)
            {
                var textInfo = (TextRenderInfo)data;

                string text = textInfo.GetText();

                // Font
                var font = textInfo.GetFont();
                var fontProgram = font.GetFontProgram();
                var fontFamily = fontProgram.GetFontNames().GetFamilyName();
                double fontSize = textInfo.GetFontSize();

                // Grayscale, RGB, CMYK
                var color = textInfo.GetStrokeColor();
                var colorSpace = color.GetColorSpace();
                float[] colorComponents = color.GetColorValue();

                // Ascent, Descent, Baseline
                // https://stackoverflow.com/questions/27631736/meaning-of-top-ascent-baseline-descent-bottom-and-leading-in-androids-font
                var ascent = textInfo.GetAscentLine();
                var descent = textInfo.GetDescentLine();
                var baseline = textInfo.GetDescentLine();
                var start = baseline.GetStartPoint();
                var end = baseline.GetEndPoint();

                // Coordinates
                float x1 = start.Get(0);
                float y1 = start.Get(1);
                float x2 = end.Get(0);
                float y2 = end.Get(1);

                Console.WriteLine($"{font.ToString()} {fontSize}: ({x1},{y1})-({x2},{y2})");
            }
        }

        public ICollection<EventType> GetSupportedEvents()
        {
            return new[] { EventType.RENDER_TEXT };
        }
    }
}
