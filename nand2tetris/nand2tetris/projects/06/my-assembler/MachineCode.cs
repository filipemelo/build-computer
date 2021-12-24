namespace my_assembler
{
    public class MachineCode
    {
        internal void Save(string binaryText, string filename)
        {
            File.WriteAllText(filename, binaryText);
        }
    }
}