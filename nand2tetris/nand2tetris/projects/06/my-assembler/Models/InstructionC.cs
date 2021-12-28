namespace my_assembler.Models
{
    internal class InstructionC : Instruction
    {
        private Fields? _fields;
        private Dictionary<string, string> _dest = new Dictionary<string, string>() 
        {
            { "",       "000" },
            { "M",      "001" },
            { "D",      "010" },
            { "MD",     "011" },
            { "A",      "100" },
            { "AM",     "101" },
            { "AD",     "110" },
            { "AMD",    "111" }
        };

        private Dictionary<string, string> _jump = new Dictionary<string, string>() 
        {
            { "",     "000" },
            { "JGT",    "001" },
            { "JEQ",    "010" },
            { "JGE",    "011" },
            { "JLT",    "100" },
            { "JNE",    "101" },
            { "JLE",    "110" },
            { "JMP",    "111" }
        };

        private Dictionary<string, string> _comp = new Dictionary<string, string>() 
        {
            { "0",              "0101010" },
            { "1",              "0111111" },
            { "-1",             "0111010" },
            { "D",              "0001100" },
            { "A",              "0110000" },
            { "M",              "1110000" },
            { "!D",             "0001101" },
            { "!A",             "0110001" },
            { "!M",             "1110001" },
            { "-D",             "0001111" },
            { "-A",             "0110011" },
            { "-M",             "1110011" },
            { "D+1",            "0011111" },
            { "A+1",            "0110111" },
            { "M+1",            "1110111" },
            { "D-1",            "0001110" },
            { "A-1",            "0110010" },
            { "M-1",            "1110010" },
            { "D+A",            "0000010" },
            { "D+M",            "1000010" },
            { "D-A",            "0010011" },
            { "D-M",            "1010011" },
            { "A-D",            "0000111" },
            { "M-D",            "1000111" },
            { "D&A",            "0000000" },
            { "D&M",            "1000000" },
            { "D|A",            "0010101" },
            { "D|M",            "1010101" }
        };

        internal InstructionC()
        {
            _fields = null;
        }

        internal InstructionC(string? dest, string? comp, string? jump)
        {
            _fields = new FieldsC 
            {
                Dest = dest,
                Comp = comp, 
                Jump = jump
            };
        }

        public Fields? GetFields()
        {
            return _fields;
        }

        public string GetDestCode(string dest)
        {
            if (_dest.ContainsKey(dest))
                return _dest[dest];

            throw new Exception($"Dest code was not found. [Dest: {dest}]");
        }

        public string GetJumpCode(string jump)
        {
            if (_jump.ContainsKey(jump))
                return _jump[jump];

            throw new Exception($"Jump code was not found. [Jump: {jump}]");
        }

        public string GetCompCode(string comp)
        {
            if (_comp.ContainsKey(comp))
                return _comp[comp];

            throw new Exception($"Jump code was not found. [Comp: {comp}]");
        }

        public string GetMachineCode(SymbolTable symbolTable)
        {
            var fieldsC = _fields as FieldsC;
            
            var dest = fieldsC.Dest;
            var comp = fieldsC.Comp;
            var jump = fieldsC.Jump;

            var destCode = GetDestCode(dest??"");
            var jumpCode = GetJumpCode(jump??"");
            var compCode = GetCompCode(comp);

            return $"111{compCode}{destCode}{jumpCode}\n";
        }

        public void SetFields(Fields field)
        {
            var fieldC = field as FieldsC;
            _fields = fieldC;
        }
    }
}