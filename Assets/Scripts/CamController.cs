using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private float _yOffset;
    [Header("Colliders")]
    [SerializeField] private BoxCollider2D _leftCollider;
    [SerializeField] private BoxCollider2D _rightCollider;
    private GameObject _player;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    
    void FixedUpdate()
    {
        //I HAVE NO IDEA HOW IT WORKS, BUT IT WORKS, SO DONT MESS WITH IT PLSS
        float t = 0;
        float time = 0.25f;
        for (; t < time; t += _smoothVelocity * Time.deltaTime)
        {
            Vector3 position = transform.position;
            position.y = _player.transform.position.y + _yOffset;
            Vector3 boundPosition = new Vector3(transform.position.x, Mathf.Clamp(position.y, _minValues.y, _maxValues.y), transform.position.z);

            transform.position = boundPosition;

            transform.position = Vector3.Lerp(transform.position, boundPosition, t / time);
        }

        if (_leftCollider && _rightCollider)
        {
            if (_rightCollider.IsTouching(_player.GetComponent<CapsuleCollider2D>()))
            {
                StartCoroutine(RealocateCamera("Right"));
            }
            else if (_leftCollider.IsTouching(_player.GetComponent<CapsuleCollider2D>()))
            {
                StartCoroutine(RealocateCamera("Left"));
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
