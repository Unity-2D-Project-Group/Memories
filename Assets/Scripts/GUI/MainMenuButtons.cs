using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject _loadSaveButton;
    public void Play()
    {
        SaveLoad.LoadSave();
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "GameSave.txt")) || SaveLoad._savedGame._currentLevel == 0)
        {
            _loadSaveButton.SetActive(false);
        }
    }
}
