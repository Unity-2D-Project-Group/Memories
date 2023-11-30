using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rb;

    [Header("Public Variables")]
    [HideInInspector] public bool _facingRight = true;
    [HideInInspector] public bool _onGround;
    [HideInInspector] public bool _isHooking;
    [HideInInspector] public bool _isRegretting = false;
    [HideInInspector] public bool _isDashing = false;
    [HideInInspector] public bool _canInteract = false;
    [HideInInspector] public bool _isWallSliding;
    [HideInInspector] public bool _isDead = false;

    [Header("Movement Variables")]
    [SerializeField] private float _movementAcceleration = 50f;
    [SerializeField] private float _maxMoveSpeed = 12f;
    [SerializeField] private float _maxYSpeed = 30f;
    [SerializeField] private float _groundLinearDrag = 10f; //We could call it friction, but we don't bcz that's the way unity calls it, its easier to find it on unity like that
    private float _horizontalDirection;
    private bool _canMove => (!_isWallSliding && !_isHooking && !_isRegretting && !_isDashing && !_isWallJumping);
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
    [SerializeField] private float _dashAmount = 1;
    [SerializeField] private float _dashAmountValue;
    private float _dashCooldownValue = 0f;
    private bool _canDash => (_dashCooldownValue <= 0 /*&& !_isGliding*/ && _dashAmountValue > 0);

    [SerializeField] private float _dashLength = .3f;

    [Header("Collision Variables")]
    [SerializeField] private float _groundRaycastLength;
    [SerializeField] private Vector3 _groundRaycastOffset;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Wall Jump/ Wall Sliding Variables")]
    [SerializeField] private float _wallSlidingSpeed = 2f;
    [SerializeField] private float _wallSlidingCheckSize = 0.6f;
    [SerializeField] private Vector2 _wallJumpForce = new Vector2(8f, 16f);
    [SerializeField] private float _wallJumpLength = .3f;
    [SerializeField] private LayerMask _wallLayer;
    private bool _canWallJump => (_isWallSliding && !_onGround);
    private bool _isWallJumping; 
    [SerializeField] private float _wallDirection = 0;

    [Header("Hook Variables")]
    [SerializeField] private LayerMask _hookableMask;
    [SerializeField] private float _hookThreshold = 7f;
    [SerializeField] private float _hookSpeed = 10f;
    [SerializeField] private float _objSpeed = 10f;
    [SerializeField] private float _hookCooldown = 1.5f;
    private float _hookCooldownValue;
    private Vector2 _target;
    private GameObject _targetGameObject;
    private LineRenderer _lineRenderer;
    private bool _canHook => (!_isWallJumping && !_isWallSliding && _hookCooldownValue <= 0);

    [Header("Interaction Variables")]
    [SerializeField] private float _interactRadius = 2f;
    [SerializeField] private LayerMask _interactLayer;

    // Higher numbers for more mouse movement on joystick press. Warning: diagonal movement lost at lower sensitivity (<1000)
    public Vector2 _sensitivity = new Vector2(1500000f, 1500000f);
    // Counteract tendency for cursor to move more easily in some directions
    public Vector2 _bias = new Vector2(0f, -1f);

    // Cached variables
    Vector3 _leftStick;
    Vector2 _mousePosition;
    Vector2 _warpPosition;

    // Stored for next frame
    Vector2 _overflow;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        _dashAmountValue = _dashAmount;

        GameObject.FindGameObjectWithTag("Pet").GetComponent<PetController>().TeleportPetToPlayer();
    }

    void Update()
    {
        //Functions
        CheckCollisions();
        ChangeFacingDirection();
        FallMultiplier();

        //Decrease the cooldown of dash
        if (_dashCooldownValue > 0)
            _dashCooldownValue -= Time.deltaTime;
        //Decrease the cooldown of hook
        if (_hookCooldownValue > 0)
            _hookCooldownValue -= Time.deltaTime;

        //Get the moveDirection
        _horizontalDirection = GetAxis().x;
        //Verify if gets the input to jump
        if (Input.GetButtonDown("Jump") && _canJump) { Jump(); } 
        //If the player cannot jump, then we verify if he can wall jump
        else if(Input.GetButtonDown("Jump") && _canWallJump) { StartCoroutine(WallJump()); }

        if (Input.GetMouseButtonDown(0) && _canDash) { StartCoroutine(Dash()); }

        if (Input.GetMouseButtonDown(0) && _canHook){ Hook(); }
        else if (Input.GetButtonDown("Hook") && _canHook) { Hook(); }

        if (Input.GetButtonDown("Interact")){ CallInteraction(); }

        if (_isRegretting)
        {
            GrabAction(_targetGameObject);

            _lineRenderer.SetPosition(1, transform.position);
            _lineRenderer.enabled = false;
        }

        if (Gamepad.current != null)
        {
            // Get the joystick position
            _leftStick = Gamepad.current.leftStick.ReadValue();
            // Prevent annoying jitter when not using joystick
            if (_leftStick.magnitude < 0.1f) return;
            // Get the current mouse position to add to the joystick movement
            _mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            _mousePosition = new Vector2(Mathf.Clamp(_mousePosition.x, 80, Screen.width - 80), Mathf.Clamp(_mousePosition.y, 80, Screen.height - 80));
            // Precise value for desired cursor position, which unfortunately cannot be used directly
            _warpPosition = _mousePosition + _bias + _overflow + _sensitivity * Time.deltaTime * _leftStick;
            // Keep the cursor in the game screen (behavior gets weird out of bounds)
            _warpPosition = new Vector2(Mathf.Clamp(_warpPosition.x, 0, Screen.width), Mathf.Clamp(_warpPosition.y, 0, Screen.height));
            // Store floating point values so they are not lost in WarpCursorPosition (which applies FloorToInt)
            _overflow = new Vector2(_warpPosition.x % 1, _warpPosition.y % 1);
            // Move the cursor
            Mouse.current.WarpCursorPosition(_warpPosition);
        }
    }

    void FixedUpdate()
    {
        MoveCharacter();
        WallSlide();
        //Apply Linear Drag
        if(_onGround) 
        {
            _extraJumpsValue = _extraJumps;
            _hangTimeCounter = _hangTime;
            _isWallJumping = false;
            _dashAmountValue = _dashAmount;
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
        if (_canMove && !_isDashing)
        {
            //Add a force in RB in horizontal direction in the velocity of acceleration that can be changed in the variable
            _rb.AddForce(new Vector2(_horizontalDirection, 0f) * _movementAcceleration);

            //Math.Abs always returns a positive number, that's why we use it here to compare with the max move speed, because if the player is moving backwards it will give a negative number and we don't wanna that
            if (Mathf.Abs(_rb.velocity.x) > _maxMoveSpeed && !_isDashing)
            {
                //Math.Sign returns always (-1,0 or 1), so we multiply this by the max move speed then we can have a linear speed 
                _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxMoveSpeed, _rb.velocity.y);
            }
            if (Mathf.Abs(_rb.velocity.y) > _maxYSpeed)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Sign(_rb.velocity.y) * _maxYSpeed);
            }
        }
    }
    private void ChangeFacingDirection()
    {
        //Facing Direction
        if( _horizontalDirection != 0f && _onGround )
        {
            if (_horizontalDirection > 0) { _facingRight = true; }
            else if (_horizontalDirection < 0) { _facingRight = false; }
        }
        else
        {
            if (_rb.velocity.x > 0) { _facingRight = true; }
            else if (_rb.velocity.x < 0) { _facingRight = false; }
        }

        //Change the scale according to facing direction
        if (_isWallSliding)
        {
            transform.localScale = new Vector3(_wallDirection * -1, 1, 1);
        }
        else if (_facingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
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
        //Save the start time of the dash
        float dashStartTime = Time.time;

        _isDashing = true;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector3 direction = (mousePosition - transform.position).normalized;

        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 0f;
        _rb.drag = 0f;
        while (Time.time < dashStartTime + _dashLength)
        {
            _rb.velocity = direction * _dashForce;
            yield return new WaitForSeconds(0.1f);
        }
        _dashCooldownValue = _dashCooldown;
        _dashAmountValue--;
        _isDashing = false;
        yield return null;
    }
    IEnumerator WallJump()
    {
        //Set the wall jump variable
        _isWallJumping = true;
        _isWallSliding = false;

        //Save the start time of the wall jump
        float jumpStartTime = Time.time;
        //Take the direction of the jump and set all the values to zero
        float _jumpingDirection = transform.localScale.x;
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 0f;
        _rb.drag = 0f;


        while (Time.time < jumpStartTime + _wallJumpLength)
        {
            _rb.velocity = new Vector2(_jumpingDirection * _wallJumpForce.x, _wallJumpForce.y);
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
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
        

        var hit = Physics2D.Raycast(transform.position - new Vector3(_wallSlidingCheckSize, 0, 0), Vector2.right, _wallSlidingCheckSize * 2, _wallLayer);
        if(hit && hit.collider.gameObject.tag == "Slidable" && !_onGround && !_isWallJumping)
        {
            _isWallSliding = true;
        }
        else { _isWallSliding = false; }
    }
    private void WallSlide()
    {
        if(_isWallSliding && !_isDashing)
        {
            _isWallJumping = false;
            _rb.velocity = new Vector2(0f, Mathf.Clamp(_rb.velocity.y, -_wallSlidingSpeed, float.MaxValue));
        }
    }
    private void FallMultiplier()
    {
        //if (!_isGliding)
        //{
            if (_rb.velocity.y < 0f)
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
        //}
    }
    private void Hook()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        RaycastHit2D hitHookable = Physics2D.Raycast(transform.position, direction, _hookThreshold, _hookableMask);
            
        if (hitHookable.collider != null)
        {
            _target = hitHookable.point;
            _lineRenderer.enabled = true;
            _lineRenderer.positionCount = 2;
            _targetGameObject = hitHookable.collider.gameObject;

            StartCoroutine(ThrowGrab());
        }
    }
    IEnumerator ThrowGrab()
    {
        float t = 0;
        float time = 10;

        Vector2 newPos;

        _lineRenderer.SetPosition(1, transform.position);

        for (; t < time; t += _hookSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(transform.position, _target, t / time);
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, newPos);
            yield return null;
        }

        _lineRenderer.SetPosition(1, _target);

        _isHooking = true;
        _isRegretting = true;
    }
    private void GrabAction(GameObject target)
    {
        if (target.tag == "Hookable")
        {
            if (Mathf.Clamp(transform.position.x - target.transform.position.x, -1, 1) != (float)target.GetComponent<HookableController>()._moveDirection)
            {
                _isHooking = false;
                _isRegretting = false;
                return;
            }
            
            Vector2 hookPos = Vector2.Lerp(new Vector2(target.transform.position.x, target.transform.position.y), new Vector2(transform.position.x, target.transform.position.y), _objSpeed * Time.deltaTime);

            target.transform.position = hookPos;

            _lineRenderer.SetPosition(1, target.transform.position);

            if (Vector2.Distance(transform.position, target.transform.position) <= 1f)
            {
                _isHooking = false;
                _isRegretting = false;
            }
        }
        else
        {
            Vector2 hookPos = Vector2.Lerp(transform.position, _target, _objSpeed * Time.deltaTime);

            transform.position = hookPos;

            if (Vector2.Distance(transform.position, _target) <= 1f)
            {
                _isHooking = false;
                _isRegretting = false;
            }
        }

        _hookCooldownValue = _hookCooldown;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + _groundRaycastOffset, transform.position + _groundRaycastOffset + Vector3.down * _groundRaycastLength);
        Gizmos.DrawLine(transform.position - _groundRaycastOffset, transform.position - _groundRaycastOffset + Vector3.down * _groundRaycastLength);
        Gizmos.DrawLine(transform.position - new Vector3(_wallSlidingCheckSize, 0, 0), transform.position + Vector3.right * _wallSlidingCheckSize);
    }
    public IEnumerator Death()
    {
        _rb.velocity = Vector2.zero;
        _isDead = true;
        _rb.angularVelocity = 0;
        _lineRenderer.enabled = false;
        _isDashing = false;
        _isHooking = false;
        _isRegretting = false;
        _isWallJumping = false;
        _isWallSliding = false;
        FindAnyObjectByType<SceneLoader>().LoadScene(SceneManager.GetActiveScene().name);

        yield return null;
    }
    private void CallInteraction()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _interactRadius, _interactLayer);

        if (colliders.Length > 0)
        {
            foreach(Collider2D collider in colliders)
            {
                collider.gameObject.GetComponent<Interact>().Interaction();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 6 || collision.collider.gameObject.layer == 3)
        {
            if (collision.gameObject.transform.position.x - transform.position.x < 0)
            { _wallDirection = -1; }
            else
            { _wallDirection = 1; }
        }
        if (_isHooking)
        {
            _isHooking = false;
            _isRegretting = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WorldLimit")
        {
            StartCoroutine(Death());
        }   
    }
}