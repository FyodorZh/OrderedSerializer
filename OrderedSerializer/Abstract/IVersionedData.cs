namespace OrderedSerializer
{
    public interface IVersionedData
    {
        byte Version { get; }
    }
}