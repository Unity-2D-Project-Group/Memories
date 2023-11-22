using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fragment : MonoBehaviour
{
    public int _fragmentID;
    public bool _activated;

    [SerializeField] private LayerMask _playerMask;

    void FixedUpdate()
    {
        RaycastHit2D boxHit = Physics2D.BoxCast(transform.position, transform.localScale, 0, Vector2.zero, transform.localScale.x, _playerMask);

        if (boxHit && _activated)
        {
            Level temp = (Level)SaveLoad._savedGame.Levels[$"Level{SaveLoad._savedGame._currentLevel}"];
            temp._currentFragment = _fragmentID;
            SaveLoad._savedGame.Levels[$"Level{SaveLoad._savedGame._currentLevel}"] = temp;
            SaveLoad.OverwriteSave();
            //print("Collected Fragment: " + _fragmentID);
            _activated = false;
            this.gameObject.SetActive(false);
        }
    }
}
