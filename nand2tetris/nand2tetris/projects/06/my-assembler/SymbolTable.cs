using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("my-assembler-test")]
namespace my_assembler
{
    internal class SymbolTable
    {
        private Dictionary<string, int> _variableTable;
        private Dictionary<string, int> _loopTable;
        private int _variableNumber;
        public SymbolTable()
        {
            _variableTable = new Dictionary<string, int>();
            _loopTable = new Dictionary<string, int>();
            _variableNumber = 16;
        }

        internal void AddNamedVariable(string variable)
        {
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
            if (_variableTable.ContainsKey(variable))
                return _variableTable[variable];
            return null;
        }

    }
}