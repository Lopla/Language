namespace Lopla.Draw.Messages
{
    using Language.Binary;
    using Lopla.Libs.Interfaces;

    public class Key : ILoplaMessage
    {
        public Number Char;

        public bool Down { get;  set; }
    }
}