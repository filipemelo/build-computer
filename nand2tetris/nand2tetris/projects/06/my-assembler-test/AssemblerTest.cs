using FluentAssertions;
using my_assembler;
using Xunit;

namespace my_assembler_test
{
    public class AssemblerTest
    {
        private readonly Parser _parser;
        private readonly Code _code;
        private SymbolTable _symbolTable;

        public AssemblerTest()
        {
            _parser = new Parser();
            _code = new Code();
            _symbolTable = new SymbolTable();
        }

        [Fact]
        public void Translate_Success_Max()
        {
            var lines = new string[]
            {
                "// This file is part of www.nand2tetris.org",
                "// and the book \"The Elements of Computing Systems\"",
                "// by Nisan and Schocken, MIT Press.",
                "// File name: projects/06/max/Max.asm",
                "",
                "// Computes R2 = max(R0, R1)  (R0,R1,R2 refer to RAM[0],RAM[1],RAM[2])",
                "",
                "@R0",
                "D=M              // D = first number",
                "@R1",
                "D=D-M            // D = first number - second number",
                "@OUTPUT_FIRST",
                "D;JGT            // if D>0 (first is greater) goto output_first",
                "@R1",
                "D=M              // D = second number",
                "@OUTPUT_D",
                "0;JMP            // goto output_d",
                "(OUTPUT_FIRST)",
                "@R0             ",
                "D=M              // D = first number",
                "(OUTPUT_D)",
                "@R2",
                "M=D              // M[2] = D (greatest number)",
                "(INFINITE_LOOP)",
                "@INFINITE_LOOP",
                "0;JMP            // infinite loop"
            };

            var instructions = _parser.UnpackInstruction(lines, ref _symbolTable);
            _symbolTable.CompileSymbolTable();
            var result = _code.Translate(instructions, _symbolTable);

            var expectedResult = "" +
                "0000000000000000\n" +
                "1111110000010000\n" +
                "0000000000000001\n" +
                "1111010011010000\n" +
                "0000000000001010\n" +
                "1110001100000001\n" +
                "0000000000000001\n" +
                "1111110000010000\n" +
                "0000000000001100\n" +
                "1110101010000111\n" +
                "0000000000000000\n" +
                "1111110000010000\n" +
                "0000000000000010\n" +
                "1110001100001000\n" +
                "0000000000001110\n" +
                "1110101010000111\n";

            result.Should().Be(expectedResult);
        }
    }
}