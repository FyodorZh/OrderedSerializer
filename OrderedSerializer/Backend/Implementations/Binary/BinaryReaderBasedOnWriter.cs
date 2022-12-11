namespace OrderedSerializer.BinaryBackend
{
    public class BinaryReaderBasedOnWriter : BinaryReader
    {
        private readonly BinaryWriter _writer;
        public BinaryReaderBasedOnWriter(BinaryWriter writer) : base(writer.GetBufferUnsafe(out int size))
        {
            _writer = writer;
        }

        public override void Reset()
        {
            var buffer = _writer.GetBufferUnsafe(out var count);
            Reset(buffer, count);
        }
    }
}