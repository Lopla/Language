﻿using System;
using Lopla.Draw.Messages;
using Lopla.Language.Binary;
using Lopla.Language.Errors;
using Lopla.Language.Libraries;
using Lopla.Language.Processing;
using Lopla.Libs.Interfaces;
using String = Lopla.Language.Binary.String;

namespace Lopla.Draw.Libs
{
    public class Draw : BaseLoplaLibrary
    {
        private readonly SkiaDrawLoplaEngine _renderingEngine;
        private readonly ISender _uiEventsProvider;

        /// <summary>
        /// Draw library for lopla
        /// </summary>
        /// <param name="drawEngine">target draw engine</param>
        /// <param name="uiEventsProvider">used by one function .: WaitForEvent
        /// if something is send to that queue it will be passed to user
        /// otherwise it's not used by any purpose
        /// ui events for the system incl: click, SetCanvas, keypress</param>
        public Draw(
            SkiaDrawLoplaEngine drawEngine,
            ISender uiEventsProvider = null
            )
        {
            _renderingEngine = drawEngine;
            _uiEventsProvider = uiEventsProvider;

            Add("Clear", Clear, "r", "g", "b");

            Add("SetColor", SetColor, "r", "g", "b");

            Add("Box", Box, "a", "b", "c", "d");
            Add("Line", Line, "a", "b", "c", "d");

            Add("Image", Image, "filename", "x", "y");
            Add("Sprite", Sprite, "assembly", "filename", "x", "y", "sx", "sy", "w", "h");

            Add("Flush", Flush);

            Add("Write", Write, "text", "align", "offset");
            Add("Log", Log, "s");
            Add("Text", Text, "posx", "posy", "text");

            Add("GetTextSize", GetTextSize, "text");
            Add("GetTextInfo", GetTextInfo);
            Add("GetCanvasSize", GetCanvasSize);

            Add("SetCanvas", SetCanvas, "a", "b");

            Add("WaitForEvent", WaitForEvent);
        }

        private Result GetCanvasSize(Mnemonic expression, Runtime runtime)
        {
            var c = _renderingEngine.DrawContext.CanvasSize();
            return new Result(new LoplaList(
                new Result(new Number(c.X)),
                new Result(new Number(c.Y))
            ));
        }

        private Result WaitForEvent(Mnemonic expression, Runtime runtime)
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
                        new Result(k.Char))
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

        private Result GetTextInfo(Mnemonic expression, Runtime runtime)
        {
            var pp = new PaintProvider();
            var dev = pp.GetPaintDevice();
            var metrics = dev.FontMetrics;

            var size = dev.MeasureText("!");

            return new Result(new LoplaList(
                    new Result(new Number((decimal) size)),
                    new Result(new Number((decimal) dev.TextSize))
                )
            );
        }

        private Result GetTextSize(Mnemonic expression, Runtime runtime)
        {
            var pp = new PaintProvider();
            var path = pp.GetPaintDevice().GetTextPath(GetArgument<String>("text", runtime).Value, 0, 0);
            var bound = path.Bounds;

            return new Result(new LoplaList(
                new Result(new Number(Convert.ToInt32(bound.Width))),
                new Result(new Number(Convert.ToInt32(bound.Height)))));
        }

        private Result Text(Mnemonic expression, Runtime runtime)
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

