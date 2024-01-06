using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalInteraction : Interact
{
    [SerializeField] private GameObject _feedback;
    private bool _collectedAll;
    private void Start()
    {
        _layerText = "Enter Portal";
    }
    public override void Interaction()
    {
        _collectedAll = true;
        FragmentController tempFrag = FindAnyObjectByType<FragmentController>();
        //Get the level info
        foreach (GameObject fragment in tempFrag._fragments.Values)
        {
            if(fragment.GetComponent<Fragment>()._activated) 
            {
                _collectedAll = false;
            }
        }
        if (_collectedAll)
        {
            //Finish the level
            SaveLoad._savedGame.FinishLevel(LoadingData.PlayingLevel);
            SaveLoad.OverwriteSave();
            //Calls the end level scene
            FindAnyObjectByType<SceneLoader>().LoadScene("EndLevel");
        }
        else
        {
            StartCoroutine(Feedback());
        }
    }

    IEnumerator Feedback()
    {
        _feedback.SetActive(true);
        _feedback.GetComponent<TMP_Text>().text = "Missing Fragments!";
        yield return new WaitForSeconds(1); 
        _feedback.SetActive(false);
    }
}
