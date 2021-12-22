namespace my_assembler.Models
{
    internal interface Instruction
    {
        Fields? GetFields();
        void SetFields(Fields field);
        string GetMachineCode(SymbolTable symbolTable);
    }
}