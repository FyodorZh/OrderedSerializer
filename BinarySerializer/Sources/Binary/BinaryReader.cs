using System.Collections.Generic;
using System.Text;

namespace BinarySerializer.BinarySource
{
    public class BinaryReader : IReader
    {
        private readonly byte[] _buffer;

        private readonly Stack<int> _stackOfSections = new Stack<int>();

        private int _position = 0;

        public BinaryReader(byte[] buffer)
        {
            _buffer = buffer;
        }

        public void BeginSection()
        {
            int size = ReadInt();
            _stackOfSections.Push(_position + size);
        }

        public void EndSection()
        {
            _position = _stackOfSections.Pop();
        }

        public byte ReadByte()
        {
            return _buffer[_position++];
        }

        public char ReadChar()
        {
            CharToByte block = new CharToByte
            {
                Byte0 = _buffer[_position++],
                Byte1 = _buffer[_position++]
            };

            return block.Value;
        }

        public int ReadInt()
        {
            IntToByte block = new IntToByte
            {
                Byte0 = _buffer[_position++],
                Byte1 = _buffer[_position++],
                Byte2 = _buffer[_position++],
                Byte3 = _buffer[_position++]
            };

            return block.Value;
        }

        public long ReadLong()
        {
            LongToByte block = new LongToByte()
            {
                Byte0 = _buffer[_position++],
                Byte1 = _buffer[_position++],
                Byte2 = _buffer[_position++],
                Byte3 = _buffer[_position++],
                Byte4 = _buffer[_position++],
                Byte5 = _buffer[_position++],
                Byte6 = _buffer[_position++],
                Byte7 = _buffer[_position++]
            };

            return block.Value;
        }

        public string ReadString()
        {
            int count = ReadInt();
            if (count == 0)
            {
                return null;
            }

            count -= 1;

            string value = Encoding.UTF8.GetString(_buffer, _position, count);
            _position += count;
            return value;
        }
    }
}