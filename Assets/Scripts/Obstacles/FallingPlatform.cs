using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

    private Rigidbody2D rb;

    [SerializeField] private float _fallDelay = 1f;
    [SerializeField] private float _destroyDelay = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }
    
    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(_fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, _destroyDelay);
    }
}
