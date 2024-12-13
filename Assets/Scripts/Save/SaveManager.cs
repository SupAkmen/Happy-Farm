using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    //Json Save
     //static readonly string FILEPATH = Application.persistentDataPath + "/Save.json";

     //public static void Save(GameSaveState save)
     //{
     //    string json = JsonUtility.ToJson(save);
     //    File.WriteAllText(FILEPATH, json);
     //}
 

    //Binary
    static readonly string FILEPATH = Application.persistentDataPath + "/Save.save";

    //public static void Save(GameSaveState save)
    //{
    //   // Save binary file
    //   using(FileStream file = File.Create(FILEPATH))
    //    {
    //        new BinaryFormatter().Serialize(file, save);
    //    }
    //}


    //public static GameSaveState Load()
    //{
    //    GameSaveState loadSave = null;
    //    //Json

    //    //if (File.Exists(FILEPATH))
    //    //{
    //    //    string json = File.ReadAllText(FILEPATH);
    //    //    loadSave = JsonUtility.FromJson<GameSaveState>(json);
    //    //}


    //    //Binary
    //    if (File.Exists(FILEPATH))
    //    {
    //        using (FileStream file = File.Open(FILEPATH, FileMode.Open))
    //        {
    //            object loadedData = new BinaryFormatter().Deserialize(file);
    //            loadSave = (GameSaveState)loadedData;
    //        }
    //    }

    //    return loadSave;
    //}

    // ktra xem co file ko

    public static void Save(GameSaveState save)
    {
        string json = JsonUtility.ToJson(save);
        File.WriteAllText(FILEPATH, json);
    }

    public static GameSaveState Load()
    {
        if (File.Exists(FILEPATH))
        {
            string json = File.ReadAllText(FILEPATH);
            return JsonUtility.FromJson<GameSaveState>(json);
        }
        return null;
    }

    public static bool HasSave()
    {
        return File.Exists(FILEPATH);
    }

}
