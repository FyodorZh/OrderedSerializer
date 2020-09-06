namespace OrderedSerializer
{
    public interface IReader
    {
        void BeginSection();
        bool EndSection();

        byte ReadByte();
        char ReadChar();
        short ReadShort();
        int ReadInt();
        long ReadLong();
        string ReadString();
    }
}