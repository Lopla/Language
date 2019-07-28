using System.IO;
using Lopla.Draw.Messages;

namespace Lopla.Draw
{
    public interface IDrawContext
    {
        Point GetCanvasSize();

        void Invalidate();

        void SetCanvasSize(int sizeX, int sizeY);
    }
}