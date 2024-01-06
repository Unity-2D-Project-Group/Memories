using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public int _checkPointID;
    public bool _activated;

    [SerializeField] private LayerMask _playerMask;

    void FixedUpdate()
    {
        //Verify if it collides with the player
        RaycastHit2D boxHit = Physics2D.BoxCast(transform.position, transform.localScale, 0 , Vector2.zero, transform.localScale.x, _playerMask);

        if(boxHit && _activated)
        {
            print("Saved game on checkpoint: " + _checkPointID);

            //Get the current level info
            Level temp = (Level)SaveLoad._savedGame.Levels[$"Level{LoadingData.PlayingLevel}"];
            //Update the current Checkpoint
            temp._currentCheckpoint = _checkPointID;
            //Update the actual info to the new info
            SaveLoad._savedGame.Levels[$"Level{LoadingData.PlayingLevel}"] = temp;
            SaveLoad.OverwriteSave();

            //Take the current checkpoint and unactive all the others that is before it
            TakeCheckpoint();
        }
    }

    public void TakeCheckpoint()
    {
        foreach (Checkpoint check in FindObjectsOfType<Checkpoint>().ToList())
        {
            if (check._checkPointID <= _checkPointID)
            {
                check._activated = false;
                check.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
    }
}
