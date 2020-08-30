namespace BinarySerializer
{
    public interface IDataClass
    {
        void Serialize(IBinarySerializer serializer);
    }
}