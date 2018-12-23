namespace Lopla.Draw
{
    using System.IO;
    using Lopla.Draw.Messages;

    public interface IDrawContext
    {
        Stream GetResourceStream(string folder, string name);

        Point CanvasSize();
    }
}