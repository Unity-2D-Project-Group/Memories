using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class SaveLoad : MonoBehaviour
{
    public static Save _savedGame;
    public static void OverwriteSave()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Path.Combine(Application.persistentDataPath, "GameSave.txt"));
        bf.Serialize(file, _savedGame);
        file.Close();
    }
    public static void NewSave()
    {
        _savedGame = new Save();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Path.Combine(Application.persistentDataPath, "GameSave.txt"));
        bf.Serialize(file, _savedGame);
        file.Close();
    }

    public static void LoadSave() 
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, "GameSave.txt")))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Path.Combine(Application.persistentDataPath, "GameSave.txt"), FileMode.Open);
            _savedGame = (Save)bf.Deserialize(file);
            file.Close();
        }
    }
}
