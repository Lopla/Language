using Lopla.Libs.Interfaces;

namespace Lopla.Draw.Messages
{
    public class TimerElapsed : ILoplaMessage
    {
        public int Id { get; set; }
    }
}