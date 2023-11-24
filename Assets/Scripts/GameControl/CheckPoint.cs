using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    public int _checkPointID;
    public bool _activated;

    [SerializeField] private LayerMask _playerMask;

    void FixedUpdate()
    {
        RaycastHit2D boxHit = Physics2D.BoxCast(transform.position, transform.localScale, 0 , Vector2.zero, transform.localScale.x, _playerMask);

        if(boxHit && _activated)
        {
            //print(LoadingData.PlayingLevel);
            Level temp = (Level)SaveLoad._savedGame.Levels[$"Level{LoadingData.PlayingLevel}"];
            temp._currentCheckpoint = _checkPointID;
            SaveLoad._savedGame.Levels[$"Level{LoadingData.PlayingLevel}"] = temp;
            SaveLoad.OverwriteSave();

            TakeCheckpoint();
        }
    }

    public void TakeCheckpoint()
    {
        CheckpointController checkpointController = FindObjectOfType<CheckpointController>().GetComponent<CheckpointController>();
        foreach (CheckPoint check in FindObjectsOfType<CheckPoint>().ToList())
        {
            if (check._checkPointID <= _checkPointID)
            {
                check._activated = false;
                check.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
    }
}
