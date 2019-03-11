using Lopla.Language.Binary;
using Lopla.Libs.Interfaces;

namespace Lopla.Draw.Messages
{
    public class Image : ILoplaMessage
    {
        public byte[] BinaryImage { get; set; }
        public Point Position { get; set; }
    }
}