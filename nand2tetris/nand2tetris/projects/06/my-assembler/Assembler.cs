using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("my_assembler_test")]
namespace my_assembler
{
    public class Assembler
    {
        private readonly Parser _parser;
        private readonly Code _code;
        private SymbolTable _symbolTable;
        private readonly MachineCode _machineCode;

        public Assembler(Parser parser, Code code, SymbolTable symbolTable, MachineCode machineCode)
        {
            _parser = parser;
            _code = code;
            _symbolTable = symbolTable;
            _machineCode = machineCode;
        }

        internal void Process(string[] lines, string filename){
            var instructions = _parser.UnpackInstruction(lines, ref _symbolTable);
            _symbolTable.CompileSymbolTable();
            var binaryText = _code.Translate(instructions, _symbolTable);
            _machineCode.Save(binaryText, filename);
        }
    }
}