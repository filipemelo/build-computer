using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("my-assembler-test")]
namespace my_assembler.Models
{
    internal class InstructionA : Instruction
    {
        private Fields? _fields;

        internal InstructionA()
        {
            _fields = null;
        }

        internal InstructionA(bool isNumber, int? intValue, string? variableName)
        {
            _fields = new FieldsA(isNumber, intValue, variableName);
        }

        public Fields? GetFields()
        {
            return _fields;
        }

        public string GetMachineCode(SymbolTable symbolTable)
        {
            var fieldsA = _fields as FieldsA;
            
            var intValue = fieldsA.IntValue??0;
            var isNumber = fieldsA.IsNumber;
            var variableName = fieldsA.VariableName;

            var binaryValue = Convert.ToString(intValue, 2);
            var variableInt = symbolTable.GetVariableFrom(variableName)??0;
            var variableValue = Convert.ToString(variableInt, 2);
            var value = isNumber ? binaryValue : variableValue;

            return $"0{value}";
        }

        public void SetFields(Fields field)
        {
            var fieldA = field as FieldsA;
            _fields = fieldA;
        }
    }
}