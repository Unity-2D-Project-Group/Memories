using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Spikes : MonoBehaviour
{
    private GameObject _player;
    private PolygonCollider2D _collider;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _collider = GetComponent<PolygonCollider2D>();
    }

    void FixedUpdate()
    {
        if (_collider.IsTouching(_player.GetComponent<CapsuleCollider2D>()))
        {
            StartCoroutine(_player.gameObject.GetComponent<PlayerController>().Death());
        }
    }
}