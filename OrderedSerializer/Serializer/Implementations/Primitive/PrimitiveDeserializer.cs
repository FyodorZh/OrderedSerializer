using System;

namespace OrderedSerializer
{
    public class PrimitiveDeserializer : IPrimitiveSerializer
    {
        protected readonly IReader _reader;

        public bool IsWriter => false;

        public PrimitiveDeserializer(IReader reader)
        {
            _reader = reader;
        }

        public void Add(ref bool value)
        {
            switch (_reader.ReadByte())
            {
                case 0:
                    value = false;
                    break;
                case 1:
                    value = true;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public void Add(ref byte value)
        {
            value = _reader.ReadByte();
        }

        public void Add(ref char value)
        {
            value = _reader.ReadChar();
        }

        public void Add(ref int value)
        {
            value = _reader.ReadInt();
        }

        public void Add(ref long value)
        {
            value = _reader.ReadLong();
        }

        public void Add(ref string value)
        {
            value = _reader.ReadString();
        }
    }
}