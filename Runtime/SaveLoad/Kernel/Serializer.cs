using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CurrencySystem
{
    public class Serializer
    {
        public static T Deserialize<T>(byte[] data) where T : class
        {
            using (var memory = new MemoryStream(data))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(memory) as T;
            }
        }

        public static byte[] Serialize<T>(T data) where T : class
        {
            using (var memory = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memory, data);
                return memory.ToArray();
            }
        }
    }
}
