using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Security.AccessControl;
using System.Security.Principal;
using System.IO;

public class Serializer
{
    private const string fileName = "required.xml";

    private bool GrantAccess(string fullPath)
    {
        Console.WriteLine(Environment.CurrentDirectory + "\\" + fileName);
        DirectoryInfo dInfo = new DirectoryInfo(Environment.CurrentDirectory + "\\" + fileName);
        DirectorySecurity dSecurity = dInfo.GetAccessControl();
        dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
        dInfo.SetAccessControl(dSecurity);
        return true;
    }

    public void Serialize(object objLst)
    {
        GrantAccess(fileName);
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
        GrantAccess(fileName);
        XmlSerializer serializer = new XmlSerializer(typeof(LList));

        StreamReader reader = new StreamReader(fileName);

        LList openedList = (LList) serializer.Deserialize(reader);
        
        reader.Close();

        return openedList;
    }
}