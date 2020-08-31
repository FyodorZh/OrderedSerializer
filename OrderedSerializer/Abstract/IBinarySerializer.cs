namespace OrderedSerializer
{
    public interface ISerializer
    {
        bool IsWriter { get; }

        int Version { get; }
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
        void AddStruct<T>(ref T value) where T : struct, IDataStruct;
        void AddClass<T>(ref T value) where T : class, IDataClass;
    }
}