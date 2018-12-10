namespace Lopla.Libs.Interfaces
{
    using System;

    public interface ISubscribe
    {
        void Subscribe<TArgs>(Action<TArgs> queue)
            where TArgs : ILoplaMessage;
    }
}