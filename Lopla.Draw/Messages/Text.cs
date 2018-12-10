using Lopla.Libs.Interfaces;

namespace Lopla.Draw.Messages
{
    public class Text : ILoplaMessage
    {
        public string Label { get; set; }

        public Point Position { get; set; }
    }
}