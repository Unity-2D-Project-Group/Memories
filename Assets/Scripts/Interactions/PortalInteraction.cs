using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalInteraction : Interact
{
    public override void Interaction()
    {
        if(SaveLoad._savedGame._currentFragment >= FragmentController._fragments.Count - 1)
        {
            SaveLoad._savedGame._currentLevel++;
            SaveLoad._savedGame._currentFragment = 0;
            SaveLoad.OverwriteSave();
            //It will be changed
            FindAnyObjectByType<SceneLoader>().LoadScene("MainMenuScene");
        }
    }
}
