using my_assembler;
using Microsoft.Extensions.DependencyInjection;

bool infoMode = false;

if(args.Contains("-h"))
{
    System.Console.WriteLine("To assembly use 'my-assebler.exe filename.asm'.");
    System.Console.WriteLine("Options:");
    System.Console.WriteLine("-i Info mode");
    System.Console.WriteLine("-h help");
}

if (!args.Any()) {
    Console.WriteLine("Must provide file as argument.");
    return;
}
if (!File.Exists(args[0]))
{
    Console.WriteLine("File can't be found.");
    return;
}

var argfile = args[0].Replace(".\\", "").Split('.');
string filename = $"{argfile[0]}.hack";
System.Console.WriteLine($"Filename output will be: {filename}");

if(args.Contains("-i"))
{
    System.Console.WriteLine("INFO MODE ON");
    infoMode = true;
}

if (infoMode) System.Console.WriteLine($"File to be assembled {args[0]}");

var lines = File.ReadAllLines(args[0]);

if (infoMode) {
    System.Console.WriteLine("Below code that will be assembled!");
    foreach (var line in lines) { System.Console.WriteLine(line);}
} 


var serviceProvider = new ServiceCollection()
            .AddTransient<Parser>()
            .AddTransient<Code>()
            .AddTransient<MachineCode>()
            .AddTransient<SymbolTable>()
            .AddTransient<Assembler>()
            .BuildServiceProvider();

var assembler = serviceProvider.GetService<Assembler>();
assembler.Process(lines, filename);