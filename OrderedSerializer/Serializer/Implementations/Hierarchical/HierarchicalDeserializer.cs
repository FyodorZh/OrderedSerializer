using System;
using System.Collections.Generic;

namespace OrderedSerializer
{
    public class HierarchicalDeserializer : PrimitiveDeserializer, IOrderedSerializer
    {
        private readonly ITypeDeserializer _typeDeserializer;

        private readonly List<IConstructor> _typeMap = new List<IConstructor>();

        private readonly Stack<byte> _versions = new Stack<byte>();
        private byte _version;

        public event Action<Exception> OnException;

        public byte Version => _version;

        public HierarchicalDeserializer(IReader reader, ITypeDeserializer typeDeserializer)
            : base(reader)
        {
            _typeDeserializer = typeDeserializer;
            _version = reader.ReadByte();
        }

        public void AddStruct<T>(ref T value)
            where T : struct, IDataStruct
        {
            value.Serialize(this);
        }

        public void AddVersionedStruct<T>(ref T value)
            where T : struct, IDataStruct, IVersionedData
        {
            var version = _reader.ReadByte();
            _versions.Push(_version);
            _version = version;
            value.Serialize(this);
            _version = _versions.Pop();
        }

        public void AddClass<T>(ref T value)
            where T : class, IDataStruct
        {
            int flag = _reader.ReadShort();

            if (flag == 0) // NULL
            {
                value = null;
            }
            else if (flag > 0) // NEW INSTANCE
            {
                int typeId = flag - 1;

                while (typeId >= _typeMap.Count)
                {
                    _typeMap.Add(null);
                }

                IConstructor ctor = _typeMap[typeId];
                if (ctor == null)
                {
                    var type = _typeDeserializer.Deserialize(_reader);
                    ctor = type != null ? TypeConstructorBuilder.Build(type) : NullConstructor.Instance;
                    _typeMap[typeId] = ctor;
                }

                _reader.BeginSection();

                value = DeserializeClass<T>(ctor);

                if (!_reader.EndSection())
                {
                    OnException?.Invoke(new InvalidOperationException());
                }
            }
        }

        protected virtual T DeserializeClass<T>(IConstructor ctor)
            where T : class, IDataStruct
        {
            var value = ctor.Construct() as T;
            if (value != null)
            {
                DeserializeClass(value);
            }

            return value;
        }

        protected void DeserializeClass<T>(T value)
            where T : class, IDataStruct
        {
            bool hasVersion = value is IVersionedData;
            if (hasVersion)
            {
                byte version = _reader.ReadByte();
                _versions.Push(version);
                _version = version;
            }

            try
            {
                value.Serialize(this);
            }
            catch (Exception ex)
            {
                OnException?.Invoke(ex);
            }

            if (hasVersion)
            {
                _version = _versions.Pop();
            }
        }

        protected interface IConstructor
        {
            object Construct();
        }

        private class NullConstructor : IConstructor
        {
            public static readonly IConstructor Instance = new NullConstructor();

            public object Construct()
            {
                return null;
            }
        }

        private static class TypeConstructorBuilder
        {
            private class TypeConstructor<T> : IConstructor
                where T : class, new()
            {
                public object Construct()
                {
                    return new T();
                }
            }

            public static IConstructor Build(Type type)
            {
                Type genericCtor = typeof(TypeConstructor<>);
                Type typeCtor = genericCtor.MakeGenericType(new Type[] {type});

                return (IConstructor)typeCtor.GetConstructor(new Type[0]).Invoke(new object[0]);
            }
        }
    }
}