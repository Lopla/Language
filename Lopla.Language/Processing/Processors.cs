using Lopla.Language.Environment;
using Lopla.Language.Interfaces;

namespace Lopla.Language.Processing
{
    using System.Collections.Generic;

    public class Processors
    {
        private readonly Stack<Processor> _processors = new Stack<Processor>();
        private IRuntime _runtime;

        public virtual void Init(IRuntime runtime)
        {
            _runtime = runtime;
        }

        public void Begin(GlobalScope stack)
        {
            _processors.Push(OnCreateProcessor(stack));
        }

        public virtual Processor OnCreateProcessor(GlobalScope stack)
        {
            return new Processor(_runtime, stack);
        }

        public void End()
        {
            _processors.Pop();
        }

        public Processor Get()
        {
            return _processors.Peek();
        }

        public void Stop()
        {
            foreach (var processor in _processors.ToArray())
                processor.Stop();
        }
    }
}