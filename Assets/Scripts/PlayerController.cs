using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rb;

    [Header("Movement Variables")]
    [SerializeField] private float _movementAcceleration = 50f;
    [SerializeField] private float _maxMoveSpeed = 12f;
    [SerializeField] private float _groundLinearDrag = 10f; //We could call it friction, but we don't bcz that's the way unity calls it, its easier to find it on unity like that
    private float _horizontalDirection;
    private bool _facingRight = true;
    private bool _canMove => (!_isWallSliding);
    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) || (_rb.velocity.x < 0f && _horizontalDirection > 0f);

    [Header("Jump Variables")]
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _airLinearDrag = 5f;
    [SerializeField] private float _fallMultiplier = 8f;
    [SerializeField] private float _lowJumpFallMultiplier = 5f;
    [SerializeField] private int _extraJumps = 1;
    private bool _canJump => (_hangTimeCounter > 0 && !_isWallSliding || _extraJumpsValue > 0 && !_isWallSliding);
    private int _extraJumpsValue = 0;
    private float _hangTime = 1f;
    private float _hangTimeCounter = 0f;
    /*So, long explanation of the reason that we need a hangtime variable, basically, when our player jumps in a corner of a 
     platform, it works normally, but, what if for some reason, the player's computer is slower than ours and gets like, 30 fps,
     the player wouldn't be able to perfect timing the jump in the corner of the platform, so, this hangtime is like a cooldown
     before the player gets unable to do the first jump (even with the double jump mechanic it would be a bad experience for the
     player) so, the player just get unable of jumping when this variable is 0*/

    [Header("Dash Variables")]
    [SerializeField] private float _dashForce = 75f;
    [SerializeField] private float _dashCooldown = 1.5f;
    private float _dashCooldownValue = 0f;
    private bool _canDash => (_dashCooldownValue <= 0);

    [SerializeField] private float _dashLenght = .3f;

    [Header("Collision Variables")]
    [SerializeField] private float _groundRaycastLength;
    [SerializeField] private Vector3 _groundRaycastOffset;
    [SerializeField] private LayerMask _groundLayer;
    private bool _onGround;

    [Header("Bounce Variables")]
    [SerializeField] private float _wallSlidingSpeed = 2f;
    [SerializeField] private float _wallSlidingCheckSize = 0.6f;
    [SerializeField] private Vector2 _wallJumpForce = new Vector2(8f, 16f);
    [SerializeField] private float _wallJumpLength = .3f;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private bool _isWallSliding;
    private bool _canWallJump => (_isWallSliding && !_onGround);
    private bool _isWallJumping;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_dashCooldownValue > 0)
            _dashCooldownValue -= Time.deltaTime;
        _horizontalDirection = GetAxis().x;
        if (Input.GetButtonDown("Jump") && _canJump) { Jump(); } 
        else if(Input.GetButtonDown("Jump") && _canWallJump) { StartCoroutine(WallJump()); }
        if (Input.GetButtonDown("Dash") && _canDash) { StartCoroutine(Dash()); }
    }

    void FixedUpdate()
    {
        MoveCharacter();
        CheckCollisions();
        FallMultiplier();
        WallSlide();
        if(_onGround) 
        {
            _extraJumpsValue = _extraJumps;
            _hangTimeCounter = _hangTime;
            ApplyGroundLinearDrag(); 
        } 
        else
        {
            if( _hangTimeCounter > 0)
                _hangTimeCounter -= Time.deltaTime;
            ApplyAirLinearDrag();
        }
    }
    private void MoveCharacter()
    {
        if (_canMove)
        {
            //Add a force in RB in horizontal direction in the velocity of acceleration that can be changed in the variable
            _rb.AddForce(new Vector2(_horizontalDirection, 0f) * _movementAcceleration);

            //Math.Abs always returns a positive number, thats why we use it here to compare with the max move speed, because if the player is moving backwards it will give a negative number and we dont wanna that
            if (Mathf.Abs(_rb.velocity.x) > _maxMoveSpeed)
            {
                //Math.Sign returns always (-1,0 or 1), so we multiply this by the max move speed then we can have a linear speed 
                _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxMoveSpeed, _rb.velocity.y);
            }
        }

        ChangeFacingDirection();
    }

    private void ChangeFacingDirection()
    {
        //Facing Direction
        if (_rb.velocity.x > 0)
        {
            _facingRight = true;
        }
        else if(_rb.velocity.x < 0) 
        {
            _facingRight = false;
        }

        if (_facingRight)
        {
            if (!_isWallSliding)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            if (!_isWallSliding)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Jump()
    {
        if (!_onGround)
        {
            _extraJumpsValue--;
        }
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        _hangTimeCounter = 0f;
    }

    IEnumerator Dash()
    {
        float dashStartTime = Time.time;
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 0f;
        _rb.drag = 0f;

        Vector2 dir;
        if (_facingRight)
        {
            dir = new Vector2(1f, 0f);
        }
        else
        {
            dir = new Vector2(-1f, 0f);
        }

        while (Time.time < dashStartTime + _dashLenght)
        {
            _rb.velocity = dir.normalized * _dashForce;
            yield return null;
        }
        _dashCooldownValue = _dashCooldown;
    }

    IEnumerator WallJump()
    {
        _isWallJumping = true;
        _isWallSliding = false;
        float jumpStartTime = Time.time;
        float _jumpingDirection = transform.localScale.x;
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 0f;
        _rb.drag = 0f;


        while (Time.time < jumpStartTime + _wallJumpLength)
        {
            _rb.velocity = new Vector2(_jumpingDirection * _wallJumpForce.x, _wallJumpForce.y);
            yield return null;
        }
        _isWallJumping = false;
    }

    private Vector2 GetAxis()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(_horizontalDirection) < 0.4f || _changingDirection)
        {
            _rb.drag = _groundLinearDrag;
        }
        else
        {
            _rb.drag = 0;
        }
    }

    private void ApplyAirLinearDrag()
    {
        _rb.drag = _airLinearDrag;
    }

    private void CheckCollisions()
    {
        //Why do we do like that? Simple, if we do only 1 raycast in the middle of the player, when the player was in a corner of a platform, the raycast wouldn't identify the collision, so, we need 2 raycast, 1 in each side of the player, detecting the ground
        _onGround = Physics2D.Raycast(transform.position + _groundRaycastOffset, Vector2.down, _groundRaycastLength, _groundLayer) ||
                    Physics2D.Raycast(transform.position - _groundRaycastOffset, Vector2.down, _groundRaycastLength, _groundLayer);
        _isWallSliding = Physics2D.Raycast(transform.position, Vector2.right, _wallSlidingCheckSize, _wallLayer) && !_onGround && !_isWallJumping ||
                         Physics2D.Raycast(transform.position, Vector2.left, _wallSlidingCheckSize, _wallLayer) && !_onGround &&!_isWallJumping;
    }

    private void WallSlide()
    {
        if(_isWallSliding)
        {
            _rb.velocity = new Vector2(0f, Mathf.Clamp(_rb.velocity.y, -_wallSlidingSpeed, float.MaxValue));
        }
    }
    private void FallMultiplier()
    {
        if(_rb.velocity.y < 0f) 
        {
            _rb.gravityScale = _fallMultiplier;
        }
        else if (_rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _rb.gravityScale = _lowJumpFallMultiplier;
        }
        else
        {
            _rb.gravityScale = 1;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + _groundRaycastOffset, transform.position + _groundRaycastOffset + Vector3.down * _groundRaycastLength);
        Gizmos.DrawLine(transform.position - _groundRaycastOffset, transform.position - _groundRaycastOffset + Vector3.down * _groundRaycastLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * _wallSlidingCheckSize);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * _wallSlidingCheckSize);
    }
}
