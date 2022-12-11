using System;
using System.Collections.Generic;
using NUnit.Framework;
using OrderedSerializer.BinaryBackend;
using OrderedSerializer.StructuredBinaryBackend;

namespace OrderedSerializer
{
    [TestFixture]
    public class IReaderIWriterTest
    {
        [Test]
        public void BinaryTest()
        {
            Check(GetList(), () => new BinaryWriter(), w =>
            {
                BinaryWriter bw = (BinaryWriter)w;
                var buffer = bw.GetBuffer();
                return new BinaryReader(buffer);
            });
        }

        [Test]
        public void StructuredBinaryTest()
        {
            Check(GetList(), () => new StructuredBinaryWriter(), w =>
            {
                StructuredBinaryWriter bw = (StructuredBinaryWriter)w;
                return new StructuredBinaryReader(bw.ExtractData());
            });
        }

        private IReadOnlyCollection<object> GetList()
        {
            return new object[]
            {
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
                "hello",
                "world",
                "",
                null
            };
        }

        private void Check(IReadOnlyCollection<object> objects, Func<IWriter> getWriter, Func<IWriter, IReader> getReader)
        {
            IWriter writer = getWriter();

            foreach (var obj in objects)
            {
                if (obj is Byte)
                {
                    writer.WriteByte((Byte)obj);
                }
                if (obj is Char)
                {
                    writer.WriteChar((Char)obj);
                }
                if (obj is short)
                {
                    writer.WriteShort((short)obj);
                }
                if (obj is int)
                {
                    writer.WriteInt((int)obj);
                }
                if (obj is long)
                {
                    writer.WriteLong((long)obj);
                }
                if (obj is string)
                {
                    writer.WriteString((string)obj);
                }
                if (obj == null)
                {
                    writer.WriteString(null);
                }
            }

            IReader reader = getReader(writer);

            foreach (var obj in objects)
            {
                if (obj is Byte)
                {
                    Assert.AreEqual((Byte)obj, reader.ReadByte());
                }
                else if (obj is Char)
                {
                    Assert.AreEqual((Char)obj, reader.ReadChar());
                }
                else if (obj is short)
                {
                    Assert.AreEqual((short)obj, reader.ReadShort());
                }
                else if (obj is int)
                {
                    Assert.AreEqual((int)obj, reader.ReadInt());
                }
                else if (obj is long)
                {
                    Assert.AreEqual((long)obj, reader.ReadLong());
                }
                else if (obj is string)
                {
                    Assert.AreEqual((string)obj, reader.ReadString());
                }
                else if (obj == null)
                {
                    Assert.AreEqual(null, reader.ReadString());
                }
            }
        }
    }
}