using System;
using System.Collections.Generic;
using NUnit.Framework;
using OrderedSerializer.BinaryBackend;
using OrderedSerializer.JsonBackend;
using OrderedSerializer.StructuredBinaryBackend;

namespace OrderedSerializer.UT
{
    [TestFixture]
    public class Check_IReaderIWriterTest
    {
        [Test]
        public void Test_Binary()
        {
            Check(GetList(), () => new BinaryWriter(), w =>
            {
                BinaryWriter bw = (BinaryWriter)w;
                var buffer = bw.GetBuffer();
                return new BinaryReader(buffer);
            });
        }

        [Test]
        public void Test_StructuredBinary()
        {
            Check(GetList(), () => new StructuredBinaryWriter(), w =>
            {
                StructuredBinaryWriter bw = (StructuredBinaryWriter)w;
                return new StructuredBinaryReader(bw.ExtractData());
            });
        }
        
        [Test]
        public void Test_Json()
        {
            Check(GetList(), () => new JsonWriter(), w =>
            {
                JsonWriter writer = (JsonWriter)w;
                return new JsonReader(writer.ToJsonString());
            });
        }

        private IReadOnlyCollection<object?> GetList()
        {
            return new object?[]
            {
                true,
                false,
                (byte)0,
                (byte)255,
                'x',
                'z',
                '\0',
                (short)400,
                (short)-1,
                1,
                -7,
                -100000,
                -1L,
                10000000000000L,
                0.25f,
                0.5,
                "hello",
                "world",
                null,
                ""
            };
        }

        private void Check(IReadOnlyCollection<object?> objects, Func<IWriter> getWriter, Func<IWriter, IReader> getReader)
        {
            IWriter writer = getWriter();

            foreach (var obj in objects)
            {
                if (obj is Boolean boolValue)
                {
                    writer.WriteBool(boolValue);
                }
                if (obj is Byte byteValue)
                {
                    writer.WriteByte(byteValue);
                }
                if (obj is Char charValue)
                {
                    writer.WriteChar(charValue);
                }
                if (obj is short shortValue)
                {
                    writer.WriteShort(shortValue);
                }
                if (obj is int intValue)
                {
                    writer.WriteInt(intValue);
                }
                if (obj is long longValue)
                {
                    writer.WriteLong(longValue);
                }
                if (obj is float floatValue)
                {
                    writer.WriteFloat(floatValue);
                }
                if (obj is double doubleValue)
                {
                    writer.WriteDouble(doubleValue);
                }
                if (obj is string stringValue)
                {
                    writer.WriteString(stringValue);
                }
                if (obj == null)
                {
                    writer.WriteString(null);
                }
            }

            IReader reader = getReader(writer);

            foreach (var obj in objects)
            {
                if (obj is Boolean boolValue)
                {
                    Assert.AreEqual(boolValue, reader.ReadBool());
                }
                if (obj is Byte byteValue)
                {
                    Assert.AreEqual(byteValue, reader.ReadByte());
                }
                else if (obj is Char charValue)
                {
                    Assert.AreEqual(charValue, reader.ReadChar());
                }
                else if (obj is short shortValue)
                {
                    Assert.AreEqual(shortValue, reader.ReadShort());
                }
                else if (obj is int intValue)
                {
                    Assert.AreEqual(intValue, reader.ReadInt());
                }
                else if (obj is long longValue)
                {
                    Assert.AreEqual(longValue, reader.ReadLong());
                }
                else if (obj is float floatValue)
                {
                    Assert.AreEqual(floatValue, reader.ReadFloat());
                }
                else if (obj is double doubleValue)
                {
                    Assert.AreEqual(doubleValue, reader.ReadDouble());
                }
                else if (obj is string stringValue)
                {
                    Assert.AreEqual(stringValue, reader.ReadString());
                }
                else if (obj == null)
                {
                    Assert.AreEqual(null, reader.ReadString());
                }
            }
        }
    }
}