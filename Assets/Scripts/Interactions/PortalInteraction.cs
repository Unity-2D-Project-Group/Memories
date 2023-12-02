using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalInteraction : Interact
{

    private void Start()
    {
        _layerText = "Enter Portal";
    }
    public override void Interaction()
    {
        //Get the level info
        Level temp = (Level)SaveLoad._savedGame.Levels[$"Level{LoadingData.PlayingLevel}"];
        if (temp._currentFragment >= FindObjectOfType<FragmentController>()._fragments.Count - 1)
        {
            //Finish the level
            SaveLoad._savedGame.FinishLevel(LoadingData.PlayingLevel);
            SaveLoad.OverwriteSave();
            //Calls the main menu scene
            FindAnyObjectByType<SceneLoader>().LoadScene("MainMenuScene");
        }
    }
}
