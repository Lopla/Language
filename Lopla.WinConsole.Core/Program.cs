using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Lopla.Windows
{
    public static class Program
    {
        private const int ATTACH_PARENT_PROCESS = -1;
        public static bool ParentConsoleAvailble;

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            if (AttachConsole(ATTACH_PARENT_PROCESS)) ParentConsoleAvailble = true;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HiddenContext(args));
        }

        /// <summary>
        ///     https://social.msdn.microsoft.com/Forums/windows/en-US/dece45c8-9076-497e-9414-8cd9b34f572f/how-to-start-a-form-in-hidden-mode?forum=winforms
        ///     &prof=required
        /// </summary>
        private class HiddenContext : ApplicationContext
        {
            public HiddenContext(string[] args)
            {
                var form1 = new LoplaForm(args, ParentConsoleAvailble);
                form1.Visible = true;
                form1.FormClosing += form1_FormClosing;

                form1.Run();
            }

            private void form1_FormClosing(object sender, FormClosingEventArgs e)
            {
                ExitThread();
            }
        }
    }
}