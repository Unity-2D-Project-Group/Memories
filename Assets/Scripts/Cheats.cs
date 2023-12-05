using System.Collections;
using System.Collections.Generic;
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
        Level currentLevelInfo = (Level)SaveLoad._savedGame.Levels[$"Level{LoadingData.PlayingLevel}"];
        foreach (GameObject checkPoint in temp._checkpoints.Values)
        {
            if(checkPoint.GetComponent<CheckPoint>()._checkPointID == currentLevelInfo._currentCheckpoint + 1)
            {
                _player.transform.position = checkPoint.transform.position;
            }
        }
        StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamController>().SendToPosition(_player.transform.position));
    }
    private void NextFragment()
    {
        FragmentController temp = FindAnyObjectByType<FragmentController>();
        Level currentLevelInfo = (Level)SaveLoad._savedGame.Levels[$"Level{LoadingData.PlayingLevel}"];
        foreach (GameObject fragment in temp._fragments.Values)
        {
            if (fragment.GetComponent<Fragment>()._fragmentID == currentLevelInfo._currentFragment + 1)
            {
                _player.transform.position = fragment.transform.position;
            }
        }
        StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamController>().SendToPosition(_player.transform.position));
    }
    private void GoToFinal()
    {
        _player.transform.position = FindAnyObjectByType<PortalInteraction>().gameObject.transform.position;

        StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamController>().SendToPosition(_player.transform.position));
    }
}
