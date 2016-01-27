using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class fserial{

    public static BinaryFormatter binaryFormatter = new BinaryFormatter();

    public static string saveasstring(object obj)
    {
        MemoryStream memoryStream = new MemoryStream();
        binaryFormatter.Serialize(memoryStream,obj);
        return System.Convert.ToBase64String(memoryStream.ToArray());
    }
    public static object loadasobj(string astring)
    {
        MemoryStream memoryStream = new MemoryStream(System.Convert.FromBase64String(astring));
        return binaryFormatter.Deserialize(memoryStream);
    }
	
}
