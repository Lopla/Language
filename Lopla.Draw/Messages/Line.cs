using Lopla.Libs.Interfaces;

namespace Lopla.Draw.Messages
{
    public class Line : ILoplaMessage
    {
        public Point Start { get; set; }
        public Point End { get; set; }
    }

    public class Point
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
    }
}