using System;
using System.Windows.Forms;

namespace Lopla.Windows
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new HiddenContext(args));
        }

        class HiddenContext : ApplicationContext
        {
            public HiddenContext(string[] args)
            {
                LoplaForm form1 = new LoplaForm(args);
                form1.Visible = false;
                form1.FormClosed += new System.Windows.Forms.FormClosedEventHandler(form1_FormClosed);

                form1.Run();
            }
            
            void form1_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
            {
                this.ExitThread();
            }
        }
    }
}