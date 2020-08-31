namespace OrderedSerializer
{
    public interface IReader
    {
        void BeginSection();
        void EndSection();

        byte ReadByte();
        char ReadChar();
        int ReadInt();
        long ReadLong();
        string ReadString();
    }
}