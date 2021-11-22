namespace my_assembler.Models
{
    internal class FieldsA : Fields
    {
        public FieldsA(bool isNumber, int intValue, string variableName)
        {
            IsNumber = isNumber;
            IntValue = intValue;
            VariableName = variableName;
        }

        internal int? IntValue { get; private set; }
        internal string? VariableName { get; private set;}
        internal bool IsNumber { get; private set; }
    }
}