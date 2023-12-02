using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [Header("Components")]
    private GameObject _player;

    void Start()
    {
        //Get the components
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Player" && _player.gameObject.GetComponent<PlayerController>()._isDead == false)
        {
            StartCoroutine(_player.gameObject.GetComponent<PlayerController>().Death());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && _player.gameObject.GetComponent<PlayerController>()._isDead == false)
        {
            StartCoroutine(_player.gameObject.GetComponent<PlayerController>().Death());
        }
    }
}