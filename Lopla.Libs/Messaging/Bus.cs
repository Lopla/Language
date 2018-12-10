using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lopla.Libs.Interfaces;
using Lopla.Libs.Messages;

namespace Lopla.Libs.Messaging
{
    public class Bus : LockingBus, IBidirect
    {
        private static int _consumerCount;
        private readonly bool _addShutDownSubscription = true;

        private readonly Dictionary<int, List<Action<ILoplaMessage>>> _subscriptions =
            new Dictionary<int, List<Action<ILoplaMessage>>>();

        public Bus()
        {
            StartConsumer();

            if (_addShutDownSubscription)
                Subscribe<ShutDownQueue>(queue => { Stop(); });
        }

        public Bus(bool addShutDownSubscription)
            : this()
        {
            _addShutDownSubscription = addShutDownSubscription;
        }

        public void Subscribe<TArg>(Action<TArg> action) where TArg : ILoplaMessage
        {
            var key = GetKey<Bus, TArg>();
            if (!_subscriptions.ContainsKey(key)) _subscriptions.Add(key, new List<Action<ILoplaMessage>>());

            _subscriptions[key].Add(message => { action.Invoke((TArg) message); });
        }

        private void StartConsumer()
        {
            Task.Run(() =>
            {
                _consumerCount++;
                Message result = null;
                do
                {
                    result = WaitForMessage();
                    if (result != null)
                        if (_subscriptions.ContainsKey(result.Key))
                            _subscriptions[result.Key].ForEach(m => { OnRun(m, result.Payload); });
                } while (result != null);

                _consumerCount--;
            });
        }

        protected virtual void OnRun(Action<ILoplaMessage> action, ILoplaMessage message)
        {
            action.Invoke(message);
        }
    }
}