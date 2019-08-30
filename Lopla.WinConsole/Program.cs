namespace Lopla.WinConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Windows.Program.ParentConsoleAvailble = true;
            Windows.Program.Main(args);
        }
    }
}