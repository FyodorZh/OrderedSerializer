namespace OrderedSerializer
{
    public class PrimitiveSerializer : IPrimitiveSerializer
    {
        protected readonly IWriter _writer;

        public bool IsWriter => true;

        public PrimitiveSerializer(IWriter writer)
        {
            _writer = writer;
        }

        public void Add(ref bool value)
        {
            _writer.WriteByte((byte)(value ? 1 : 0));
        }

        public void Add(ref byte value)
        {
            _writer.WriteByte(value);
        }

        public void Add(ref char value)
        {
            _writer.WriteChar(value);
        }

        public void Add(ref short value)
        {
            _writer.WriteShort(value);
        }

        public void Add(ref int value)
        {
            _writer.WriteInt(value);
        }

        public void Add(ref long value)
        {
            _writer.WriteLong(value);
        }

        public void Add(ref string value)
        {
            _writer.WriteString(value);
        }
    }
}