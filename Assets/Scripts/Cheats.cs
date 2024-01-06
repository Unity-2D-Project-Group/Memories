using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class Cheats : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private void Start()
    {
        _player = this.gameObject;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            NextCheckpoint();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            NextFragment();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            GoToFinal();
        }
    }

    private void NextCheckpoint()
    {
        CheckpointController temp = FindAnyObjectByType<CheckpointController>();
        Level currentLevelInfo = (Level)SaveLoad._savedGame.Levels.getNode(LoadingData.PlayingLevel).data;
        foreach (GameObject checkPoint in temp._checkpoints.Values)
        {
            if(checkPoint.GetComponent<Checkpoint>()._checkPointID == currentLevelInfo._currentCheckpoint + 1)
            {
                _player.transform.position = checkPoint.transform.position;
            }
        }
        StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamController>().SendToPosition(_player.transform.position));
    }
    private void NextFragment()
    {
        bool found = false;
        FragmentController temp = FindAnyObjectByType<FragmentController>();
        Level currentLevelInfo = (Level)SaveLoad._savedGame.Levels.getNode(LoadingData.PlayingLevel).data;

        for(int i = 0; i < currentLevelInfo._collectedFragments.Length; i++)
        {
            foreach (GameObject fragmentGameObject in temp._fragments.Values)
            {
                if (fragmentGameObject.GetComponent<Fragment>()._fragmentID == i && currentLevelInfo._collectedFragments[fragmentGameObject.GetComponent<Fragment>()._fragmentID] == null)
                {
                    _player.transform.position = fragmentGameObject.transform.position;
                    found = true;
                }
                if(found) break;
            }
            if (found) break;
        }
        
        StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamController>().SendToPosition(_player.transform.position));
    }
    private void GoToFinal()
    {
        _player.transform.position = FindAnyObjectByType<PortalInteraction>().gameObject.transform.position;

        StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamController>().SendToPosition(_player.transform.position));
    }
}
