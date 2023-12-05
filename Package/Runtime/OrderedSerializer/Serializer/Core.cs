namespace OrderedSerializer
{
    public interface IOrderedSerializerCore
    {
        bool IsWriter { get; }
        
        ILowLevelReader Reader { get; }
        ILowLevelWriter Writer { get; }
        
        IOrderedReader AsReader();
        IOrderedWriter AsWriter();
    }
}