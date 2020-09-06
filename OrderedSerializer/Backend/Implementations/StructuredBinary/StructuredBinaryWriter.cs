using System;
using System.Collections.Generic;

namespace OrderedSerializer.StructuredBinaryBackend
{
    public class StructuredBinaryWriter : IWriter
    {
        private List<Record> _section = new List<Record>();
        private readonly Stack<List<Record>> _stack = new Stack<List<Record>>();

        public void WriteTo(IWriter writer)
        {
            if (_stack.Count != 0)
            {
                throw new InvalidOperationException();
            }
            writer.WriteInt(_section.Count);
            foreach (var record in _section)
            {
                record.WriteTo(writer);
            }
        }

        public StructuredBinaryReader ConstructReader()
        {
            if (_stack.Count != 0)
            {
                throw new InvalidOperationException();
            }

            return new StructuredBinaryReader(new List<Record>(_section));
        }

        public void Clear()
        {
            _section.Clear();
            _stack.Clear();
        }

        public void BeginSection()
        {
            _stack.Push(_section);
            _section = new List<Record>();
        }

        public void EndSection()
        {
            Record r = new Record(_section);
            _section = _stack.Pop();
            _section.Add(r);
        }

        public void WriteByte(byte value)
        {
            _section.Add(new Record(value));
        }

        public void WriteChar(char value)
        {
            _section.Add(new Record(value));
        }

        public void WriteShort(short value)
        {
            _section.Add(new Record(value));
        }

        public void WriteInt(int value)
        {
            _section.Add(new Record(value));
        }

        public void WriteLong(long value)
        {
            _section.Add(new Record(value));
        }

        public void WriteString(string value)
        {
            _section.Add(new Record(value));
        }
    }
}