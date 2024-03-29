﻿using System.Collections.Generic;

namespace OrderedSerializer
{
    public class QueueSerializerExtension<T> : ISerializerExtension<Queue<T>?>
    {
        private readonly ISerializerExtension<T> _elementSerializer;

        public QueueSerializerExtension(ISerializerExtension<T> elementSerializer)
        {
            _elementSerializer = elementSerializer;
        }

        public void Add(IOrderedSerializer serializer, ref Queue<T>? value)
        {
            if (serializer.IsWriter)
            {
                if (value == null)
                {
                    serializer.Writer.WriteInt(0);
                }
                else
                {
                    serializer.Writer.WriteInt(value.Count + 1);
                    foreach (var element in value)
                    {
                        var tmp = element;
                        _elementSerializer.Add(serializer, ref tmp);
                    }
                }
            }
            else
            {
                int count = serializer.Reader.ReadInt();
                if (count == 0)
                {
                    value = null;
                }
                else
                {
                    count -= 1;
                    value = new Queue<T>(count);
                    for (int i = 0; i < count; ++i)
                    {
                        T element = default!;
                        _elementSerializer.Add(serializer, ref element);
                        value.Enqueue(element);
                    }
                }
            }
        }
    }
}