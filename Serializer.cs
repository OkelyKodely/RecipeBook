using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class Serializer
{
    private const string fileName = "required.xml";

    public void Serialize(object objLst)
    {
        SerializeCollection(fileName, (LList) objLst);
    }

    public void SerializeCollection<T>(string fullFileName, IEnumerable<T> items)
    {
        var writer = new XmlSerializer(items.GetType());

        var file = new StreamWriter(fullFileName);
        
        writer.Serialize(file, items);
        
        file.Close();
    }

    public LList Deserialize()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(LList));

        StreamReader reader = new StreamReader(fileName);

        LList openedList = (LList) serializer.Deserialize(reader);
        
        reader.Close();

        return openedList;
    }
}