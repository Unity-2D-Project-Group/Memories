using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [Header ("Variables")]
    [Range (0f, 2f)]
    [SerializeField] private float _smoothVelocity = 2f;
    [SerializeField] private Vector3 _minValues;
    [SerializeField] private Vector3 _maxValues;
    private bool _realocating = false;

    [Header("Offsets")]
    [SerializeField] private Vector3 _leftOffset;
    [SerializeField] private Vector3 _rightOffset;
    [SerializeField] private float _yOffset;

    [Header("Colliders")]
    [SerializeField] private BoxCollider2D _leftCollider;
    [SerializeField] private BoxCollider2D _rightCollider;
    private GameObject _player;

    [Header("Limits")]
    [SerializeField] private Vector3 _maxOffset;
    [SerializeField] private Vector3 _minOffset;

    [Header("Camera Movement with Mouse")]
    [SerializeField] private float _moveSpeed = 1f;

    private Vector3 _initMousePos;
    private Vector3 _initCamPos;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        //Put the colliders in the right position
        _leftCollider.offset = new Vector3(-Camera.main.orthographicSize * 0.57f, 0f);
        _leftCollider.size = new Vector2(0.5f, Camera.main.orthographicSize / 2);
        _rightCollider.offset = new Vector3(Camera.main.orthographicSize * 0.57f, 0f);
        _rightCollider.size = new Vector2(0.5f, Camera.main.orthographicSize / 2);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _initMousePos = Input.mousePosition;
            _initCamPos = transform.position;
        }
        else if (Input.GetMouseButton(1))
        {
            _realocating = true;
            Vector3 moveDir = Input.mousePosition - _initMousePos;
            float moveX = moveDir.x * _moveSpeed * Time.fixedDeltaTime;
            float moveY = moveDir.y * _moveSpeed * Time.fixedDeltaTime;

            Vector3 newPos = transform.position + new Vector3(moveX, moveY, 0f);
            newPos.x = Mathf.Clamp(newPos.x, _initCamPos.x + _minOffset.x, _initCamPos.x + _maxOffset.x);
            newPos.y = Mathf.Clamp(newPos.y, _initCamPos.y + _minOffset.y, _initCamPos.y + _maxOffset.y);

            transform.position = newPos;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            transform.position = _initCamPos;
            _realocating = false;
        }
    }

    void FixedUpdate()
    {
        


        //I HAVE NO IDEA HOW IT WORKS, BUT IT WORKS, SO DONT MESS WITH IT PLSS
        if (!_realocating)
        {
            float t = 0;
            float time = 0.25f;
            for (; t < time; t += _smoothVelocity * Time.fixedDeltaTime)
            {
                Vector3 position = transform.position;
                position.y = _player.transform.position.y + _yOffset;
                Vector3 boundPosition = new Vector3(transform.position.x, Mathf.Clamp(position.y, _minValues.y, _maxValues.y), transform.position.z);

                transform.position = boundPosition;

                transform.position = Vector3.Lerp(transform.position, boundPosition, t / time);
            }
        }

        //transform.position = new Vector3(transform.position.x, _player.transform.position.y, transform.position.z);

        if (_leftCollider && _rightCollider && !_realocating)
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
        _realocating = true;
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
        _realocating = false;
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
