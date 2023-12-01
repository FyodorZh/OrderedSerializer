namespace OrderedSerializer
{
    public class LowLevelGenericReader : ILowLevelGenericReader
    {
        private readonly ILowLevelReader _reader;

        public LowLevelGenericReader(ILowLevelReader reader)
        {
            _reader = reader;
        }
        
        public void Read(out bool value)
        {
            value = _reader.ReadBool();
        }

        public void Read(out byte value)
        {
            value = _reader.ReadByte();
        }

        public void Read(out char value)
        {
            value = _reader.ReadChar();
        }

        public void Read(out short value)
        {
            value = _reader.ReadShort();
        }

        public void Read(out int value)
        {
            value = _reader.ReadInt();
        }

        public void Read(out long value)
        {
            value = _reader.ReadLong();
        }

        public void Read(out float value)
        {
            value = _reader.ReadFloat();
        }

        public void Read(out double value)
        {
            value = _reader.ReadDouble();
        }

        public void Read(out string? value)
        {
            value = _reader.ReadString();
        }

        public void Read(out byte[]? value)
        {
            value = _reader.ReadBytes();
        }
    }
}