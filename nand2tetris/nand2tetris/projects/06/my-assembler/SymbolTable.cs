using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("my-assembler-test")]
namespace my_assembler
{
    public class SymbolTable
    {
        private Dictionary<string, int> _variableTable;
        private Dictionary<string, int> _loopTable;
        private Dictionary<string, int> _specialCases = new Dictionary<string, int>
        {
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
        public SymbolTable()
        {
            _variableTable = new Dictionary<string, int>();
            _loopTable = new Dictionary<string, int>();
            _variableNumber = 16;
        }

        internal void AddNamedVariable(string variable)
        {
            if (_specialCases.ContainsKey(variable))
                return;
            if (_variableTable.ContainsKey(variable))
                return;
            if (_loopTable.ContainsKey(variable))
                return;
            _variableTable.Add(variable, _variableNumber++);
        }

        internal void AddLabel(string label, int row)
        {
            if (_variableTable.ContainsKey(label)) 
            {
                _variableTable.Remove(label);
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
            var initialVariableInt = 16;
            var compiledVariableTable = new Dictionary<string, int>();
            foreach (var item in _variableTable)
            {
                compiledVariableTable.Add(item.Key, initialVariableInt++);
            }
            _variableTable = compiledVariableTable;
            _variableNumber = initialVariableInt;
        }
    }
}