using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _confirmation;
    [SerializeField] private GameObject _interact;
    [SerializeField] private GameObject _interactText;
    [SerializeField] private LayerMask _interactLayer;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
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

        RaycastHit2D hit = Physics2D.CircleCast(_player.transform.position, 2, Vector2.zero, 2, _interactLayer);

        if (hit)
        {
            _interactText.GetComponent<TMPro.TMP_Text>().text = hit.collider.gameObject.GetComponent<Interact>()._layerText;
            _interact.SetActive(true);
        }
        else
        {
            _interact.SetActive(false);
        }
    }

    public void GoBackToMainMenu(string saveConfirm)
    {
        if(saveConfirm == "Yes")
        {
            SaveLoad.OverwriteSave();
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

    public void ResetPlayer()
    {
        StartCoroutine(_player.GetComponent<PlayerController>().Death());
        Time.timeScale = 1.0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_player.transform.position, 2);
    }
}
