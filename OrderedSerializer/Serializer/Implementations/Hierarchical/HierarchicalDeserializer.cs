﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace OrderedSerializer
{
    public class HierarchicalDeserializer : PrimitiveDeserializer, IOrderedSerializer
    {
        private readonly ITypeDeserializer _typeDeserializer;

        private readonly List<IConstructor> _typeMap = new List<IConstructor>();

        private readonly Stack<byte> _versions = new Stack<byte>();
        private byte _version;

        public event Action<Exception> OnException;

        private readonly ISerializerExtensionsFactory _factory;

        public byte Version => _version;
        
        public HierarchicalDeserializer(IReader reader, ITypeDeserializer typeDeserializer, ISerializerExtensionsFactory factory = null)
            : base(reader)
        {
            factory ??= SerializerExtensionsFactory.Instance;
            factory.OnError += (type, exception) =>
            {
                OnException?.Invoke(exception);
            };
            _factory = factory;

            _typeDeserializer = typeDeserializer;
            Reset();
        }

        public void Reset()
        {
            _reader.Reset();
            _versions.Clear();
            _typeMap.Clear();
            _version = _reader.ReadByte();
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

                    if (!ctor.IsValid)
                    {
                        OnException?.Invoke(new InvalidOperationException($"Failed to construct '{type}'. It must have private or public default constructor"));
                    }
                }

                _reader.BeginSection();

                value = DeserializeClass<T>(ctor);

                if (!_reader.EndSection())
                {
                    OnException?.Invoke(new InvalidOperationException("Failed to deserialize object section. Skip it"));
                }
            }
        }

        public void AddAny<T>(ref T value)
        {
            var extension = _factory.Construct<T>();
            if (extension == null)
                throw new InvalidOperationException($"{typeof(T)} must be recognizable by extensions factory");
            extension.Add(this, ref value);
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
            bool IsValid { get; }
            object Construct();
        }

        private class NullConstructor : IConstructor
        {
            public static readonly IConstructor Instance = new NullConstructor();

            public bool IsValid => false;

            public object Construct()
            {
                return null;
            }
        }

        private class ReflectionBasedConstructor : IConstructor
        {
            private static readonly object[] VoidObjectList = new object[0];

            private readonly ConstructorInfo _ctorInfo;

            public bool IsValid => _ctorInfo != null;

            public ReflectionBasedConstructor(Type type)
            {
                _ctorInfo = type.GetConstructor(BindingFlags.CreateInstance |
                                                BindingFlags.Instance |
                                                BindingFlags.Public |
                                                BindingFlags.NonPublic,
                    null, new Type[0], null);
            }

            public object Construct()
            {
                return _ctorInfo?.Invoke(VoidObjectList);
            }
        }

        private static class TypeConstructorBuilder
        {
            private class TypeConstructor<T> : IConstructor
                where T : class, new()
            {
                public bool IsValid => true;

                public object Construct()
                {
                    return new T();
                }
            }

            private static readonly Type[] VoidTypeList = new Type[0];

            public static IConstructor Build(Type type)
            {
                if (type.GetConstructor(VoidTypeList) != null)
                {
                    Type genericCtor = typeof(TypeConstructor<>);
                    Type typeCtor = genericCtor.MakeGenericType(new Type[] {type});
                    return (IConstructor)typeCtor.GetConstructor(new Type[0]).Invoke(new object[0]);
                }

                return new ReflectionBasedConstructor(type);
            }
        }
    }
}