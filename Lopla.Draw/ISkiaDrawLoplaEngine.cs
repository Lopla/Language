using System;
using Lopla.Libs.Interfaces;
using SkiaSharp;

namespace Lopla.Draw
{
    public interface ISkiaDrawLoplaEngine : IDisposable
    {
        ILoplaRequestsHandler LoplaRequestsHandler { get; }
        void SetupCanvas(int x, int y);
        void Perform(ILoplaMessage instruction);
        void Render(SKCanvas canvas);
    }
}