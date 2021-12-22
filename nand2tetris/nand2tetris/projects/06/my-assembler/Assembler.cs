using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("my_assembler_test")]
namespace my_assembler
{
    internal class Assembler
    {
        private readonly Parser _parser;
        private readonly Code _code;
        private SymbolTable _symbolTable;
        private readonly MachineCode _machineCode;

        internal Assembler(Parser parser, Code code, SymbolTable symbolTable, MachineCode machineCode)
        {
            _parser = parser;
            _code = code;
            _symbolTable = symbolTable;
            _machineCode = machineCode;
        }

        internal void Process(string[] lines){
            var instructions = _parser.UnpackInstruction(lines, ref _symbolTable);
            var binaryText = _code.Translate(instructions, _symbolTable);
            _machineCode.Save(binaryText);
        }
    }
}