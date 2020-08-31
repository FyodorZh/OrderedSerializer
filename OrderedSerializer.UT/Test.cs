using System.Collections.Generic;
using NUnit.Framework;
using OrderedSerializer.TypeSerializers;

namespace OrderedSerializer
{
    public struct SA : IDataStruct
    {
        public int x;

        public void Serialize(IOrderedSerializer serializer)
        {
            serializer.Add(ref x);
        }
    }

    class CA : IDataClass
    {
        public int y;
        public string hello;
        public CA self;

        public void Serialize(IOrderedSerializer serializer)
        {
            serializer.Add(ref y);
            serializer.Add(ref hello);
            serializer.AddClass(ref self);
        }
    }

    class Root : IDataClass
    {
        public CA a;
        public SA sa;

        public void Serialize(IOrderedSerializer serializer)
        {
            serializer.AddClass(ref a);
            serializer.AddStruct(ref sa);
        }
    }

    [TestFixture]
    public class Test
    {
        [Test]
        public void DoTest()
        {
            Root r0 = new Root();
            r0.sa = new SA();
            r0.sa.x = 3;
            r0.a = new CA();
            r0.a.self = r0.a;
            r0.a.y = 7;
            r0.a.hello = "hello";

            int hc1 = r0.a.GetHashCode();
            int hc2 = r0.a.self.GetHashCode();


            ReflectionBasedTypeSerializer typeSerializer = new ReflectionBasedTypeSerializer();

            byte[] data;
            byte[] typeData;
            {
                var dataWriter = new BinarySource.BinaryWriter();
                var typeWriter = new BinarySource.BinaryWriter();
                Serializer writer = new Serializer(dataWriter, typeWriter, 1);

                writer.AddClass(ref r0);

                writer.Finish(typeSerializer);

                data = dataWriter.GetBuffer();
                typeData = typeWriter.GetBuffer();
            }

            {
                var binaryReader = new BinarySource.BinaryReader(data);

                TypeMap typeMap = new TypeMap();
                typeMap.Deserialize(new BinarySource.BinaryReader(typeData), typeSerializer);

                IOrderedSerializer reader = new Deserializer(binaryReader, typeMap);

                Root r1 = null;
                reader.AddClass(ref r1);
            }

        }

    }
}