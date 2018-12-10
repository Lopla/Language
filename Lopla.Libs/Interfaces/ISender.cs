namespace Lopla.Libs.Interfaces
{
    public interface ISender
    {
        void Send<TArg>(TArg message)
            where TArg : ILoplaMessage;

        Message WaitForMessage();
    }

    public class Message
    {
        public int Key;
        public ILoplaMessage Payload;
    }
}