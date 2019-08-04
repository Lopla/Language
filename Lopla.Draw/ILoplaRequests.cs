namespace Lopla.Draw
{
    using Messages;

    public interface ILoplaRequestsHandler
    {
        Point GetCanvasSize();

        void Invalidate();

        void SetCanvasSize(decimal sizeX, decimal sizeY);
    }
}