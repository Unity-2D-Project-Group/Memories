using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PetController : MonoBehaviour
{

    private GameObject _player;

    [Header("Variables")]
    [SerializeField] private float _followingMaxSpeed;
    [SerializeField] private float _deaceleration;
    [SerializeField] private Vector3 _offset;

    private Rigidbody2D _rb;
    private ParticleSystem _ps;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _ps = GetComponent<ParticleSystem>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(_player.transform.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(0.4f,0.4f,1);
        }
        else
        {
            transform.localScale = new Vector3(-0.4f, 0.4f, 1);
        }

        if (_rb.velocity.magnitude > 0.5f && !_ps.isPlaying)
        {
            _ps.Play();
        }
        else if( _rb.velocity.magnitude < 0.5f)
        {
            _ps.Stop();
        }
    }

    void FixedUpdate()
    {
        Vector3 direction;
        if (!_player.GetComponent<PlayerController>()._isWallSliding)
        {
            direction = _player.transform.position + new Vector3(_offset.x * -_player.transform.localScale.x, _offset.y, _offset.z) - transform.position;
        }
        else
        {
            direction = _player.transform.position + new Vector3(_offset.x * _player.transform.localScale.x, _offset.y, _offset.z) - transform.position;
        }
        
        float distance = direction.magnitude;

        float actualSpeed = Mathf.Lerp(0, _followingMaxSpeed, distance / _deaceleration);

        Vector3 velocity = direction.normalized * actualSpeed;
        _rb.velocity = velocity;
    }

    public void TeleportPetToPlayer()
    {
        transform.position = _player.transform.position + _offset;
    }
}
