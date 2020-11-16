using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    static string path = Application.persistentDataPath + "/BrinkOfLight.save";

    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData();

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Saved");
    }

    public static GameData LoadGame()
    {
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = (GameData) formatter.Deserialize(stream);
            stream.Close();
            Debug.Log("Loaded at " + path);
            return data;
        }
        else
        {
            Debug.Log("Save File not Found in " + path);
            return null;
        }
    }

}
