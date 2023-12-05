using System;

namespace OrderedSerializer
{
    public class PrimitiveSerializer : IPrimitiveWriter
    {
        protected readonly IWriter _writer;

        public bool IsWriter => true;
        public ILowLevelReader Reader => throw new InvalidOperationException();
        public ILowLevelWriter Writer => _writer;

        protected PrimitiveSerializer(IWriter writer)
        {
            _writer = writer;
        }

        public void Add(ref bool value)
        {
            _writer.WriteBool(value);
        }
        public void Write(bool value)
        {
            _writer.WriteBool(value);
        }

        public void Add(ref byte value)
        {
            _writer.WriteByte(value);
        }
        public void Write(byte value)
        {
            _writer.WriteByte(value);
        }

        public void Add(ref sbyte value)
        {
            byte uValue = unchecked((byte)value);
            _writer.WriteByte(uValue);
        }
        public void Write(sbyte value)
        {
            byte uValue = unchecked((byte)value);
            _writer.WriteByte(uValue);
        }

        public void Add(ref char value)
        {
            _writer.WriteChar(value);
        }
        public void Write(char value)
        {
            _writer.WriteChar(value);
        }

        public void Add(ref short value)
        {
            _writer.WriteShort(value);
        }
        public void Write(short value)
        {
            _writer.WriteShort(value);
        }

        public void Add(ref ushort value)
        {
            _writer.WriteShort(unchecked((short)value));
        }
        public void Write(ushort value)
        {
            _writer.WriteShort(unchecked((short)value));
        }

        public void Add(ref int value)
        {
            _writer.WriteInt(value);
        }
        public void Write(int value)
        {
            _writer.WriteInt(value);
        }

        public void Add(ref uint value)
        {
            int sValue = unchecked((int)value);
            _writer.WriteInt(sValue);
        }
        public void Write(uint value)
        {
            int sValue = unchecked((int)value);
            _writer.WriteInt(sValue);
        }

        public void Add(ref long value)
        {
            _writer.WriteLong(value);
        }
        public void Write(long value)
        {
            _writer.WriteLong(value);
        }

        public void Add(ref ulong value)
        {
            long sValue = unchecked((long)value);
            _writer.WriteLong(sValue);
        }
        public void Write(ulong value)
        {
            long sValue = unchecked((long)value);
            _writer.WriteLong(sValue);
        }

        public void Add(ref float value)
        {
            _writer.WriteFloat(value);
        }
        public void Write(float value)
        {
            _writer.WriteFloat(value);
        }

        public void Add(ref double value)
        {
            _writer.WriteDouble(value);
        }
        public void Write(double value)
        {
            _writer.WriteDouble(value);
        }

        public void Add(ref string? value)
        {
            _writer.WriteString(value);
        }
        public void Write(string? value)
        {
            _writer.WriteString(value);
        }
        
        public void Add(ref byte[]? value)
        {
            _writer.WriteBytes(value);
        }
        public void Write(byte[]? value)
        {
            _writer.WriteBytes(value);
        }
    }
}