using Xunit;
using my_assembler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using my_assembler.Models;

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

        var fields = result.Should().BeEquivalentTo(expectedInstructionA);
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

    }

    [Fact]
    public void Private_Verify_GetInstructionC_Comparing_Jump()
    {

    }

    [Fact]
    public void Private_Verify_GetInstructionC_Setting_Value_Jump()
    {

    }

    [Fact]
    public void Private_Verify_SetUpLabel_Verify_If_Added_To_SymbolTable()
    {
        
    }
}