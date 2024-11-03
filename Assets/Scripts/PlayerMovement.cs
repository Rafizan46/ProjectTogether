using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _anim;

    [SerializeField] private SpriteRenderer _sr;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;

    public LayerMask wallLayer;
    public Transform wallCheck;
    public float wallCheckDistance;

    public float moveSpeed;
    public float jumpForce;
    public float doubleJumpCount;

    public float defaultGravityScale;
    public float gravityMultiplier;

    private float _doubleJumpLeft;
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _isWallSliding;
    public float wallSlideSpeed = 2f;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        _doubleJumpLeft = doubleJumpCount;
    }

    void Update()
    {
        HandleMove();
        HandleJump();
        HandleWallSlide();
        HandleVisuals();
    }

    void HandleMove()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        _rb.velocity = new Vector2(moveInput * moveSpeed, _rb.velocity.y);

        if (moveInput > 0)
        {
            _sr.flipX = false;
        }
        else if (moveInput < 0)
        {
            _sr.flipX = true;
        }
    }

    void HandleJump()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetButtonDown("Jump"))
        {
            if (_isGrounded)
            {
                Jump();
            }
            else if (_isWallSliding)
            {
                WallJump();
            }
            else if (_doubleJumpLeft > 0)
            {
                _doubleJumpLeft--;
                Jump();
            }
        }

        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0)
        {
            _rb.gravityScale = defaultGravityScale * gravityMultiplier;
        }
        else if (_isGrounded)
        {
            _rb.gravityScale = defaultGravityScale;
            _doubleJumpLeft = doubleJumpCount;
        }
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            _anim.Play("player_jump");
        }
        else
        {
            _anim.Play("player_doublejump");
        }

        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        _rb.gravityScale = defaultGravityScale;
    }

    private void WallJump()
    {
        _anim.Play("player_jump");
        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        _rb.gravityScale = defaultGravityScale;
    }

    private void HandleWallSlide()
    {
        _isTouchingWall = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, wallLayer) ||
                            Physics2D.Raycast(wallCheck.position, Vector2.left, wallCheckDistance, wallLayer);
        _isWallSliding = _isTouchingWall && !_isGrounded && _rb.velocity.y <= 0;

        if (_isWallSliding)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -wallSlideSpeed);
            _doubleJumpLeft = doubleJumpCount;
        }
    }

    private void HandleVisuals()
    {
        _anim.SetFloat("h_speed", Mathf.Abs(_rb.velocity.x));
        _anim.SetFloat("v_speed", _rb.velocity.y);
        _anim.SetBool("is_grounded", _isGrounded);
        _anim.SetBool("is_wall_sliding", _isWallSliding);
    }
}