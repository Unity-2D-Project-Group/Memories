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
    private bool _reallocating = false;

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
    [SerializeField] private float _horMoveSpeed = 1f;
    [SerializeField] private float _maxMouseDistance = 1f;

    private Vector3 _currentCamPos;
    private bool _returnedToPlayer = true;
    private bool _isMoving;
    private Camera _mainCam;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _mainCam = GetComponent<Camera>();
        //Put the colliders in the right position
        _leftCollider.offset = new Vector3(-_mainCam.orthographicSize * 0.54f, 0f);
        _leftCollider.size = new Vector2(0.7f, _mainCam.orthographicSize / 2);
        _rightCollider.offset = new Vector3(_mainCam.orthographicSize * 0.54f, 0f);
        _rightCollider.size = new Vector2(0.7f, _mainCam.orthographicSize / 2);
        StartCoroutine(SendToPosition(_player.transform.position));
    }

    private void Update()
    {
        if (_isMoving)
        {
            _returnedToPlayer = false;
            _currentCamPos = _player.transform.position;
            Vector3 moveDir = Input.mousePosition - new Vector3(Screen.width / 2, 0, 0);
            float moveX = moveDir.x * _horMoveSpeed * Time.fixedDeltaTime;

            Vector3 newPos = transform.position + new Vector3(moveX, 0f, 0f);
            newPos.x = Mathf.Clamp(newPos.x, _currentCamPos.x + _minOffset.x, _currentCamPos.x + _maxOffset.x);
            newPos.y = Mathf.Clamp(newPos.y, _currentCamPos.y + _minOffset.y, _currentCamPos.y + _maxOffset.y);

            newPos.x = Mathf.Clamp(newPos.x, _minValues.x, _maxValues.x);
            newPos.y = Mathf.Clamp(newPos.y, _minValues.y, _maxValues.y);

            transform.position = newPos;
        }
        else if(!_returnedToPlayer){ StartCoroutine(SendToPosition(_player.transform.position)); _returnedToPlayer = true; }
    }

    void LateUpdate()
    {
        if (!_reallocating)
        {
            Vector3 position = transform.position;
            position.y = _player.transform.position.y + _yOffset;
            Vector3 boundPosition = new Vector3(transform.position.x, Mathf.Clamp(position.y, _minValues.y, _maxValues.y), transform.position.z);

            transform.position = boundPosition;

            if (_rightCollider.IsTouching(_player.GetComponent<CapsuleCollider2D>()))
            {
                StartCoroutine(ReallocateCamera("Right"));
            }
            else if (_leftCollider.IsTouching(_player.GetComponent<CapsuleCollider2D>()))
            {
                StartCoroutine(ReallocateCamera("Left"));
            }
        }

        if (Vector2.Distance(new Vector2(Screen.width / 2, 0), new Vector2(Input.mousePosition.x, 0f)) > Screen.width / 2 - _maxMouseDistance)
        {
            _isMoving = true;
        }
        else { _isMoving = false; }
    }

    public IEnumerator ReallocateCamera(string position)
    {
        _reallocating = true;
        _isMoving = false;
        _currentCamPos = Vector3.zero;
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
        _reallocating = false;
    }

    public IEnumerator SendToPosition(Vector3 position) 
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
