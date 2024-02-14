using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void Save(SavedData data)
    {
        string path = Application.persistentDataPath + "/saves/" + data.worldName + "/";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        BinaryFormatter formatter = new();
        FileStream stream = new FileStream(path + "Saved.world", FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SavedData Load(string name)
    {
        string loadPath = Application.persistentDataPath + "/saves/" + name + "/";

        if (Directory.Exists(loadPath))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new FileStream(loadPath + "Saved.world", FileMode.Open);

            SavedData save = formatter.Deserialize(stream) as SavedData;

            return save;
        }
        else
        {
            SavedData save = new SavedData(name);

            return save;
        }
    }
}