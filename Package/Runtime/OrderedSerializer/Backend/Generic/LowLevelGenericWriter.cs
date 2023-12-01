namespace OrderedSerializer
{
    public class LowLevelGenericWriter : ILowLevelGenericWriter
    {
        private readonly ILowLevelWriter _writer;

        public LowLevelGenericWriter(ILowLevelWriter writer)
        {
            _writer = writer;
        }
        
        public void Write(bool value)
        {
            _writer.WriteBool(value);
        }

        public void Write(byte value)
        {
            _writer.WriteByte(value);
        }

        public void Write(char value)
        {
            _writer.WriteChar(value);
        }

        public void Write(short value)
        {
            _writer.WriteShort(value);
        }

        public void Write(int value)
        {
            _writer.WriteInt(value);
        }

        public void Write(long value)
        {
            _writer.WriteLong(value);
        }

        public void Write(float value)
        {
            _writer.WriteFloat(value);
        }

        public void Write(double value)
        {
            _writer.WriteDouble(value);
        }

        public void Write(string? value)
        {
            _writer.WriteString(value);
        }

        public void Write(byte[]? value)
        {
            _writer.WriteBytes(value);
        }
    }
}