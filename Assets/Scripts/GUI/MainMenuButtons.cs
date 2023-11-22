using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private Button _loadSaveButton;
    public void Start()
    {
        SaveLoad.LoadSave();
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "GameSave.txt")) || SaveLoad._savedGame._currentLevel == 0)
        {
            _loadSaveButton.interactable = false;
        }
    }

    public void NewGame()
    {
        SaveLoad.NewSave();
    }

    public void LoadGame()
    {
        SaveLoad.LoadSave();
    }
    public void CloseApp()
    {
        Application.Quit();
    }
}
