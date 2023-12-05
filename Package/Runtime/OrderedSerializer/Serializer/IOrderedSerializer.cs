namespace OrderedSerializer
{
    public interface IOrderedSerializer : IPrimitiveSerializer
    {
        byte Version { get; }

        void AddStruct<T>(ref T value) where T : struct, IDataStruct;
        
        void AddVersionedStruct<T>(ref T value) where T : struct, IDataStruct, IVersionedData;

        /// <summary>
        /// Serialize both versioned and unversioned data
        /// </summary>
        void AddClass<T>(ref T? value) where T : class, IDataStruct;

        void AddAny<T>(ref T value);
    }
}