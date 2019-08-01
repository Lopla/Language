using Lopla.Draw.SkiaLayer;
using Lopla.Language.Binary;
using Lopla.Language.Libraries;
using Lopla.Language.Processing;
using Lopla.Libs.Interfaces;

namespace Lopla.Windows
{
    public class WinFormsDraw : Draw.Libs.Draw
    {
        private readonly LoplaForm _form;
        private static bool Visible = false;
        public WinFormsDraw(LoplaForm form, SkiaDrawLoplaEngine drawEngine, ISender uiEventsProvider = null) : base(drawEngine,
            uiEventsProvider)
        {
            _form = form;
        }

        public override Result Call(DoHandler action, Mnemonic context, Runtime runtime)
        {
            if (!Visible)
            {
                this._form.Visible = true;
                Visible = true;
            }

            return base.Call(action, context, runtime);
        }
    }
}