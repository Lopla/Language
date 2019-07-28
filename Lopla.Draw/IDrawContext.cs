using System.IO;
using Lopla.Draw.Messages;

namespace Lopla.Draw
{
    public interface IDrawContext
    {
        Stream GetResourceStream(string folder, string name);

        Point GetCanvasSize();

        void Invalidate();

        void SetCanvasSize(int sizeX, int sizeY);
    }
}