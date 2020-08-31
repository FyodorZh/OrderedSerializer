namespace OrderedSerializer
{
    public interface IDataClass
    {
        void Serialize(IOrderedSerializer serializer);
    }
}