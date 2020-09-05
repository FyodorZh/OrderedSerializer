using System;
using System.Collections.Generic;

namespace OrderedSerializer.StructuredBinaryBackend
{
    public class StructuredBinaryReader : IReader
    {
        private List<Record> _section;
        private readonly Stack<List<Record>> _sections = new Stack<List<Record>>();
        private readonly Stack<int> _positions = new Stack<int>();

        private int _position;

        public StructuredBinaryReader(List<Record> data)
        {
            _section = data;
        }

        private void CheckType(Record r, RecordType type)
        {
            if (r.Type != type)
            {
                throw new InvalidOperationException();
            }
        }

        public void BeginSection()
        {
            Record r = _section[_position];
            CheckType(r, RecordType.Section);

            _sections.Push(_section);
            _positions.Push(_position + 1);

            _section = r.Section;
            _position = 0;
        }

        public void EndSection()
        {
            _position = _positions.Pop();
            _section = _sections.Pop();
        }

        public byte ReadByte()
        {
            Record r = _section[_position++];
            CheckType(r, RecordType.Byte);
            checked
            {
                return (byte)r.Value;
            }
        }

        public char ReadChar()
        {
            Record r = _section[_position++];
            CheckType(r, RecordType.Char);
            checked
            {
                return (char)r.Value;
            }
        }

        public int ReadInt()
        {
            Record r = _section[_position++];
            CheckType(r, RecordType.Int);
            checked
            {
                return (int)r.Value;
            }
        }

        public long ReadLong()
        {
            Record r = _section[_position++];
            CheckType(r, RecordType.Long);
            return r.Value;
        }

        public string ReadString()
        {
            Record r = _section[_position++];
            CheckType(r, RecordType.String);
            return r.Text;
        }
    }
}