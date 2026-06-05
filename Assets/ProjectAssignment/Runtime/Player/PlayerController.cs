using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;      
    [SerializeField] private AudioSource footstepSource; 
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip runClip;

    private InputAction moveAction;
    private InputAction jumpAction;
    private float moveInput;
    private bool jumpRequested;

    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isJumping = false;


    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Player/Move");
        jumpAction = InputSystem.actions.FindAction("Player/Jump");
    }

    private void OnEnable()
    {
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
        moveAction.Enable();

        jumpAction.performed += OnJump;
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        moveAction.Disable();

        jumpAction.performed -= OnJump;
        jumpAction.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().x;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpRequested = true;
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rigidbody.linearVelocity;

        if (moveInput != 0)
        {
            velocity.x = moveInput * moveSpeed;

            spriteRenderer.flipX = velocity.x < 0 ? true : false;

            isMoving = true;
        }
        else isMoving = false;


        bool grounded = IsGrounded();

        if (jumpRequested && grounded)
        {
            velocity.y = jumpForce;
            isJumping = true;
            PlayJumpSound();
        }
        jumpRequested = false;

        if (grounded && velocity.y <= 0f)
            isJumping = false;

        rigidbody.linearVelocity = velocity;

        UpdateFootstepSound(grounded);
        CheckAnimation();
    }

    private void PlayJumpSound()
    {
        if (sfxSource != null && jumpClip != null)
            sfxSource.PlayOneShot(jumpClip);
    }

    private void UpdateFootstepSound(bool grounded)
    {
        if (footstepSource == null || runClip == null)
            return;

        bool shouldPlay = isMoving && grounded;

        if (shouldPlay && !footstepSource.isPlaying)
        {
            footstepSource.clip = runClip;
            footstepSource.loop = true;
            footstepSource.Play();
        }
        else if (!shouldPlay && footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private void CheckAnimation()
    {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isJumping", isJumping);
    }
}
