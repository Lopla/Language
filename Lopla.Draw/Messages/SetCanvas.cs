using Lopla.Libs.Interfaces;

namespace Lopla.Draw.Messages
{
    public class SetCanvas : ILoplaMessage
    {
        public Point Size { get; set; }
    }
}