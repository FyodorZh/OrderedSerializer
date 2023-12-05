namespace OrderedSerializer
{
    public interface IPrimitiveWriter<T> : IPrimitiveSerializer<T>
    {
        void Write(T value);
    }
    
    public interface IPrimitiveClassWriter<T> : IPrimitiveClassSerializer<T>, IPrimitiveWriter<T?>
        where T : class
    {}

    public interface IPrimitiveWriter :
        IPrimitiveWriter<bool>,
        IPrimitiveWriter<byte>,
        IPrimitiveWriter<sbyte>,
        IPrimitiveWriter<char>,
        IPrimitiveWriter<short>,
        IPrimitiveWriter<ushort>,
        IPrimitiveWriter<int>,
        IPrimitiveWriter<uint>,
        IPrimitiveWriter<long>,
        IPrimitiveWriter<ulong>,
        IPrimitiveWriter<float>,
        IPrimitiveWriter<double>,
        IPrimitiveClassWriter<string>,
        IPrimitiveClassWriter<byte[]>
    {}
    
    public interface IOrderedWriter : IOrderedSerializer, IPrimitiveWriter
    {
    }
}