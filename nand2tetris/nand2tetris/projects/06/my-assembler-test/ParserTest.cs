using Xunit;
using my_assembler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using my_assembler.Models;
using System.Collections.Generic;

namespace my_assembler_test;

public class ParserTest
{
    private Parser _parser;

    public ParserTest()
    {
        _parser = new Parser();
    }

    [Fact]
    public void Private_Verify_RemoveComments_With_Comments()
    {
        string line = "A=D;JMP//Its a demo comment =)";
        PrivateObject obj = new PrivateObject(_parser);
        var result = obj.Invoke("RemoveComments", line);
        result.Should().Be("A=D;JMP");
    }

    [Fact]
    public void Private_Verify_RemoveComments_Only_Comments()
    {
        string line = "//A=D;JMP//Its a demo comment =)";
        PrivateObject obj = new PrivateObject(_parser);
        var result = obj.Invoke("RemoveComments", line);
        result.Should().Be("");
    }

    [Fact]
    public void Private_Verify_RemoveComments_No_Comments()
    {
        string line = "A=D;JMP";
        PrivateObject obj = new PrivateObject(_parser);
        var result = obj.Invoke("RemoveComments", line);
        result.Should().Be("A=D;JMP");
    }

    [Fact]
    public void Private_Verify_RemoveSpaces_InstructionC_With_Spaces()
    {
        string line = "A = D ; JMP ";
        PrivateObject obj = new PrivateObject(_parser);
        var result = obj.Invoke("RemoveSpaces", line);
        result.Should().Be("A=D;JMP");
    }

    [Fact]
    public void Private_Verify_GetInstructionA_With_Number()
    {
        string line = "@1";
        int row = 1;
        var symbolTable = new SymbolTable();
        PrivateObject obj = new PrivateObject(_parser);
        var result = obj.Invoke("GetInstructionA", line, row, symbolTable) as InstructionA;
        var expectedInstructionA = new InstructionA();
        expectedInstructionA.SetFields(new FieldsA(true, 1, null));
        var fields = result?.GetFields() as FieldsA;
        fields.IntValue.Should().Be(1);
        fields.IsNumber.Should().Be(true);
        fields.IntValue.Should().Be(1);
    }

    [Fact]
    public void Private_Verify_GetInstructionA_With_Variable()
    {
        string line = "@Loop";
        int row = 1;
        var symbolTable = new SymbolTable();
        PrivateObject obj = new PrivateObject(_parser);
        var result = obj.Invoke("GetInstructionA", line, row, symbolTable) as InstructionA;
        var fieldsA = result.GetFields() as FieldsA;
        fieldsA.IsNumber.Should().BeFalse();
        fieldsA.IntValue.Should().BeNull();
        fieldsA.VariableName.Should().Be("Loop");
    }

    [Fact]
    public void Private_Verify_GetInstructionC_Setting_Value()
    {
        string line = "D=A";
        int row = 1;
        PrivateObject obj = new PrivateObject(_parser);
        var result = obj.Invoke("GetInstructionC", line, row) as InstructionC;
        var fieldsC = result.GetFields() as FieldsC;
        fieldsC.Comp.Should().Be("A");
        fieldsC.Dest.Should().Be("D");
        fieldsC.Jump.Should().BeNull();
    }

    [Fact]
    public void Private_Verify_GetInstructionC_Setting_Value_Comma_End()
    {
        string line = "D=A;";
        int row = 1;
        PrivateObject obj = new PrivateObject(_parser);
        var result = obj.Invoke("GetInstructionC", line, row) as InstructionC;
        var fieldsC = result.GetFields() as FieldsC;
        fieldsC.Comp.Should().Be("A");
        fieldsC.Dest.Should().Be("D");
        fieldsC.Jump.Should().BeNull();
    }

    [Fact]
    public void Private_Verify_GetInstructionC_Comparing_Jump()
    {
        string line = "D+M;JMP";
        int row = 1;
        PrivateObject obj = new PrivateObject(_parser);
        var result = obj.Invoke("GetInstructionC", line, row) as InstructionC;
        var fieldsC = result.GetFields() as FieldsC;
        fieldsC.Dest.Should().BeNull();
        fieldsC.Comp.Should().Be("D+M");
        fieldsC.Jump.Should().Be("JMP");
    }

    [Fact]
    public void Private_Verify_GetInstructionC_Setting_Value_Jump()
    {
        string line = "M=D+M;JGT";
        int row = 1;
        PrivateObject obj = new PrivateObject(_parser);
        var result = obj.Invoke("GetInstructionC", line, row) as InstructionC;
        var fieldsC = result.GetFields() as FieldsC;
        fieldsC.Comp.Should().Be("D+M");
        fieldsC.Dest.Should().Be("M");
        fieldsC.Jump.Should().Be("JGT");
    }

    [Fact]
    public void Private_Verify_SetUpLabel_Verify_If_Added_To_SymbolTable()
    {
        string line = "(Loop)";
        int row = 16;
        var symbolTable = new SymbolTable();
        PrivateObject obj = new PrivateObject(_parser);
        object[] args = new object[] {line, row, symbolTable};
        var result = obj.Invoke("SetUpLabel", args) as Instruction;
        result.Should().BeNull();
        var value = symbolTable.GetLabelFrom("Loop");
        value.Should().Be(16);
    }

    [Fact]
    public void UnpackInstructions()
    {
        var lines = new string[] 
        {
            "@i",
            "M=1 // i=1",
            "@sum // sum=0",
            "M=0",
            "(Loop)",
            "@i // if (i-100)=0 goto End", //5
            "D=M",
            "@100",
            "D=D-A",
            "@End",
            "D;JGT",
            "@i // sum+=i",
            "D=M",
            "@sum",
            "M=D+M",
            "@i // i++",
            "M=M+1",
            "@Loop // goto Loop",
            "0;JMP",
            "(End)",
            "@End",
            "0;JMP"
        };
        
        var symbolTable = new SymbolTable();
        var instructions = _parser.UnpackInstruction(lines, ref symbolTable);

        var expectedInstructions = new List<Instruction> {
            new InstructionA(false, null, "i"),
            new InstructionC("M", "1", null),
            new InstructionA(false, null, "sum"),
            new InstructionC("M", "0", null),
            new InstructionA(false, null, "i"),
            new InstructionC("D", "M", null),
            new InstructionA(true, 100, null),
            new InstructionC("D", "D-A", null),
            new InstructionA(false, null, "end"),
            new InstructionC(null, "D", "JGT"),
            new InstructionA(false, null, "i"),
            new InstructionC("D", "M", null),
            new InstructionA(false, null, "sum"),
            new InstructionC("M", "D+M", null),
            new InstructionA(false, null, "i"),
            new InstructionC("M", "M+1", null),
            new InstructionA(false, null, "Loop"),
            new InstructionC(null, "0", "JMP"),
            new InstructionA(false, null, "End"),
            new InstructionC(null, "0", "JMP")
        };

        instructions.Should().Equals(expectedInstructions);
        
        symbolTable.GetLabelFrom("Loop").Should().Be(5);
        symbolTable.GetLabelFrom("End").Should().Be(19);

        symbolTable.GetVariableFrom("i").Should().Be(16);
        symbolTable.GetVariableFrom("sum").Should().Be(17);
    }

}