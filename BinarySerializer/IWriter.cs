namespace BinarySerializer
{
    public interface IWriter
    {
        void BeginSection();
        void EndSection();

        void WriteByte(byte value);
        void WriteChar(char value);
        void WriteInt(int value);
        void WriteLong(long value);
        void WriteString(string value);
    }
}