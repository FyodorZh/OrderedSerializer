namespace OrderedSerializer
{
    public interface ILowLevelReader
    {
        byte ReadByte();
        char ReadChar();
        short ReadShort();
        int ReadInt();
        long ReadLong();
        float ReadFloat();
        double ReadDouble();
        string ReadString();
    }

    public interface IReader : ILowLevelReader
    {
        void Reset();
        void BeginSection();
        bool EndSection();
    }
}