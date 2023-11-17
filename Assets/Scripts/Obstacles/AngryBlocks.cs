using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AngryBlocks : MonoBehaviour
{
    private Rigidbody2D rb;
    private float _distance;
    private int _direction = 1;

    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _speed = 1.5f;

    [SerializeField] private Transform _angryblock;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _distance = Vector2.Distance(_endPoint.position, _startPoint.position);
    }

    private void Update()
    {
        if (Physics2D.Raycast(_angryblock.transform.position, Vector2.down, _distance, _playerLayer))
        {
            StartCoroutine(Attack());
        }
    }

    Vector2 CurrentMovementTarget()
    {
        if (_direction == 1)
        {
            return _startPoint.position;
        }
        else
        {
            return _endPoint.position;
        }
    }

    private IEnumerator Attack()
    {
        yield return null;
        //yield return new WaitForSeconds(_fallDelay);
        //rb.bodyType = RigidbodyType2D.Dynamic;

        Vector2 target = CurrentMovementTarget();
        _angryblock.position = Vector2.Lerp(_angryblock.position, target, _speed * Time.deltaTime);

        float distance = (target - (Vector2)_angryblock.position).magnitude;

        if (distance <= 0.1f)
        {
            _direction *= -1;
        }
    }

    private void OnDrawGizmos()
    {
        if (_endPoint && _startPoint)
        {
            Gizmos.DrawLine(_startPoint.position, _endPoint.position);
        }

        Gizmos.DrawLine(_angryblock.transform.position, _angryblock.transform.position + Vector3.down * _distance);
    }
}