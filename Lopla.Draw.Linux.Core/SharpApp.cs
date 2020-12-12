using Gtk;
using System;
using SkiaSharp.Views.Gtk;

namespace Lopla.Draw.Linux.Core
{
    class SharpApp : Window {
    
        public SharpApp() : base("Center")
        {
            SetDefaultSize(250, 200);
            SetPosition(WindowPosition.Center);
            
            DeleteEvent += delegate { Application.Quit(); };
                
            VBox vbox = new VBox(false, 5);
            HBox hbox = new HBox(true, 3);
            
            var drawingArea = new SKDrawingArea();
            drawingArea.SetSizeRequest(100,100);
            drawingArea.PaintSurface += (sender, e) => {
                var surface = e.Surface;
                var surfaceWidth = e.Info.Width;
                var surfaceHeight = e.Info.Height;

                var canvas = surface.Canvas;

                // draw on the canvas

                canvas.Clear(new SkiaSharp.SKColor(255,255,255) );
                canvas.Flush ();
            };


            Alignment valign = new Alignment(0, 1, 0, 0);
            vbox.PackStart(valign, true, false, 1);
            vbox.Add(drawingArea);


            Button ok = new Button("OK");
            ok.SetSizeRequest(70, 30);
            Button close = new Button("Close");
            
            hbox.Add(ok);
            hbox.Add(close);
            
            Alignment halign = new Alignment(1, 0, 0, 0);
            halign.Add(hbox);
            
            vbox.PackStart(halign, false, false, 3);

            Add(vbox);

            ShowAll();
        }
        
        public static void Main()
        {
            Application.Init();
            new SharpApp();        
            Application.Run();
        }
    }
}