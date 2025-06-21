using System.IO;
using UnityEngine;

public class FileDiskManager : MonoBehaviour
{
   public void WriteToDisk(string name, string content)
    {
        string path = Application.persistentDataPath + name;
        File.WriteAllText(path, content);
    }

    public string ReadFromDisk(string name)
    {
        string path = Application.persistentDataPath + name;
        return File.ReadAllText(path);
    }
}
