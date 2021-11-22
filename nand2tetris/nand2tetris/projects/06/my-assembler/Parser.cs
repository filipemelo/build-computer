using my_assembler.Models;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("my_assembler_test")]
namespace my_assembler
{
    internal class Parser
    {
        internal object UnpackInstruction(string[] lines, ref SymbolTable symbolTable)
        {
            var row = 1;
            foreach (var line in lines)
            {
                var lineNoComments = RemoveComments(line);
                var cleanLine = RemoveSpaces(lineNoComments);
                if (!cleanLine.Any()) continue; // jump to another line and do not process this line, don't even increase row
                var fields = Analyse(cleanLine, row, ref symbolTable);
                row++;
            }

            throw new NotImplementedException();
        }
        
        private string RemoveComments(string lineNoSpace)
        {
            var foundSlash = false;
            var indexOfComment = 0;
            foreach (var c in lineNoSpace)
            {
                if (c == '/')
                {
                    if (foundSlash) 
                    {
                        break;
                    }
                    foundSlash = true;
                }
                indexOfComment++;
            }
            return lineNoSpace.Remove(indexOfComment);
        }
        
        private string RemoveSpaces(string line)
        {
            return line.Replace(" ", "");
        }
        
        private Instruction? Analyse(string line, int row, ref SymbolTable symbolTable)
        {
            if (line[0] == '@')
            {
                // A-Instruction
                return GetInstructionA(line, row, ref symbolTable);
            }

            if (line[0] == '(')
            {
                // Label - add this label and row to symbolTable
                return SetUpLabel(line, row, ref symbolTable);
            }

            // C-Instruction
            return GetInstructionC(line, row);
        }

        private Instruction? SetUpLabel(string line, int row, ref SymbolTable symbolTable)
        {
            var label = line.Replace("(", "").Replace(")", "");
            symbolTable.AddLabel(label, row);
            return null;
        }

        private Instruction GetInstructionC(string line, int row)
        {
            var splitEqual = line.Split('=');
            string? dest = null;
            string analyze = line;
            if (splitEqual.Length > 0)
            {
                dest = splitEqual[0];
                analyze = splitEqual[1];
            }

            var splitSemicolon = analyze.Split(';');
            string? comp = null;
            string? jump = null;
            if (splitSemicolon.Length > 0)
            {
                comp = splitSemicolon[0];
                jump = splitSemicolon[1];
            }
            else 
            {
                comp = analyze;
            }

            var instructionC = new InstructionC();
            var fieldsC = new FieldsC { Dest = dest, Comp = comp, Jump = jump };
            instructionC.SetFields(fieldsC);
            return instructionC;
        }

        private Instruction GetInstructionA(string line, int row, ref SymbolTable symbolTable)
        {
            var instructionA = new InstructionA();
            var isNumber = int.TryParse(line.Substring(1), out int intValue);
            var fieldsA = new FieldsA(isNumber, intValue, line.Substring(1));
            if (!isNumber)
            {
                symbolTable.AddNamedVariable(line.Substring(1));
            }
            instructionA.SetFields(fieldsA);
            return instructionA;
        }
    }
}