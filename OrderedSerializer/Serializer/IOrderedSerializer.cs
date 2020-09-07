namespace OrderedSerializer
{
    public interface ISerializer
    {
        bool IsWriter { get; }
    }

    public interface IPrimitiveSerializer<T> : ISerializer
    {
        void Add(ref T value);
    }

    public interface IPrimitiveSerializer :
        IPrimitiveSerializer<bool>,
        IPrimitiveSerializer<byte>,
        IPrimitiveSerializer<char>,
        IPrimitiveSerializer<int>,
        IPrimitiveSerializer<long>,
        IPrimitiveSerializer<string>
    {
    }

    public interface IOrderedSerializer : IPrimitiveSerializer
    {
        byte Version { get; }

        void AddStruct<T>(ref T value) where T : struct, IDataStruct;
        void AddVersionedStruct<T>(ref T value) where T : struct, IDataStruct, IVersionedData;
        void AddClass<T>(ref T value) where T : class, IDataStruct;
    }
}