using my_assembler;
using Microsoft.Extensions.DependencyInjection;

if (!args.Any()) {
    Console.WriteLine("Must provide file as argument.");
    return;
}
if (!File.Exists(args[0]))
{
    Console.WriteLine("File can't be found.");
    return;
}

var lines = File.ReadAllLines(args[0]);

var serviceProvider = new ServiceCollection()
            .AddSingleton<Assembler>()
            .AddSingleton<Parser>()
            .AddSingleton<Code>()
            .AddSingleton<MachineCode>()
            .AddSingleton<SymbolTable>()
            .BuildServiceProvider();


var assembler = serviceProvider.GetRequiredService<Assembler>();
assembler.Process(lines);