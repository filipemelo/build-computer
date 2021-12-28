using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("my-assembler-test")]
namespace my_assembler
{
    public class SymbolTable
    {
        private Dictionary<string, int> _variableTable;
        private List<string> _symbols;
        private Dictionary<string, int> _loopTable;
        private Dictionary<string, int> _specialCases = new Dictionary<string, int>
        {
            { "SP",     00 },
            { "LCL",    01 },
            { "ARG",    02 },
            { "THIS",   03 },
            { "THAT",   04 },
            { "R0",     00 },
            { "R1",     01 },
            { "R2",     02 },
            { "R3",     03 },
            { "R4",     04 },
            { "R5",     05 },
            { "R6",     06 },
            { "R7",     07 },
            { "R8",     08 },
            { "R9",     09 },
            { "R10",    10 },
            { "R11",    11 },
            { "R12",    12 },
            { "R13",    13 },
            { "R14",    14 },
            { "R15",    15 },
            { "SCREEN", 16384 },
            { "KBD",    24576 } 
        };
        private int _variableNumber;
        private bool _isCompiled;
        public SymbolTable()
        {
            _symbols = new List<string>();
            _variableTable = new Dictionary<string, int>();
            _loopTable = new Dictionary<string, int>();
            _variableNumber = 16;
            _isCompiled = false;
        }

        internal void AddNamedVariable(string symbol)
        {
            if (_symbols.Contains(symbol))
                return;
            if (_specialCases.ContainsKey(symbol))
                return;
            if (_loopTable.ContainsKey(symbol))
                return;
            _symbols.Add(symbol);
        }

        internal void AddLabel(string label, int row)
        {
            if (_symbols.Contains(label)) 
            {
                _symbols.Remove(label);
            }

            if (_loopTable.ContainsKey(label)) 
            {
                _loopTable[label] = row;
            } 
            else 
            {
                _loopTable.Add(label, row);
            }
        }

        internal int? GetLabelFrom(string label)
        {
            if (_loopTable.ContainsKey(label))
                return _loopTable[label];
            return null;
        }

        internal int? GetVariableFrom(string variable)
        {
            if (!_isCompiled) CompileSymbolTable();
            if (_specialCases.ContainsKey(variable))
                return _specialCases[variable];
            if (_variableTable.ContainsKey(variable))
                return _variableTable[variable];
            if (_loopTable.ContainsKey(variable))
                return _loopTable[variable];
            return null;
        }

        internal void CompileSymbolTable()
        {
            foreach (var item in _symbols)
            {
                _variableTable.Add(item, _variableNumber++);
            }
            _isCompiled = true;
        }
    }
}