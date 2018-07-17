using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace _24_Serialization
{
    class QuickStart
    {
        static void Main(string[] args)
        {
            //创建对象图以便把它们序列化到流中
            var objectGraph = new List<String> { "Jeff", "Kristin", "Aidan", "Grant" };
            Stream stream = SerializeToMemory(objectGraph);

            stream.Position = 0;
            objectGraph = null;

            objectGraph = (List<string>)DeserializeFromMemory(stream);
            foreach (var s in objectGraph) Console.WriteLine(s);
        }

        private static MemoryStream SerializeToMemory(Object objectGraph)
        {
            //构造流来容纳序列化的对象
            MemoryStream stream = new MemoryStream();
            
            BinaryFormatter formatter = new BinaryFormatter();
            //告诉格式化器将对象序列化到流中
            //对流对象的引用（标识了序列化好的字节应放在哪里），想要系列化的对象图的引用
            formatter.Serialize(stream, objectGraph);

            return stream;
        }

        private static Object DeserializeFromMemory(Stream stream)
        {
            //构造序列化器来做所有真正的工作
            BinaryFormatter formatter = new BinaryFormatter();

            //告诉格式化器从流中反序列化对象
            return formatter.Deserialize(stream);
        }
    }
}
