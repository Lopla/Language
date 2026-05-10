using System;
using System.IO;
using System.Linq;
using System.Reflection;

class Program {
    static void Main() {
        var packageDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages", "avalonia", "12.0.2", "ref", "net10.0");
        var asm = Assembly.LoadFrom(Path.Combine(packageDir, "Avalonia.Base.dll"));
        var type = asm.GetType("Avalonia.Media.FormattedText");
        Console.WriteLine(type?.FullName ?? "Type missing");
        Console.WriteLine("Constructors:");
        foreach (var c in type.GetConstructors()) {
            Console.WriteLine("  " + c.ToString());
        }
        Console.WriteLine("Properties:");
        foreach (var p in type.GetProperties()) {
            Console.WriteLine("  " + p.PropertyType.Name + " " + p.Name);
        }
    }
}
