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

    private bool isJumping = false;
    public float jumpingTime = 0.5f;
    public float FixOnGroundRecoveryTime = 0.2f;

    [Header("FixOnGround Settings")]
    public float groundDistanceThreshold = 0.3f;

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
        IsOnGround();
        FixOnGround();
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
            Debug.Log("W pressed");
            SetIsJumping();
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            playerAnimation.TriggerJump();
            //playerAudio.PlayBoingJump();
            SoundManager.Instance.PlaySFX(SFXType.Jump);
            Invoke("SetIsNotJumping", FixOnGroundRecoveryTime);
            StartCoroutine(DisablePlatformCollision());
        }
    }

    private void HorizontalMove()
    {
        moveInput = Input.GetAxis("Horizontal");
        if(moveInput != 0)
        {
            //Debug.Log($"moving horizontally");
        }
        rigidBody.linearVelocity = new Vector2(moveInput * moveSpeed, rigidBody.linearVelocity.y);
        isRunning = (moveInput != 0);
        playerAnimation.SetRunning(isRunning && isGrounded);
    }

    private void IsOnGround()
    {
        bool wasGrounded = isGrounded;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer)
            || Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, platformLayer);
        animator.SetBool("IsGrounded", isGrounded);


    }

    private void StepDownPlatform()
    {
        if (Input.GetKeyDown(KeyCode.S) && isGrounded)
        {
            isJumping = true;
            StartCoroutine(DisablePlatformCollision());
        }
    }

    private IEnumerator DisablePlatformCollision()
    {
        // 현재 플레이어가 속한 레이어와 Platform 레이어 간의 충돌을 끔
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform"), true);
        //Debug.Log("플랫폼 충돌 비활성화");

        yield return new WaitForSeconds(jumpingTime); // 다시 충돌 활성화

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform"), false);
        //Debug.Log("플랫폼 충돌 복구");
    }

    // 경사로에서 잠시 뜰 때 끌어내리는 힘을 주는 메소드
    private void FixOnGround()
    {
        if (!isGrounded && !isJumping)
        {
            // 지면과의 거리 측정
            RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundDistanceThreshold * 2f, groundLayer | platformLayer);

            if (hit.collider != null)
            {
                float distanceToGround = hit.distance;

                if (distanceToGround <= groundDistanceThreshold)
                {
                    // 지면과 매우 가까우면 다운포스 적용
                    rigidBody.AddForce(Vector2.down * 10000f);
                }
            }
        }
    }


    private void SetIsJumping()
    {
        isJumping = true;
    }
    private void SetIsNotJumping()
    {
        isJumping = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistanceThreshold);
    }

}