            _renderingEngine.Send(new Text
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

        private Result SetCanvas(Mnemonic expression, Runtime runtime)
        {
            if (runtime.GetVariable("a").Get(runtime) is Number x1 &&
                runtime.GetVariable("b").Get(runtime) is Number y1)
                _renderingEngine.Send(new SetCanvas
                {
                    Size = new Point
                    {
                        X = x1.Value,
                        Y = y1.Value
                    }
                });

            return new Result();
        }

        private Result Flush(Mnemonic expression, Runtime runtime)
        {
            _renderingEngine.Send(new Flush());

            return new Result();
        }

        private Result Sprite(Mnemonic expression, Runtime runtime)
        {
            if (runtime.GetVariable("x").Get(runtime) is Number x1 &&
                runtime.GetVariable("y").Get(runtime) is Number y1 &&
                runtime.GetVariable("sx").Get(runtime) is Number sx &&
                runtime.GetVariable("sy").Get(runtime) is Number sy &&
                runtime.GetVariable("w").Get(runtime) is Number w &&
                runtime.GetVariable("h").Get(runtime) is Number h &&
                runtime.GetVariable("filename").Get(runtime) is String name &&
                runtime.GetVariable("assembly").Get(runtime) is String assembly)
                _renderingEngine.Send(new Sprite
                    {
                        AssemblyName = assembly.Value,
                        ResourceName = name.Value,
                        Position = new Point
                        {
                            X = x1.Value,
                            Y = y1.Value
                        },
                        Rectangle = new Rect
                        {
                            Position = new Point
                            {
                                X = sx.Value,
                                Y = sy.Value
                            },
                            Size = new Point
                            {
                                X = w.Value,
                                Y = h.Value
                            }
                        }
                    }
                );
            else
                runtime.AddError(new RuntimeError("Incorrect paramters provided."));
            return new Result();
        }

        private Result Image(Mnemonic expression, Runtime runtime)
        {
            if (runtime.GetVariable("x").Get(runtime) is Number x1 &&
                runtime.GetVariable("y").Get(runtime) is Number y1 &&
                runtime.GetVariable("filename").Get(runtime) is String name)
                _renderingEngine.Send(new Image
                {
                    File = name.Value,
                    Position = new Point
                    {
                        X = x1.Value,
                        Y = y1.Value
                    }
                });
            else
                runtime.AddError(new RuntimeError("Incorrect paramters provided."));

            return new Result();
        }

        private Result Box(Mnemonic expression, Runtime runtime)
        {
            if (runtime.GetVariable("a").Get(runtime) is Number x1 &&
                runtime.GetVariable("b").Get(runtime) is Number y1 &&
                runtime.GetVariable("c").Get(runtime) is Number x2 &&
                runtime.GetVariable("d").Get(runtime) is Number y2)
                _renderingEngine.Send(new Box
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

        private Result SetColor(Mnemonic expression, Runtime runtime)
        {
            if (runtime.GetVariable("r").Get(runtime) is Number r &&
                runtime.GetVariable("g").Get(runtime) is Number g &&
                runtime.GetVariable("b").Get(runtime) is Number b)
                _renderingEngine.Send(new SetColor
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

        private Result Line(Mnemonic expression, Runtime runtime)
        {
            if (runtime.GetVariable("a").Get(runtime) is Number x1 &&
                runtime.GetVariable("b").Get(runtime) is Number y1 &&
                runtime.GetVariable("c").Get(runtime) is Number x2 &&
                runtime.GetVariable("d").Get(runtime) is Number y2)
                _renderingEngine.Send(new Line
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

        private Result Write(Mnemonic expression, Runtime runtime)
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

            _renderingEngine.Send(new Write
            {
                Align = (Aligmnent) alingment.Value,
                Text = text,
                Offset = offset.Value
            });
            return new Result();
        }

        private Result Log(Mnemonic expression, Runtime runtime)
        {
            var text = "";
            if (runtime.GetVariable("s").Get(runtime) is String s) text = s.Value;
            if (runtime.GetVariable("s").Get(runtime) is Number i) text = i.Value.ToString();
            if (runtime.GetVariable("s").Get(runtime) is LoplaList l) text = $"array [{l.Length}]";
            _renderingEngine.Send(new Log
            {
                Text = text
            });
            return new Result();
        }

        private Result Clear(Mnemonic expression, Runtime runtime)
        {
            if (runtime.GetVariable("r").Get(runtime) is Number r &&
                runtime.GetVariable("g").Get(runtime) is Number g &&
                runtime.GetVariable("b").Get(runtime) is Number b)
                _renderingEngine.Send(new Clear
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