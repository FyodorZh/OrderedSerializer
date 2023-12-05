namespace OrderedSerializer
{
    public interface IPrimitiveSerializer<T> : IOrderedSerializerCore
    {
        void Add(ref T value);
    }

    public interface IPrimitiveClassSerializer<T> : IPrimitiveSerializer<T?>
        where T : class
    {
    }
    
    public interface IPrimitiveSerializer :
        IPrimitiveSerializer<bool>,
        IPrimitiveSerializer<byte>,
        IPrimitiveSerializer<sbyte>,
        IPrimitiveSerializer<char>,
        IPrimitiveSerializer<short>,
        IPrimitiveSerializer<ushort>,
        IPrimitiveSerializer<int>,
        IPrimitiveSerializer<uint>,
        IPrimitiveSerializer<long>,
        IPrimitiveSerializer<ulong>,
        IPrimitiveSerializer<float>,
        IPrimitiveSerializer<double>,
        IPrimitiveClassSerializer<string>,
        IPrimitiveClassSerializer<byte[]>
    {
    }
}