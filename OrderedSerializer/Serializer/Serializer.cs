using System.Collections.Generic;

namespace OrderedSerializer
{
    public class Serializer : IOrderedSerializer
    {
        private readonly IWriter _writer;
        private readonly IWriter _typeWriter;

        private readonly TypeMap _typeMap;

        private readonly Dictionary<object, int> _instanceMap = new Dictionary<object, int>(new ReferenceComparer());

        public bool IsWriter => true;

        public int Version { get; }

        public Serializer(IWriter dataWriter, IWriter typeWriter, int version)
        {
            _writer = dataWriter;
            _typeWriter = typeWriter;

            _typeMap = new TypeMap();
            Version = version;
            _writer.WriteInt(version);
        }

        public void Finish(ITypeSerializer typeSerializer)
        {
            _typeMap.Serialize(_typeWriter, typeSerializer);
        }

        public void Add(ref bool value)
        {
            _writer.WriteByte((byte)(value ? 1 : 0));
        }

        public void Add(ref byte value)
        {
            _writer.WriteByte(value);
        }

        public void Add(ref char value)
        {
            _writer.WriteChar(value);
        }

        public void Add(ref int value)
        {
            _writer.WriteInt(value);
        }

        public void Add(ref long value)
        {
            _writer.WriteLong(value);
        }

        public void Add(ref string value)
        {
            _writer.WriteString(value);
        }

        public void AddStruct<T>(ref T value)
            where T : struct, IDataStruct
        {
            value.Serialize(this);
        }

        public void AddClass<T>(ref T value)
            where T : class, IDataClass
        {
            if (value == null)
            {
                _writer.WriteInt(0);
            }
            else
            {
                if (_instanceMap.TryGetValue(value, out int instanceId))
                {
                    instanceId = -(instanceId + 1);
                    _writer.WriteInt(instanceId);
                }
                else
                {
                    instanceId = _instanceMap.Count;
                    _instanceMap.Add(value, instanceId);
                    instanceId = -(instanceId + 1);

                    int typeId = _typeMap.GetTypeId(value.GetType()) + 1;
                    _writer.WriteInt(typeId);
                    _writer.WriteInt(instanceId);

                    _writer.BeginSection();
                    value.Serialize(this);
                    _writer.EndSection();
                }
            }
        }

        private class ReferenceComparer : IEqualityComparer<object>
        {
            bool IEqualityComparer<object>.Equals(object x, object y)
            {
                return ReferenceEquals(x, y);
            }

            int IEqualityComparer<object>.GetHashCode(object obj)
            {
                int hash = obj.GetHashCode();
                return hash;
            }
        }
    }
}