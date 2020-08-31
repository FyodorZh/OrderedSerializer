using System;
using System.Collections.Generic;

 namespace OrderedSerializer
{
    public class TypeMap : IFactory
    {
        private readonly Dictionary<Type, int> _map = new Dictionary<Type, int>();
        private readonly List<Type> _types = new List<Type>();
        private readonly List<IConstructor> _constructors = new List<IConstructor>();

        public int GetTypeId(Type type)
        {
            if (!_map.TryGetValue(type, out int id))
            {
                id = _map.Count;
                _map.Add(type, id);
                _types.Add(type);
                _constructors.Add(ConstructCtor(type));
            }

            return id;
        }

        public void Serialize(IWriter writer, ITypeSerializer typeSerializer)
        {
            int count = _types.Count;
            writer.WriteInt(count);
            foreach (var type in _types)
            {
                typeSerializer.Serialize(writer, type);
            }
        }

        public void Deserialize(IReader reader, ITypeDeserializer typeDeserializer)
        {
            _types.Clear();
            _map.Clear();
            _constructors.Clear();

            int count = reader.ReadInt();
            for (int i = 0; i < count; ++i)
            {
                Type type = typeDeserializer.Deserialize(reader);
                _map.Add(type, _types.Count);
                _types.Add(type);
                _constructors.Add(ConstructCtor(type));
            }
        }

        public object Construct(int typeId)
        {
            return _constructors[typeId].Construct();
        }

        private IConstructor ConstructCtor(Type type)
        {
            Type genericCtor = typeof(TypeCtor<>);
            Type typeCtor = genericCtor.MakeGenericType(new Type[] {type});

            return (IConstructor)typeCtor.GetConstructor(new Type[0]).Invoke(new object[0]);
        }

        private interface IConstructor
        {
            object Construct();
        }

        private class TypeCtor<T> : IConstructor
            where T : class, new()
        {
            public object Construct()
            {
                return new T();
            }
        }
    }
}