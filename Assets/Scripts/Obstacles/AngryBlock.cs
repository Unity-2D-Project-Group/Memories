using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AngryBlock : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rb;

    [Header("Falling/Getting Up Variables")]
    [SerializeField] private float _gravity;
    [SerializeField] private float _speedToGetUp;
    [SerializeField] private float _maxTimeToGetUp;

    private float _timeToGetUp;
    private Vector3 _initialPosition;
    private bool _canFall;
    private bool _hasFallen;
    private void Start()
    {
        //Get the components
        _rb = GetComponent<Rigidbody2D>();

        //Reset all the variables
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
            //Decrease the time to get up
            _timeToGetUp -= Time.deltaTime;

            if ( _timeToGetUp <= 0)
            {
                //Reset the variables again
                _hasFallen = false;
                _rb.gravityScale = 0f;
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