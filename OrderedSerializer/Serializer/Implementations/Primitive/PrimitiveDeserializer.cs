using System;

namespace OrderedSerializer
{
    public class PrimitiveDeserializer : IPrimitiveSerializer
    {
        protected readonly IReader _reader;

        public bool IsWriter => false;
        public ILowLevelReader Reader => _reader;
        public ILowLevelWriter Writer => throw new InvalidOperationException();

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

        public void Add(ref sbyte value)
        {
            var uValue = _reader.ReadByte();
            value = unchecked((sbyte)uValue);
        }

        public void Add(ref char value)
        {
            value = _reader.ReadChar();
        }

        public void Add(ref short value)
        {
            value = _reader.ReadShort();
        }

        public void Add(ref ushort value)
        {
            value = _reader.ReadChar();
        }

        public void Add(ref int value)
        {
            value = _reader.ReadInt();
        }

        public void Add(ref uint value)
        {
            var sValue = _reader.ReadInt();
            value = unchecked((uint)sValue);
        }

        public void Add(ref long value)
        {
            value = _reader.ReadLong();
        }

        public void Add(ref ulong value)
        {
            var sValue = _reader.ReadLong();
            value = unchecked((ulong)sValue);
        }

        public void Add(ref float value)
        {
            value = _reader.ReadFloat();
        }
        
        public void Add(ref double value)
        {
            value = _reader.ReadDouble();
        }

        public void Add(ref string value)
        {
            value = _reader.ReadString();
        }
    }
}