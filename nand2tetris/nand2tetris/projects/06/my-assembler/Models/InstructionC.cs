namespace my_assembler.Models
{
    internal class InstructionC : Instruction
    {
        private Fields? _fields;

        internal InstructionC()
        {
            _fields = null;
        }

        public Fields? GetFields()
        {
            return _fields;
        }

        public void SetFields(Fields field)
        {
            var fieldC = field as FieldsC;
            _fields = fieldC;
        }
    }
}