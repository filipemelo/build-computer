using System.Text;

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
            _variableNumber = 0;
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
        }
    }
}