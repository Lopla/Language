using Lopla.Libs.Interfaces;

namespace Lopla.Draw.Messages
{
    public class Animation : ILoplaMessage
    {
        public byte[] BinaryImage { get; set; }
        public Point Position { get; set; }
    }
}