using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalInteraction : Interact
{
    public int _portalLevel;
    public override void Interaction()
    {
        Level temp = (Level)SaveLoad._savedGame.Levels[$"Level{_portalLevel}"];
        if (temp._currentFragment >= FindObjectOfType<FragmentController>()._fragments.Count - 1)
        {
            SaveLoad._savedGame.FinishLevel(_portalLevel);
            SaveLoad.OverwriteSave();
            //It will be changed
            FindAnyObjectByType<SceneLoader>().LoadScene("MainMenuScene");
        }
    }
}
