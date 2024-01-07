using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform _platform;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private float _speed = 1.5f;
    int _direction = 1;

    private void FixedUpdate()
    {
        Vector2 target = CurrentMovementTarget();
        //Move the platform
        _platform.position = Vector3.MoveTowards(_platform.position, target, _speed);

        //Calculate the distance of the target
        float distance = (target - (Vector2)_platform.position).magnitude;

        //If it gets close, change the direction
        if (distance <= 0.1f)
        {
            _direction *= -1;
        }
    }

    Vector2 CurrentMovementTarget()
    {
        //Changes the target accoring to the direction
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
