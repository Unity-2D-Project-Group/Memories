using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _confirmation;
    [SerializeField] private GameObject _resetConfirmation;
    [SerializeField] private GameObject _interact;
    [SerializeField] private GameObject _interactText;
    [SerializeField] private LayerMask _interactLayer;
    void Start()
    {
        //Get the components
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        //Switches between the active pause menu and unactive pause menu with the "Esc" keybind
        if (Input.GetKeyDown(KeyCode.Escape) && _pauseMenu.activeSelf == false)
        {
            _pauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _pauseMenu.activeSelf == true)
        {
            _pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
        }

        //Verify if the player is close to an interactable object
        RaycastHit2D hit = Physics2D.CircleCast(_player.transform.position, 2, Vector2.zero, 2, _interactLayer);
        //If it is, set active the iteract GUI, and set the text to the text set on the object
        if (hit)
        {
            _interactText.GetComponent<TMPro.TMP_Text>().text = hit.collider.gameObject.GetComponent<Interact>()._layerText;
            _interact.SetActive(true);
        }
        //If the player is not, just unactive the interact GUI
        else
        {
            _interact.SetActive(false);
        }
    }

    public void GoBackToMainMenu(string saveConfirm)
    {
        if(saveConfirm == "Yes")
        {
            FindAnyObjectByType<SceneLoader>().LoadScene("MainMenuScene");
            Time.timeScale = 1.0f;
        }
        else
        {
            _confirmation.SetActive(false);
        }
    }

    public void Continue()
    {
        Time.timeScale = 1.0f;
    }

    public void ResetPlayer(string saveConfirm)
    {
        if (saveConfirm == "Yes")
        {
            //Get the current level info
            Level temp = (Level)SaveLoad._savedGame.Levels[$"Level{LoadingData.PlayingLevel}"];
            //Update the current Checkpoint
            temp._currentCheckpoint = 0;
            temp._currentFragment = -1;
            //Update the actual info to the new info
            SaveLoad._savedGame.Levels[$"Level{LoadingData.PlayingLevel}"] = temp;
            SaveLoad.OverwriteSave();

            StartCoroutine(_player.GetComponent<PlayerController>().Death());
        }
        else
        {
            _resetConfirmation.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_player.transform.position, 2);
    }
}
