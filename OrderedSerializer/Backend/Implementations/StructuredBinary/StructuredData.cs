namespace OrderedSerializer.StructuredBinaryBackend
{
    public class StructuredData
    {
        private Record _root;

        public Record Data => _root;

        public StructuredData(Record root)
        {
            _root = root;
        }
    }
}