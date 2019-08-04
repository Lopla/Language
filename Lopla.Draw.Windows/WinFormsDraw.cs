using System.Windows.Forms;
using Lopla.Draw.SkiaLayer;
using Lopla.Language.Binary;
using Lopla.Language.Interfaces;
using Lopla.Language.Libraries;
using Lopla.Language.Processing;
using Lopla.Libs.Interfaces;

namespace Lopla.Draw.Windows
{
    public class WinFormsDraw : Draw.Libs.Draw
    {
        private static bool _visible;
        private readonly Form _form;

        public WinFormsDraw(Form form, SkiaDrawLoplaEngine drawEngine, ISender uiEventsProvider = null) : base(
            drawEngine,
            uiEventsProvider)
        {
            _form = form;
        }

        public override string Name => "Draw";

        public override Result Call(DoHandler action, Mnemonic context, IRuntime runtime)
        {
            if (!_visible)
            {
                if (_form.InvokeRequired)
                {
                    _form.Invoke((MethodInvoker) (() => { _form.Visible = true; }));
                }
                else
                {
                    _form.Visible = true;
                }

                _visible = true;
            }

            return base.Call(action, context, runtime);
        }
    }
}