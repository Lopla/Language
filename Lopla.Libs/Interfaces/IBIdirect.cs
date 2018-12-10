namespace Lopla.Libs.Interfaces
{
    public interface IBidirect : ISender, ISubscribe
    {
        void Stop();
    }
}
