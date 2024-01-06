using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public static CheckpointController _instance;

    [Header("Components")] 
    private GameObject _player;
    public Hashtable _checkpoints = new Hashtable();

    public void StartCheckPoints()
    {
        //Get the components
        _player = GameObject.FindGameObjectWithTag("Player");


        Save._instance = SaveLoad._savedGame;
        //Get the current level info
        Level temp = (Level)SaveLoad._savedGame.Levels.getNode(LoadingData.PlayingLevel).data;

        //Search for all the Checkpoints on the scene
        foreach (Checkpoint checkPoint in FindObjectsOfType<Checkpoint>().ToList())
        {
            //Save the checkpoints in the hashtable
            _checkpoints.Add(checkPoint._checkPointID.ToString(), checkPoint.gameObject);

            //If the Checkpoint was already collected before, set it to unactive
            if (checkPoint._checkPointID <= temp._currentCheckpoint)
            {
                checkPoint._activated = false;
                checkPoint.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
        TeleportToCheckPoint((Level)SaveLoad._savedGame.Levels.getNode(LoadingData.PlayingLevel).data);
    }

    public void TeleportToCheckPoint(Level level)
    {
        foreach(GameObject checkPoint in _checkpoints.Values) {
            if (level._currentCheckpoint == checkPoint.GetComponent<Checkpoint>()._checkPointID)
            {
                _player.transform.position = checkPoint.transform.position;
            }
        }
    }
}
