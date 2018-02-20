using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class UserPathListener : IEventListener
    {
        public void EventOccurred(IEventData data, EventType type)
        {
            if (data is PathRenderInfo)
            {
                var path = (PathRenderInfo)data;

                var subpaths = path.GetPath().GetSubpaths();
                
                foreach(var subpath in subpaths)
                {
                    var shape = subpath.GetSegments();

                    foreach(var line in shape)
                    {
                        var points = line.GetBasePoints();

                        foreach(var p in points)
                        {
                            double x = p.x;
                            double y = p.y;

                            Console.Write(" - ");
                            Console.Write($"({x.ToString("0.00")},{y.ToString("0.00")})");
                        }
                    }
                }

                Console.WriteLine();
            }

        }

        public ICollection<EventType> GetSupportedEvents()
        {
            return new[] { EventType.RENDER_PATH };
        }
    }
}
