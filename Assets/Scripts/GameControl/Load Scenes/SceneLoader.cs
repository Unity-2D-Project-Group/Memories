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
        if (FindObjectsOfType<SceneLoader>().Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    public void LoadScene(string sceneName)
    {
        LoadingData.SceneToBeLoaded = sceneName;
        LoadingData.SceneToBeUnloaded = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
    }

    public void LoadLevel(int level)
    {
        if(level == 0)
            LoadingData.SceneToBeLoaded = "TutorialScene";
        else
            LoadingData.SceneToBeLoaded = "Level" + level + "Scene";

        LoadingData.PlayingLevel = level;
        LoadingData.SceneToBeUnloaded = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
    }
}
