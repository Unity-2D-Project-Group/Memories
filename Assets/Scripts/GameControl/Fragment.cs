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
        //Verify if it collides with the player
        RaycastHit2D boxHit = Physics2D.BoxCast(transform.position, transform.localScale, 0, Vector2.zero, transform.localScale.x, _playerMask);

        if (boxHit && _activated)
        {
            print("Collected Fragment: " + _fragmentID);

            //Get the current level info
            Level temp = (Level)SaveLoad._savedGame.Levels.getNode(LoadingData.PlayingLevel).data;
            //Update the fragment info
            temp._collectedFragments[_fragmentID] = _fragmentID;
            //Update the actual info to the new info
            SaveLoad._savedGame.Levels.getNode(LoadingData.PlayingLevel).data = temp;
            SaveLoad.OverwriteSave();

            _activated = false;
            this.gameObject.SetActive(false);
        }
    }
}
