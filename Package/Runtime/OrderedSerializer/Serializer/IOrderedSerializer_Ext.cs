using System.Collections.Generic;

namespace OrderedSerializer
{
    public static class IOrderedSerializer_Ext
    {
        public static void Add<T>(this IPrimitiveSerializer<T?> serializer, ref T?[]? array)
        {
            if (serializer.IsWriter)
            {
                if (array == null)
                {
                    serializer.Writer.WriteInt(0);
                }
                else
                {
                    int count = array.Length;
                    serializer.Writer.WriteInt(count + 1);
                    for (int i = 0; i < count; ++i)
                    {
                        serializer.Add(ref array[i]);
                    }
                }
            }
            else
            {
                int count = serializer.Reader.ReadInt();
                if (count <= 0)
                {
                    array = null;
                    return;
                }

                count -= 1;
                array = new T[count];
                for (int i = 0; i < count; ++i)
                {
                    serializer.Add(ref array[i]);
                }
            }
        }
        
        public static void Add<T>(this IPrimitiveSerializer<T?> serializer, ref List<T?>? list)
        {
            if (serializer.IsWriter)
            {
                if (list == null)
                {
                    serializer.Writer.WriteInt(0);
                }
                else
                {
                    int count = list.Count;
                    serializer.Writer.WriteInt(count + 1);
                    for (int i = 0; i < count; ++i)
                    {
                        var element = list[i];
                        serializer.Add(ref element);
                    }
                }
            }
            else
            {
                int count = serializer.Reader.ReadInt();
                if (count <= 0)
                {
                    list = null;
                    return;
                }

                count -= 1;
                list = new List<T?>(count);
                for (int i = 0; i < count; ++i)
                {
                    var element = default(T);
                    serializer.Add(ref element);
                    list.Add(element);
                }
            }
        }
    }
}