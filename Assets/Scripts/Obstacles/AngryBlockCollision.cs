using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AngryBlockCollision : MonoBehaviour
{
    private GameObject _player;
    private BoxCollider2D _collider;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        if (_collider.IsTouching(_player.GetComponent<CapsuleCollider2D>()))
        {
            StartCoroutine(_player.gameObject.GetComponent<PlayerController>().Death());
        }
    }
}