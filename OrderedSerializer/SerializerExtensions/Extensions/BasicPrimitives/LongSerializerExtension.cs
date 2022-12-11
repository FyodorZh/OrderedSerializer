namespace OrderedSerializer
{
    public class LongSerializerExtension : ISerializerExtension<long>
    {
        public void Add(IOrderedSerializer serializer, ref long value)
        {
            serializer.Add(ref value);
        }
    }
}