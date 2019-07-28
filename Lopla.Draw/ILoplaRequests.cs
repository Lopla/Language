namespace Lopla.Draw
{
    using Messages;

    public interface ILoplaRequests
    {
        Point GetCanvasSize();

        void Invalidate();

        void SetCanvasSize(int sizeX, int sizeY);
    }
}