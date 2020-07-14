namespace Lopla.Draw.Windows
{
    using System.Windows.Forms;
    using Language.Binary;
    using Language.Interfaces;
    using Language.Libraries;
    using Libs;
    using Lopla.Libs.Interfaces;

    public class WinFormsDraw : Draw
    {
        private static bool _visible;
        private readonly Form _form;

        public WinFormsDraw(Form form, ISkiaDrawLoplaEngine drawEngine, ISender uiEventsProvider = null) : base(
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
                    _form.Invoke((MethodInvoker) (() => { _form.Visible = true; }));
                else
                    _form.Visible = true;

                _visible = true;
            }

            return base.Call(action, context, runtime);
        }
    }
}