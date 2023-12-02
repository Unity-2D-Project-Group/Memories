using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpike : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rb;

    [Header("Falling Variables")]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _raycastSize;
    [SerializeField] private float _fallDelay;
    [SerializeField] private float _dropMultiplier;
    [SerializeField] private float _destroyTimer;

    void Start()
    {
        //Get the components
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //If it detects the player, it falls
        if (Physics2D.Raycast(transform.position, Vector3.down, _raycastSize, _playerLayer))
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        //Wait for fall delay
        yield return new WaitForSeconds(_fallDelay);
        //Fall
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.gravityScale = _dropMultiplier;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _raycastSize);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If it collides with the ground or with other obstacles, it gets destroyed
        if(collision.collider.gameObject.layer == 3 || collision.collider.gameObject.layer == 9)
        {
            Destroy(gameObject, _destroyTimer);
        }
    }
}
