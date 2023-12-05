namespace OrderedSerializer
{
    public interface IPrimitiveReader<T> : IPrimitiveSerializer<T>
    {
        void Read(out T value);
    }
    
    public interface IPrimitiveClassReader<T> : IPrimitiveClassSerializer<T>, IPrimitiveReader<T?>
        where T : class
    {}
    
    public interface IPrimitiveReader :
        IPrimitiveReader<bool>,
        IPrimitiveReader<byte>,
        IPrimitiveReader<sbyte>,
        IPrimitiveReader<char>,
        IPrimitiveReader<short>,
        IPrimitiveReader<ushort>,
        IPrimitiveReader<int>,
        IPrimitiveReader<uint>,
        IPrimitiveReader<long>,
        IPrimitiveReader<ulong>,
        IPrimitiveReader<float>,
        IPrimitiveReader<double>,
        IPrimitiveClassReader<string>,
        IPrimitiveClassReader<byte[]>
    {}

    public interface IOrderedReader : IOrderedSerializer, IPrimitiveReader
    {
        // void ReadStruct<T>(out T value) where T : struct, IDataStruct;
        //
        // void ReadVersionedStruct<T>(out T value) where T : struct, IDataStruct, IVersionedData;
        //
        // /// <summary>
        // /// Serialize both versioned and unversioned data
        // /// </summary>
        // void ReadClass<T>(out T? value) where T : class, IDataStruct;
        //
        // void ReadAny<T>(out T value);
    }
}