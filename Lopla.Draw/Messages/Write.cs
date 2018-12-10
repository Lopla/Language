using Lopla.Libs.Interfaces;

namespace Lopla.Draw.Messages
{
    public class Write : ILoplaMessage
    {
        public string Text { get; set; }

        public Aligmnent Align { get; set; }

        public decimal Offset { get; set; }
    }

    public enum Aligmnent
    {
        Left = 0,
        Center = 1,
        Right = 2
    }
}