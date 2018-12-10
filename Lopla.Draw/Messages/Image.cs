using Lopla.Libs.Interfaces;

namespace Lopla.Draw.Messages
{
    public class Image : ILoplaMessage
    {
        public string ResourceName;
        public string AssemblyName;
        public Point Position { get; set; }
    }
}