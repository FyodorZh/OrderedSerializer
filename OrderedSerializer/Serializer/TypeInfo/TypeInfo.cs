namespace OrderedSerializer
{
    public static class TypeInfo<T>
    {
        public static bool IsVersioned { get; } = typeof(IVersionedData).IsAssignableFrom(typeof(T));
    }
}