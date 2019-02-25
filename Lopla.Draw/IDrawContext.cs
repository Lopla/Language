namespace Lopla.Draw
{
    using System.IO;
    using Messages;

    public interface IDrawContext
    {
        Stream GetResourceStream(string folder, string name);

        Point CanvasSize();

        Stream GetStream(string imgFile);
    }
}