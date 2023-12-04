using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelGUI : MonoBehaviour
{
    public void GoToMainMenu()
    {
        //Find the scene loader and call the function to load the main menu scene
        FindAnyObjectByType<SceneLoader>().LoadScene("MainMenuScene");
    }
}
