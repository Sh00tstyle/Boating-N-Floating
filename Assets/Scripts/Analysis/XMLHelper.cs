using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;

public static class XMLHelper
{
    public static void Serialize<T>(T obj, string filepath, string filename)
    {
        System.IO.TextWriter writer = new StreamWriter(string.Format("{0}\\{1}.xml", filepath, filename), false);
        var serializer = new XmlSerializer(typeof(T));
        serializer.Serialize(writer, obj);
        writer.Close();
    }

    public static T Deserialize<T>(string filepath, string filename)
    {
        var deserializer = new XmlSerializer(typeof(T));
        var reader = new StreamReader(string.Format("{0}\\{1}.xml", filepath, filename));
        var result = (T)deserializer.Deserialize(reader);
        reader.Close();
        return result;
    }
}