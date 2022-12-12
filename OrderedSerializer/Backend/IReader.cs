namespace OrderedSerializer
{
    public interface ILowLevelReader
    {
        bool ReadBool();
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
        void BeginSection();
        bool EndSection();
    }
}