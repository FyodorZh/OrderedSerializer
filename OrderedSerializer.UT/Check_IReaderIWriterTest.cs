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
                "",
                (byte[]?)null,
                new byte[]{},
                new byte[]{1},
                new byte[]{200,201,202}
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
                else if (obj is Byte byteValue)
                {
                    writer.WriteByte(byteValue);
                }
                else if (obj is Char charValue)
                {
                    writer.WriteChar(charValue);
                }
                else if (obj is short shortValue)
                {
                    writer.WriteShort(shortValue);
                }
                else if (obj is int intValue)
                {
                    writer.WriteInt(intValue);
                }
                else if (obj is long longValue)
                {
                    writer.WriteLong(longValue);
                }
                else if (obj is float floatValue)
                {
                    writer.WriteFloat(floatValue);
                }
                else if (obj is double doubleValue)
                {
                    writer.WriteDouble(doubleValue);
                }
                else if (obj is string stringValue)
                {
                    writer.WriteString(stringValue);
                }
                else if (obj is byte[] arrayValue)
                {
                    writer.WriteBytes(arrayValue);
                }
                else if (obj == null)
                {
                    writer.WriteString(null);
                }
                else
                {
                    Assert.Fail("Unknown type");
                }
            }

            IReader reader = getReader(writer);

            foreach (var obj in objects)
            {
                if (obj is Boolean boolValue)
                {
                    Assert.That(reader.ReadBool(), Is.EqualTo(boolValue));
                }
                else if (obj is Byte byteValue)
                {
                    Assert.That(reader.ReadByte(), Is.EqualTo(byteValue));
                }
                else if (obj is Char charValue)
                {
                    Assert.That(reader.ReadChar(), Is.EqualTo(charValue));
                }
                else if (obj is short shortValue)
                {
                    Assert.That(reader.ReadShort(), Is.EqualTo(shortValue));
                }
                else if (obj is int intValue)
                {
                    Assert.That(reader.ReadInt(), Is.EqualTo(intValue));
                }
                else if (obj is long longValue)
                {
                    Assert.That(reader.ReadLong(), Is.EqualTo(longValue));
                }
                else if (obj is float floatValue)
                {
                    Assert.That(reader.ReadFloat(), Is.EqualTo(floatValue));
                }
                else if (obj is double doubleValue)
                {
                    Assert.That(reader.ReadDouble(), Is.EqualTo(doubleValue));
                }
                else if (obj is string stringValue)
                {
                    Assert.That(reader.ReadString(), Is.EqualTo(stringValue));
                }
                else if (obj is byte[] arrayValue)
                {
                    Assert.That(reader.ReadBytes(), Is.EqualTo(arrayValue));
                }
                else if (obj == null)
                {
                    Assert.That(reader.ReadString(), Is.EqualTo(null));
                }
                else
                {
                    Assert.Fail("Unknown type");
                }
            }
        }
    }
}