using Lopla.Libs.Interfaces;

namespace Lopla.Draw.Messages
{
    public class SetColor : ILoplaMessage
    {
        public Color Color;
    }

    public class Color
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
    }
}