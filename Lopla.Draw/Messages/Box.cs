using Lopla.Libs.Interfaces;

namespace Lopla.Draw.Messages
{
    public class Box : ILoplaMessage
    {
        public Point Start { get; set; }
        public Point End { get; set; }
    }
}