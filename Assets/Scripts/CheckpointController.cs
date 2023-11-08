using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [Header("Components")]
    public GameObject _actualCheckpoint;
    private GameObject _mainCamera;
    void Start()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void TeleportToCheckPoint(GameObject checkpoint)
    {
        transform.position = checkpoint.transform.position;
        StartCoroutine(_mainCamera.GetComponent<CamController>().SendToPlayer(checkpoint.transform.position));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            _actualCheckpoint = collision.gameObject;
            collision.gameObject.SetActive(false);
        }
    }
}
