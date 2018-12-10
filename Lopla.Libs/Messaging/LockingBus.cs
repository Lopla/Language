namespace Lopla.Libs.Messaging
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using Interfaces;

    public class LockingBus : ISender
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly BlockingCollection<Message> _queue = new BlockingCollection<Message>();

        public void Send<TArg>(TArg message) where TArg : ILoplaMessage
        {
            var key = GetKey<Bus, TArg>();

            _queue.Add(new Message
            {
                Key = key,
                Payload = message
            });
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        public Message WaitForMessage()
        {
            var token = _cts.Token;
            if (!token.IsCancellationRequested)
                try
                {
                    var data = _queue.Take(token);
                    return data;
                }
                catch (OperationCanceledException)
                {
                }

            return null;
        }

        protected int GetKey<T, T1>()
        {
            return (typeof(T).FullName + typeof(T1).FullName).GetHashCode();
        }
    }
}