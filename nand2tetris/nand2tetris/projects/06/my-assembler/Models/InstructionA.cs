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

        public Fields? GetFields()
        {
            return _fields;
        }

        public void SetFields(Fields field)
        {
            var fieldA = field as FieldsA;
            _fields = fieldA;
        }
    }
}