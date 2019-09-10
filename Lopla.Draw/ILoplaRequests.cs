using Lopla.Draw.Messages;

namespace Lopla.Draw
{
    public interface ILoplaRequestsHandler
    {
        Point GetCanvasSize();

        void Invalidate();

        void SetCanvasSize(decimal sizeX, decimal sizeY);
    }
}