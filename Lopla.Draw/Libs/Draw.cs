namespace Lopla.Draw.Libs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Language.Binary;
    using Language.Errors;
    using Language.Interfaces;
    using Language.Libraries;
    using Lopla.Libs.Interfaces;
    using Messages;
    using SkiaLayer;
    using SkiaSharp;
    using String = Language.Binary.String;

    public class Draw : BaseLoplaLibrary
    {
        private readonly ISkiaDrawLoplaEngine _renderingEngine;
        private readonly ISender _uiEventsProvider;

        /// <summary>
        ///     Draw library for lopla
        /// </summary>
        /// <param name="drawEngine">target draw engine</param>
        /// <param name="uiEventsProvider">
        ///     used by one function .: WaitForEvent
        ///     if something is send to that queue it will be passed to user
        ///     otherwise it's not used by any purpose
        ///     ui events for the system incl: click, SetCanvas, keypress
        /// </param>
        public Draw(
            ISkiaDrawLoplaEngine drawEngine,
            ISender uiEventsProvider = null
        )
        {
            _renderingEngine = drawEngine;
            _uiEventsProvider = uiEventsProvider;

            Add("Clear", Clear, "r", "g", "b");

            Add("SetColor", SetColor, "r", "g", "b");

            Add("Box", Box, "a", "b", "c", "d");
            Add("Line", Line, "a", "b", "c", "d");

            Add("Image", Image, "x", "y", "arrayOfBinaryData");
            Add("GetImageSize", GetImageSize, "arrayOfBinaryData");
            Add("GetImagePart", GetImagePart, "arrayOfBinaryData", "left", "top", "right", "bottom");

            Add("Animation", Animation, "x", "y", "animatedGif");

            Add("Flush", Flush);

            Add("Write", Write, "text", "align", "offset");
            Add("Log", Log, "s");
            Add("Text", Text, "posx", "posy", "text");

            Add("GetTextSize", GetTextSize, "text");
            Add("GetTextInfo", GetTextInfo);
            Add("GetCanvasSize", GetCanvasSize);

            Add("SetCanvas", SetCanvas, "a", "b");

            Add("WaitForEvent", WaitForEvent);

            Add("AddTimer", AddTimer, "interval");
        }

        private Result GetImagePart(Mnemonic expression, IRuntime runtime)
        {
            if(
                runtime.GetVariable("arrayOfBinaryData").Get(runtime) is LoplaList arrayImage
                && runtime.GetVariable("left").Get(runtime) is Number a
                && runtime.GetVariable("top").Get(runtime) is Number b
                && runtime.GetVariable("right").Get(runtime) is Number c
                && runtime.GetVariable("bottom").Get(runtime) is Number d
            )
            {
                var binaryData = arrayImage
                    .Select(e => e.Get(runtime) as Number)
                    .Select(n => n.ValueAsByte)
                    .ToArray();
                var resourceBitmap = SKBitmap.Decode(binaryData);

                var bmp = new SKBitmap();
                resourceBitmap.ExtractSubset(bmp, 
                    new SKRectI(a.ValueAsInt, b.ValueAsInt, c.ValueAsInt, d.ValueAsInt));

                SKDynamicMemoryWStream ms =new SKDynamicMemoryWStream();
                SKPixmap.Encode(ms, bmp.PeekPixels(), SKEncodedImageFormat.Png, 100);
                
                var data = ms.CopyToData();

                LoplaList loplaList = new LoplaList();
                
                foreach(var dbyte in data.ToArray())
                {
                    loplaList.Add(new Result(new Number(dbyte)));
                }

                return new Result(loplaList);
            }

            return new Result();
        }

        private Result GetImageSize(Mnemonic expression, IRuntime runtime)
        {
            //arrayOfBinaryData
            if(runtime.GetVariable("arrayOfBinaryData").Get(runtime) is LoplaList arrayImage){
                var binaryData = arrayImage.Select(e => e.Get(runtime) as Number).Select(n => n.ValueAsByte).ToArray();
                var resourceBitmap = SKBitmap.Decode(binaryData);

                return new Result(new LoplaList(){
                    new Result(new Number(resourceBitmap.Height)),
                    new Result(new Number(resourceBitmap.Width))
                });
            }
            else{
                return new Result();
            }
        }

        private static Dictionary<int, Thread> timerPool = new Dictionary<int, Thread>();

        private Result AddTimer(Mnemonic expression, IRuntime runtime)
        {
            if (runtime.GetVariable("interval").Get(runtime) is Number interval)
            {
                var id =  timerPool.Keys.Count > 0 ? 
                    timerPool.Keys.Max() + 1
                    : 0;

                timerPool.Add(id, new Thread(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(interval.ValueAsInt);

                        _uiEventsProvider.Send(new TimerElapsed()
                        {
                            Id = id
                        });
                    };
                }));

                timerPool[id].Start();

                return new Result(new Number(id));

            }
            else
            {
                runtime.AddError(new RuntimeError("No interval provided", expression));
                return new Result();
            }
        }

        private Result Animation(Mnemonic expression, IRuntime runtime)
        {
            if (runtime.GetVariable("x").Get(runtime) is Number x1 &&
                runtime.GetVariable("y").Get(runtime) is Number y1 &&
                runtime.GetVariable("animatedGif").Get(runtime) is LoplaList arrayImage)
            {
                var binaryData = arrayImage.Select(e => e.Get(runtime) as Number).Select(n => n.ValueAsByte).ToArray();

                _renderingEngine.Perform(new Animation
                {
                    BinaryImage = binaryData,
                    Position = new Point
                    {
                        X = x1.Value,
                        Y = y1.Value
                    }
                });
            }

            return new Result();
        }

        private Result GetCanvasSize(Mnemonic expression, IRuntime runtime)
        {
            var c = _renderingEngine.LoplaRequestsHandler.GetCanvasSize();
            return new Result(new LoplaList(
                new Result(new Number(c.X)),
                new Result(new Number(c.Y))
            ));
        }

        private Result WaitForEvent(Mnemonic expression, IRuntime runtime)
        {
            var m = _uiEventsProvider.WaitForMessage();
            if (m == null)
                return new Result();

            var name = m.Payload.GetType().Name;
            switch (m.Payload)
            {
                case Click c:
                    return new Result(new LoplaList(
                        new Result(new String(name)),
                        new Result(new Number(c.Pos.X)),
                        new Result(new Number(c.Pos.Y))
                    ));
                case Key k:
                    return new Result(new LoplaList(
                        new Result(new String(name)),
                        new Result(k.Char),
                        new Result(new Number(k.Down ? 1 : 0)))
                    );
                case SetCanvas sc:
                    return new Result(new LoplaList(
                        new Result(new String(name)),
                        new Result(new Number(sc.Size.X)),
                        new Result(new Number(sc.Size.Y))
                    ));
                default:
                    return new Result(new LoplaList(
                        new Result(new String(name))
                    ));
            }
        }

        private Result GetTextInfo(Mnemonic expression, IRuntime runtime)
        {
            var pp = new PaintProvider();
            var dev = pp.GetPaintDevice();
            var metrics = dev.FontMetrics;

            var size = dev.MeasureText("!");

            return new Result(new LoplaList(
                    new Result(new Number((decimal)size)),
                    new Result(new Number((decimal)dev.TextSize))
                )
            );
        }

        private Result GetTextSize(Mnemonic expression, IRuntime runtime)
        {
            var pp = new PaintProvider();
            var path = pp.GetPaintDevice().GetTextPath(GetArgument<String>("text", runtime).Value, 0, 0);
            var bound = path.Bounds;

            return new Result(new LoplaList(
                new Result(new Number(Convert.ToInt32(bound.Width))),
                new Result(new Number(Convert.ToInt32(bound.Height)))));
        }

        private Result Text(Mnemonic expression, IRuntime runtime)
        {
            var text = "";
            var textArgument = runtime.GetVariable("text").Get(runtime);
            if (textArgument is String s)
            {
                text = s.Value;
            }
            else if (textArgument is Number i)
            {
                text = i.Value.ToString();
            }
            else
            {
                runtime.AddError(new RuntimeError("Type not supported", expression));
                return new Result();
            }

            _renderingEngine.Perform(new Text
            {
                Label = text,
                Position = new Point    
                {
                    X = GetArgument<Number>("posx", runtime).Value,
                    Y = GetArgument<Number>("posy", runtime).Value
                }
            });

            return new Result();
        }

        private Result SetCanvas(Mnemonic expression, IRuntime runtime)
        {
            if (runtime.GetVariable("a").Get(runtime) is Number x1 &&
                runtime.GetVariable("b").Get(runtime) is Number y1)
                _renderingEngine.Perform(new SetCanvas
                {
                    Size = new Point
                    {
                        X = x1.Value,
                        Y = y1.Value
                    }
                });

            return new Result();
        }

        private Result Flush(Mnemonic expression, IRuntime runtime)
        {
            _renderingEngine.Perform(new Flush());

            return new Result();
        }

        private Result Image(Mnemonic expression, IRuntime runtime)
        {
            if (runtime.GetVariable("x").Get(runtime) is Number x1 &&
                runtime.GetVariable("y").Get(runtime) is Number y1 &&
                runtime.GetVariable("arrayOfBinaryData").Get(runtime) is LoplaList arrayImage)
            {
                var binaryData = arrayImage.Select(e => e.Get(runtime) as Number).Select(n => n.ValueAsByte).ToArray();

                _renderingEngine.Perform(new Image
                {
                    BinaryImage = binaryData,
                    Position = new Point
                    {
                        X = x1.Value,
                        Y = y1.Value
                    }
                });
            }
            else
            {
                runtime.AddError(new RuntimeError("Incorrect parameters provided."));
            }

            return new Result();
        }

        private Result Box(Mnemonic expression, IRuntime runtime)
        {
            if (runtime.GetVariable("a").Get(runtime) is Number x1 &&
                runtime.GetVariable("b").Get(runtime) is Number y1 &&
                runtime.GetVariable("c").Get(runtime) is Number x2 &&
                runtime.GetVariable("d").Get(runtime) is Number y2)
                _renderingEngine.Perform(new Box
                {
                    Start = new Point
                    {
                        X = x1.Value,
                        Y = y1.Value
                    },
                    End = new Point
                    {
                        X = x2.Value,
                        Y = y2.Value
                    }
                });
            else
                runtime.AddError(new RuntimeError("Color to clear the background not provided."));

            return new Result();
        }

        private Result SetColor(Mnemonic expression, IRuntime runtime)
        {
            if (runtime.GetVariable("r").Get(runtime) is Number r &&
                runtime.GetVariable("g").Get(runtime) is Number g &&
                runtime.GetVariable("b").Get(runtime) is Number b)
                _renderingEngine.Perform(new SetColor
                {
                    Color =
                        new Color
                        {
                            R = Convert.ToByte(r.Value),
                            G = Convert.ToByte(g.Value),
                            B = Convert.ToByte(b.Value)
                        }
                });
            else
                runtime.AddError(new RuntimeError("Color to clear the background not provided."));

            return new Result();
        }

        private Result Line(Mnemonic expression, IRuntime runtime)
        {
            if (runtime.GetVariable("a").Get(runtime) is Number x1 &&
                runtime.GetVariable("b").Get(runtime) is Number y1 &&
                runtime.GetVariable("c").Get(runtime) is Number x2 &&
                runtime.GetVariable("d").Get(runtime) is Number y2)
                _renderingEngine.Perform(new Line
                {
                    Start = new Point
                    {
                        X = x1.Value,
                        Y = y1.Value
                    },
                    End = new Point
                    {
                        X = x2.Value,
                        Y = y2.Value
                    }
                });
            else
                runtime.AddError(new RuntimeError("Color to clear the background not provided."));

            return new Result();
        }

        private Result Write(Mnemonic expression, IRuntime runtime)
        {
            var text = "";

            var alingment = runtime.GetVariable("align").Get(runtime) as Number;
            var offset = runtime.GetVariable("offset").Get(runtime) as Number;

            var textArgument = runtime.GetVariable("text").Get(runtime);
            if (textArgument is String s) text = s.Value;
            else if (textArgument is Number i)
                text = i.Value.ToString();
            else
                runtime.AddError(new RuntimeError($"Type {textArgument} not supproted.", expression));

            _renderingEngine.Perform(new Write
            {
                Align = (Aligmnent)alingment.Value,
                Text = text,
                Offset = offset.Value
            });
            return new Result();
        }

        private Result Log(Mnemonic expression, IRuntime runtime)
        {
            var text = "";
            if (runtime.GetVariable("s").Get(runtime) is String s) text = s.Value;
            if (runtime.GetVariable("s").Get(runtime) is Number i) text = i.Value.ToString();
            if (runtime.GetVariable("s").Get(runtime) is LoplaList l) text = $"array [{l.Length}]";
            _renderingEngine.Perform(new Log
            {
                Text = text
            });
            return new Result();
        }

        private Result Clear(Mnemonic expression, IRuntime runtime)
        {
            if (runtime.GetVariable("r").Get(runtime) is Number r &&
                runtime.GetVariable("g").Get(runtime) is Number g &&
                runtime.GetVariable("b").Get(runtime) is Number b)
                _renderingEngine.Perform(new Clear
                {
                    Color = new Color
                    {
                        R = Convert.ToByte(r.Value),
                        G = Convert.ToByte(g.Value),
                        B = Convert.ToByte(b.Value)
                    }
                });
            else
                runtime.AddError(new RuntimeError("Color to clear the background not provided."));

            return new Result();
        }
    }
}