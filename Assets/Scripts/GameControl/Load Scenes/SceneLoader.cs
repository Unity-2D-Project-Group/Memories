using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void LoadScene(string sceneName)
    {
        LoadingData.SceneToBeLoaded = sceneName;
        LoadingData.SceneToBeUnloaded = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        SaveLoad.NewSave();
        LoadScene("TutorialScene");
    }

    public void LoadGame()
    {
        if ( SaveLoad._savedGame._currentLevel == 0 )
        {
            LoadScene("TutorialScene");
        }
        else
        {
            LoadScene("Level" +  SaveLoad._savedGame._currentLevel + "Scene");
        }
    }
}
