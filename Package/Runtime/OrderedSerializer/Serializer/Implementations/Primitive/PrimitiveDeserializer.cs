using System;

namespace OrderedSerializer
{
    public class PrimitiveDeserializer : IPrimitiveReader
    {
        protected readonly IReader _reader;

        public bool IsWriter => false;
        public ILowLevelReader Reader => _reader;
        public ILowLevelWriter Writer => throw new InvalidOperationException();

        protected PrimitiveDeserializer(IReader reader)
        {
            _reader = reader;
        }

        public void Add(ref bool value)
        {
            value = _reader.ReadBool();
        }
        public void Read(out bool value)
        {
            value = _reader.ReadBool();
        }

        public void Add(ref byte value)
        {
            value = _reader.ReadByte();
        }
        public void Read(out byte value)
        {
            value = _reader.ReadByte();
        }

        public void Add(ref sbyte value)
        {
            var uValue = _reader.ReadByte();
            value = unchecked((sbyte)uValue);
        }
        public void Read(out sbyte value)
        {
            var uValue = _reader.ReadByte();
            value = unchecked((sbyte)uValue);
        }

        public void Add(ref char value)
        {
            value = _reader.ReadChar();
        }
        public void Read(out char value)
        {
            value = _reader.ReadChar();
        }

        public void Add(ref short value)
        {
            value = _reader.ReadShort();
        }
        public void Read(out short value)
        {
            value = _reader.ReadShort();
        }

        public void Add(ref ushort value)
        {
            var shortValue = _reader.ReadShort();
            value = unchecked((ushort)shortValue);
        }
        public void Read(out ushort value)
        {
            var shortValue = _reader.ReadShort();
            value = unchecked((ushort)shortValue);
        }

        public void Add(ref int value)
        {
            value = _reader.ReadInt();
        }
        public void Read(out int value)
        {
            value = _reader.ReadInt();
        }

        public void Add(ref uint value)
        {
            var sValue = _reader.ReadInt();
            value = unchecked((uint)sValue);
        }
        public void Read(out uint value)
        {
            var sValue = _reader.ReadInt();
            value = unchecked((uint)sValue);
        }

        public void Add(ref long value)
        {
            value = _reader.ReadLong();
        }
        public void Read(out long value)
        {
            value = _reader.ReadLong();
        }

        public void Add(ref ulong value)
        {
            var sValue = _reader.ReadLong();
            value = unchecked((ulong)sValue);
        }
        public void Read(out ulong value)
        {
            var sValue = _reader.ReadLong();
            value = unchecked((ulong)sValue);
        }

        public void Add(ref float value)
        {
            value = _reader.ReadFloat();
        }
        public void Read(out float value)
        {
            value = _reader.ReadFloat();
        }
        
        public void Add(ref double value)
        {
            value = _reader.ReadDouble();
        }
        public void Read(out double value)
        {
            value = _reader.ReadDouble();
        }

        public void Add(ref string? value)
        {
            value = _reader.ReadString();
        }
        public void Read(out string? value)
        {
            value = _reader.ReadString();
        }
        
        public void Add(ref byte[]? value)
        {
            value = _reader.ReadBytes();
        }
        public void Read(out byte[]? value)
        {
            value = _reader.ReadBytes();
        }
    }
}