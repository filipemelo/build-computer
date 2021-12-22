using System.Text;
using my_assembler.Models;

namespace my_assembler
{
    internal class Code
    {
        internal string Translate(List<Instruction> instructions, SymbolTable symbolTable)
        {
            var sb = new StringBuilder();
            
            foreach(var instruction in instructions)
            {
                var machineCode = instruction.GetMachineCode(symbolTable);
                sb.Append(machineCode);
            }

            return sb.ToString();
        }
    }
}