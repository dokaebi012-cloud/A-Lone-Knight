using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;

    private Rigidbody2D rigidBody;
    private bool isGrounded;

    private float moveInput;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public LayerMask platformLayer;

    private Animator animator;
    private PlayerAnimation playerAnimation;
    private bool isRunning = false;

    private PlayerAudio playerAudio;

    public bool lookingRight = true;

    private PlayerHealth playerHealth;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAudio = GetComponent<PlayerAudio>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void HandleMovement()
    {
        if (playerHealth.isAlive)
        {
            HorizontalMove();
            HandleRotation(moveInput);
            PerformJump();
            StepDownPlatform();
        }
        OnGround();
    }
    private void HandleRotation(float moveInput)
    {
        if (moveInput > 0)
        {
            lookingRight = true;
        }
        else if (moveInput < 0)
        {
            lookingRight = false;
        }
        SwitchRotation();
    }
    private void SwitchRotation()
    {
        if (lookingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    private void PerformJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            playerAnimation.TriggerJump();
            //playerAudio.PlayBoingJump();
            SoundManager.Instance.PlaySFX(SFXType.Jump);
            StartCoroutine(DisablePlatformCollision());
        }
    }

    private void HorizontalMove()
    {
        moveInput = Input.GetAxis("Horizontal");
        rigidBody.linearVelocity = new Vector2(moveInput * moveSpeed, rigidBody.linearVelocity.y);
        isRunning = (moveInput != 0);
        playerAnimation.SetRunning(isRunning && isGrounded);
    }

    private void OnGround()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer)
            || Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, platformLayer);
        animator.SetBool("IsGrounded", isGrounded);

    }

    private void StepDownPlatform()
    {
        if (Input.GetKeyDown(KeyCode.S) && isGrounded)
        {
            StartCoroutine(DisablePlatformCollision());
        }
    }

    private IEnumerator DisablePlatformCollision()
    {
        // 현재 플레이어가 속한 레이어와 Platform 레이어 간의 충돌을 끔
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform"), true);
        Debug.Log("플랫폼 충돌 비활성화");

        yield return new WaitForSeconds(0.5f); // 0.5초 후 다시 충돌 활성화

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform"), false);
        Debug.Log("플랫폼 충돌 복구");
    }


}
