namespace OrderedSerializer
{
    public interface IDataStruct
    {
        void Serialize(IOrderedSerializer serializer);
    }

    public interface IVersionedDataStruct : IDataStruct, IVersionedData
    {
    }
}