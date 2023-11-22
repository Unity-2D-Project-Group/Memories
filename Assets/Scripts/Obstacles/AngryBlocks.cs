using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AngryBlocks : MonoBehaviour
{
    [Header("Angry Block Variables")]
    [SerializeField] private float _gravity;
    [SerializeField] private float _speedToGetUp;
    [SerializeField] private float _maxTimeToGetUp;
    private Rigidbody2D _rb;
    private float _timeToGetUp;
    private Vector3 _initialPosition;
    private bool _canFall;
    private bool _hasFallen;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _canFall = false;
        _hasFallen = false;
        _rb.gravityScale = 0f;

        _initialPosition = transform.position;
        _timeToGetUp = _maxTimeToGetUp;
    }

    private void Update()
    {
        Chronometer();
    }

    private void Chronometer()
    {
        if (_hasFallen) 
        {
            _timeToGetUp -= Time.deltaTime;

            if ( _timeToGetUp <= 0)
            {
                _hasFallen= false;
                _rb.gravityScale= 0f;
                _timeToGetUp = _maxTimeToGetUp;
            }
        }
        else
        {
            if (transform.position != _initialPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, _initialPosition, _speedToGetUp * Time.deltaTime);
                _canFall = false;
            }
            else
            {
                _canFall = true;
            }
        }
    }

    public void ActivateGravity()
    {
        if (_canFall)
        {
            _hasFallen = true;
            _rb.gravityScale = _gravity;
        }
    }
}