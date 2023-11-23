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
        Level temp = (Level)SaveLoad._savedGame.Levels[$"Level{LoadingData.PlayingLevel}"];
        if (temp._currentFragment >= FindObjectOfType<FragmentController>()._fragments.Count - 1)
        {
            SaveLoad._savedGame.FinishLevel(LoadingData.PlayingLevel);
            SaveLoad.OverwriteSave();
            //It will be changed
            FindAnyObjectByType<SceneLoader>().LoadScene("MainMenuScene");
        }
    }
}
