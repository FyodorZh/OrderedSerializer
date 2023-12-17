namespace OrderedSerializer
{
    public interface IDataStruct
    {
        void Serialize(IOrderedSerializer serializer);
    }
}