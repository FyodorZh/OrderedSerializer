namespace OrderedSerializer
{
    public interface ILowLevelWriter
    {
        void WriteByte(byte value);
        void WriteChar(char value);
        void WriteShort(short value);
        void WriteInt(int value);
        void WriteLong(long value);
        void WriteFloat(float value);
        void WriteDouble(double value);
        void WriteString(string value);
    }

    public interface IWriter : ILowLevelWriter
    {
        void Reset();
        void BeginSection();
        void EndSection();
    }
}