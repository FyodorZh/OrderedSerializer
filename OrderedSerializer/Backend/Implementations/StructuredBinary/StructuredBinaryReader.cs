﻿using System;
using System.Collections.Generic;

namespace OrderedSerializer.StructuredBinaryBackend
{
    public class StructuredBinaryReader : IReader
    {
        private List<Record> _section;
        private readonly Stack<List<Record>> _sections = new Stack<List<Record>>();
        private readonly Stack<int> _positions = new Stack<int>();

        private int _position;

        public StructuredBinaryReader(StructuredData data)
        {
            _section = data.Data.Section;
        }

        public void Reset()
        {
            _position = 0;
            _positions.Clear();
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

        public bool EndSection()
        {
            bool isOk = _position == _section.Count;
            _position = _positions.Pop();
            _section = _sections.Pop();
            return isOk;
        }

        public byte ReadByte()
        {
            Record r = _section[_position++];
            CheckType(r, RecordType.Byte);
            return r.Value.ByteValue;
        }

        public char ReadChar()
        {
            Record r = _section[_position++];
            CheckType(r, RecordType.Char);
            return r.Value.CharValue;
        }

        public short ReadShort()
        {
            Record r = _section[_position++];
            CheckType(r, RecordType.Short);
            return r.Value.ShortValue;
        }

        public int ReadInt()
        {
            Record r = _section[_position++];
            CheckType(r, RecordType.Int);
            return r.Value.IntValue;
        }

        public long ReadLong()
        {
            Record r = _section[_position++];
            CheckType(r, RecordType.Long);
            return r.Value.LongValue;
        }

        public float ReadFloat()
        {
            Record r = _section[_position++];
            CheckType(r, RecordType.Float);
            return r.Value.FloatValue;
        }

        public double ReadDouble()
        {
            Record r = _section[_position++];
            CheckType(r, RecordType.Double);
            return r.Value.DoubleValue;
        }

        public string ReadString()
        {
            Record r = _section[_position++];
            CheckType(r, RecordType.String);
            return r.Text;
        }
    }
}