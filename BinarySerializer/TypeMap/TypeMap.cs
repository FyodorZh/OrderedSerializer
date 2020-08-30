﻿using System;
using System.Collections.Generic;

 namespace BinarySerializer
{
    public class TypeMap : IFactory
    {
        private readonly Dictionary<Type, int> _map = new Dictionary<Type, int>();
        private readonly List<Type> _types = new List<Type>();

        public int GetTypeId(Type type)
        {
            if (!_map.TryGetValue(type, out int id))
            {
                id = _map.Count;
                _map.Add(type, id);
                _types.Add(type);
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

            int count = reader.ReadInt();
            for (int i = 0; i < count; ++i)
            {
                Type type = typeDeserializer.Deserialize(reader);
                _map.Add(type, _types.Count);
                _types.Add(type);
            }
        }

        public object Construct(int typeId)
        {
            return _types[typeId].GetConstructor(new Type[0]).Invoke(new object[0]);
        }
    }
}