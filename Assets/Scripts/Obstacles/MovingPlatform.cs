using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlat : MonoBehaviour
{
    public Transform _platform;
    public Transform _startPoint;
    public Transform _endPoint;
    public float _speed = 1.5f;
    int _direction = 1;

    private void Update()
    {
        Vector2 target = CurrentMovementTarget();
        _platform.position = Vector2.Lerp(_platform.position, target, _speed * Time.deltaTime);

        float distance = (target - (Vector2)_platform.position).magnitude;

        if (distance <= 0.1f)
        {
            _direction *= -1;
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
    private void OnDrawGizmos()
    {
        if(_endPoint && _startPoint)
        {
            Gizmos.DrawLine(_startPoint.position, _endPoint.position);
        }
    }
}
