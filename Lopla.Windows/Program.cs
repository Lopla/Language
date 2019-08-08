using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Lopla.Windows
{
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        { 
            AttachConsole(ATTACH_PARENT_PROCESS);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new HiddenContext(args));
        }

        /// <summary>
        /// https://social.msdn.microsoft.com/Forums/windows/en-US/dece45c8-9076-497e-9414-8cd9b34f572f/how-to-start-a-form-in-hidden-mode?forum=winforms&prof=required
        /// </summary>
        class HiddenContext : ApplicationContext
        {
            public HiddenContext(string[] args)
            {
                LoplaForm form1 = new LoplaForm(args);
                form1.Visible = false;
                form1.FormClosing += new FormClosingEventHandler(form1_FormClosing);

                form1.Run();
            }

            private void form1_FormClosing(object sender, FormClosingEventArgs e)
            {
                this.ExitThread();
            }
        }
    }
}