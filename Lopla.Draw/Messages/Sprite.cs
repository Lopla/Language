using Lopla.Libs.Interfaces;

namespace Lopla.Draw.Messages
{
    public class Sprite : ILoplaMessage
    {
        public string AssemblyName;
        public string ResourceName;
        public Point Position { get; set; }
        public Rect Rectangle;

    }

    public class Rect
    {
        public Point Position { get; set; }
        public Point Size { get; set; }
    }
}