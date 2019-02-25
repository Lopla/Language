using Lopla.Libs.Interfaces;

namespace Lopla.Draw.Messages
{
    public class Image : ILoplaMessage
    {
        /// <summary>
        /// File with path
        /// </summary>
        public string File { get; set; }
        public Point Position { get; set; }
    }
}