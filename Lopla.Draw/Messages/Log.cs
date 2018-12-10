using Lopla.Libs.Interfaces;

namespace Lopla.Draw.Messages
{
    public class Log : ILoplaMessage
    {
        public string Text { get; set; }
    }
}