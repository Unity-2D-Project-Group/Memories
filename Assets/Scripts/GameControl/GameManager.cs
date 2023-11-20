using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, "GameSave.txt")))
        {
            print(Application.persistentDataPath);
            SaveLoad.LoadSave();
        }
        else
        {
            SaveLoad.NewSave();
        }
        this.GetComponent<FragmentController>().StartFragments();
        this.GetComponent<CheckpointController>().StartCheckPoints();
        Time.timeScale = 1.0f;
    }
}
