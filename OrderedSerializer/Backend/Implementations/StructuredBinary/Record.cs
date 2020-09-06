using System;
using System.Collections.Generic;

namespace OrderedSerializer.StructuredBinaryBackend
{
    public enum RecordType : byte
    {
        Byte,
        Char,
        Short,
        Int,
        Long,
        String,
        Section
    }

    public readonly struct Record
    {
        public readonly RecordType Type;
        public readonly long Value;
        public readonly string Text;
        public readonly List<Record> Section;

        public Record(byte value)
        {
            Type = RecordType.Byte;
            Value = value;
            Text = null;
            Section = null;
        }

        public Record(Char value)
        {
            Type = RecordType.Char;
            Value = value;
            Text = null;
            Section = null;
        }

        public Record(short value)
        {
            Type = RecordType.Short;
            Value = value;
            Text = null;
            Section = null;
        }

        public Record(int value)
        {
            Type = RecordType.Int;
            Value = value;
            Text = null;
            Section = null;
        }

        public Record(long value)
        {
            Type = RecordType.Long;
            Value = value;
            Text = null;
            Section = null;
        }

        public Record(string value)
        {
            Type = RecordType.String;
            Value = 0;
            Text = value;
            Section = null;
        }

        public Record(List<Record> value)
        {
            Type = RecordType.Section;
            Value = 0;
            Text = null;
            Section = value;
        }

        public void WriteTo(IWriter writer)
        {
            writer.WriteByte((byte)Type);
            switch (Type)
            {
                case RecordType.Byte:
                    writer.WriteByte((byte)Value);
                    break;
                case RecordType.Char:
                    writer.WriteChar((char)Value);
                    break;
                case RecordType.Short:
                    writer.WriteShort((short)Value);
                    break;
                case RecordType.Int:
                    writer.WriteInt((int)Value);
                    break;
                case RecordType.Long:
                    writer.WriteLong(Value);
                    break;
                case RecordType.String:
                    writer.WriteString(Text);
                    break;
                case RecordType.Section:
                    {
                        writer.WriteInt(Section.Count);
                        foreach (var element in Section)
                        {
                            element.WriteTo(writer);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static Record ReadFrom(IReader reader)
        {
            RecordType type = (RecordType)reader.ReadByte();
            switch (type)
            {
                case RecordType.Byte:
                    return new Record(reader.ReadByte());
                case RecordType.Char:
                    return new Record(reader.ReadChar());
                case RecordType.Short:
                    return new Record(reader.ReadShort());
                case RecordType.Int:
                    return new Record(reader.ReadInt());
                case RecordType.Long:
                    return new Record(reader.ReadLong());
                case RecordType.String:
                    return new Record(reader.ReadString());
                case RecordType.Section:
                    {
                        int count = reader.ReadInt();
                        List<Record> section = new List<Record>(count);
                        for (int i = 0; i < count; ++i)
                        {
                            Record r = ReadFrom(reader);
                            section.Add(r);
                        }
                        return new Record(section);
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}