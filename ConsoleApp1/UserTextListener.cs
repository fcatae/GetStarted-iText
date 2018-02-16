using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class UserTextListener : IEventListener
    {
        public void EventOccurred(IEventData data, EventType type)
        {
            if(data is TextRenderInfo)
            {
                var textInfo = (TextRenderInfo)data;

                string text = textInfo.GetText();

                Console.Write(text);
                Console.Write(" ");
            }
        }

        public ICollection<EventType> GetSupportedEvents()
        {
            return new[] { EventType.RENDER_TEXT };
        }
    }
}
