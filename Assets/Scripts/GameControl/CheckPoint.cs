using System.Collections;
using System.Collections.Generic;
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
            SaveLoad._savedGame._currentCheckpoint = _checkPointID;
            SaveLoad.OverwriteSave();
            _activated = false;
            print("Saved the game at: " + _checkPointID);
        }
    }
}
