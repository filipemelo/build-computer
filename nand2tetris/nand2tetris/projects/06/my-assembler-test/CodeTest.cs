using System.Collections.Generic;
using FluentAssertions;
using my_assembler;
using my_assembler.Models;
using Xunit;

namespace my_assembler_test
{
    public class CodeTest
    {
        private Code _code;
        
        public CodeTest()
        {
            _code = new Code();
        }

        [Fact]
        public void Translate_Success()
        {
            var instructions = new List<Instruction>()
            {
                new InstructionA(false, null, "i"),
                new InstructionC("M", "1", null),
                new InstructionA(false, null, "sum"),
            };
            
            var symbolTable = new SymbolTable();
            symbolTable.AddNamedVariable("i");
            symbolTable.AddNamedVariable("sum");

            var result = _code.Translate(instructions, symbolTable);
            
            // 0000000000000000 => InstructionA
            // 111accccccdddjjj => InstructionC  1,1,1,a c1,c2,c3,c4 c5,c6,d1,d2 d3,j1,j2,j3
            var expectedResult = "0000000000010000\n" + // InstructionA(false, null, "i") @i
                "1110111111001000\n" + // InstructionC("M", "1", null) M=1
                "0000000000010001\n"; // InstructionA(false, null, "sum") @sum

            result.Should().Be(expectedResult);
        }
    }
}