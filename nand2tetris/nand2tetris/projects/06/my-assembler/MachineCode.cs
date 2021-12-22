namespace my_assembler
{
    internal class MachineCode
    {
        internal void Save(string binaryText)
        {
            File.WriteAllText("machinecode.txt", binaryText);
        }
    }
}