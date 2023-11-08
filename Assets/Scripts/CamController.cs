using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [Header ("Variables")]
    [SerializeField] private float _smoothVelocity = 2f;
    [SerializeField] private Vector3 _minValues;
    [SerializeField] private Vector3 _maxValues;
    [Header("Offsets")]
    [SerializeField] private Vector3 _leftOffset;
    [SerializeField] private Vector3 _rightOffset;
    [SerializeField] private Vector3 _downOffset;
    [Header("Colliders")]
    [SerializeField] private BoxCollider2D _leftCollider;
    [SerializeField] private BoxCollider2D _rightCollider;
    [SerializeField] private BoxCollider2D _upCollider;
    [SerializeField] private BoxCollider2D _downCollider;
    private GameObject _player;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        if (_leftCollider && _rightCollider && _upCollider && _downCollider)
        {
            if (_rightCollider.IsTouching(_player.GetComponent<CapsuleCollider2D>()) || _upCollider.IsTouching(_player.GetComponent<CapsuleCollider2D>()))
            {
                StartCoroutine(RealocateCamera("Right"));
            }
            else if (_leftCollider.IsTouching(_player.GetComponent<CapsuleCollider2D>()))
            {
                StartCoroutine(RealocateCamera("Left"));
            }else if (_downCollider.IsTouching(_player.GetComponent<CapsuleCollider2D>()))
            {
                StartCoroutine(RealocateCamera("Down"));
            }
        }
    }

    public IEnumerator RealocateCamera(string position)
    {
        float t = 0;
        float time = 0.25f;

        switch (position)
        {
            case "Left":
                for (; t < time; t += _smoothVelocity * Time.deltaTime)
                {
                    Vector3 targetPosition = _player.transform.position + _leftOffset;
                    Vector3 boundPosition = new Vector3(
                        Mathf.Clamp(targetPosition.x, _minValues.x, _maxValues.x),
                        Mathf.Clamp(targetPosition.y, _minValues.y, _maxValues.y),
                        Mathf.Clamp(targetPosition.z, _minValues.z, _maxValues.z)
                        );
                    transform.position = Vector3.Lerp(transform.position, boundPosition, t / time);

                    yield return null;
                }
                break;
            case "Right":
                for (; t < time; t += _smoothVelocity * Time.deltaTime)
                {
                    Vector3 targetPosition = _player.transform.position + _rightOffset; 
                    Vector3 boundPosition = new Vector3(
                        Mathf.Clamp(targetPosition.x, _minValues.x, _maxValues.x),
                        Mathf.Clamp(targetPosition.y, _minValues.y, _maxValues.y),
                        Mathf.Clamp(targetPosition.z, _minValues.z, _maxValues.z)
                        );
                    transform.position = Vector3.Lerp(transform.position, boundPosition, t / time);

                    yield return null;
                }
                break;
            case "Down":
                for (; t < time; t += _smoothVelocity * Time.deltaTime)
                {
                    Vector3 targetPosition = _player.transform.position + _downOffset;
                    Vector3 boundPosition = new Vector3(
                        Mathf.Clamp(targetPosition.x, _minValues.x, _maxValues.x),
                        Mathf.Clamp(targetPosition.y, _minValues.y, _maxValues.y),
                        Mathf.Clamp(targetPosition.z, _minValues.z, _maxValues.z)
                        );
                    transform.position = Vector3.Lerp(transform.position, boundPosition, t / time);

                    yield return null;
                }
                break;
        }
    }

    public IEnumerator SendToPlayer(Vector3 position) 
    {
        float t = 0;
        float time = 0.25f;

        for (; t < time; t += _smoothVelocity * Time.deltaTime)
        {
            Vector3 targetPosition = _player.transform.position + _rightOffset;
            Vector3 boundPosition = new Vector3(
                Mathf.Clamp(targetPosition.x, _minValues.x, _maxValues.x),
                Mathf.Clamp(targetPosition.y, _minValues.y, _maxValues.y),
                Mathf.Clamp(targetPosition.z, _minValues.z, _maxValues.z)
                );
            transform.position = Vector3.Lerp(transform.position, boundPosition, t / time);

            yield return null;
        }
    }

}
