using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpike : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _raycastSize;
    [SerializeField] private float _fallDelay;
    [SerializeField] private float _dropMultiplier;
    [SerializeField] private float _destroyTimer;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Physics2D.Raycast(transform.position, Vector3.down, _raycastSize, _playerLayer))
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(_fallDelay);
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.gravityScale = _dropMultiplier;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _raycastSize);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.layer == 3)
        {
            Destroy(gameObject, _destroyTimer);
        }
    }
}
