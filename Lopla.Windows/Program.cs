using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;

namespace Lopla.Windows
{
    public static class Program
    {
        private const int ATTACH_PARENT_PROCESS = -1;

        private const int STD_OUTPUT_HANDLE = -11;
        private const int STD_ERROR_HANDLE = -12;
        private const int STD_INPUT_HANDLE = -10;

        public static bool ParentConsoleAvailble;

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int AllocConsole();

        [DllImport("kernel32.dll",
            EntryPoint = "GetStdHandle",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            if(AttachConsole(ATTACH_PARENT_PROCESS)){
                ParentConsoleAvailble = true;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HiddenContext(args));
        }

        private static void StealOut()
        {
            var stdHandle = GetStdHandle(STD_OUTPUT_HANDLE); 
            var safeFileHandle = new SafeFileHandle(stdHandle, true);
            var fileStream = new FileStream(safeFileHandle, FileAccess.Write);
            var encoding = Encoding.ASCII;
            var standardOutput = new StreamWriter(fileStream, encoding);
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
        }

        private static void StealIn()
        {
            var stdHandle = GetStdHandle(STD_INPUT_HANDLE); 
            var safeFileHandle = new SafeFileHandle(stdHandle, true);
            var fileStream = new FileStream(safeFileHandle, FileAccess.Read);
            var encoding = Encoding.ASCII;
            var standardOutput = new StreamReader
                (fileStream, encoding);

            Console.SetIn(standardOutput);
        }

        /// <summary>
        ///     https://social.msdn.microsoft.com/Forums/windows/en-US/dece45c8-9076-497e-9414-8cd9b34f572f/how-to-start-a-form-in-hidden-mode?forum=winforms
        ///     &prof=required
        /// </summary>
        private class HiddenContext : ApplicationContext
        {
            public HiddenContext(string[] args)
            {
                var form1 = new LoplaForm(args, Program.ParentConsoleAvailble);
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