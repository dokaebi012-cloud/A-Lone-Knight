using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool isGrounded = true;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public float jumpUpDuration = 0.5f;
    private JumpPoint jumpPoint;
    public float selfJumpForce = 5f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //groundCheck = GetComponent<Transform>(); //위치 설정을 덮어쓰므로 groundCheck 방해
    }
    private void Update()
    {
        OnGround();
    }
    private void OnGround()
    {
        isGrounded = (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer)
            || Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, platformLayer));
        //Debug.Log("isSlimeGrounded : " + isGrounded);
    }
    //트리거 충돌 시 점프
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("JumpPoint") && isGrounded)
        {
            jumpPoint = other.GetComponent<JumpPoint>();
            if (jumpPoint == null) return;

            // 점프포인트와 몬스터의 상대적인 방향(=점프포인트 방향) 판단
            float directionToJumpPoint = other.transform.position.x - transform.position.x;

            // x축 속도를 horizontalForce 만큼 가속 -> 점프 중 가로 방향 힘 정의
            EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
            enemyMovement.chaseSpeed += jumpPoint.horizontalForce;
            enemyMovement.patrolSpeed += jumpPoint.horizontalForce;

            // 점프포인트 방향에 따른 점프 가능 여부 판단 후 점프
            switch (jumpPoint.allowedDirection)
            {
                case JumpPoint.AllowedDirection.Both:
                    Jump(jumpPoint.jumpForce);
                    break;
                case JumpPoint.AllowedDirection.LeftOnly:
                    if (directionToJumpPoint > 0)
                    {
                        Jump(jumpPoint.jumpForce);
                    }
                    break;
                case JumpPoint.AllowedDirection.RightOnly:
                    if (directionToJumpPoint < 0)
                    {
                        Jump(jumpPoint.jumpForce);
                    }
                    break;
            }

        }
    }
    public void Jump()
    {
        // y축 속도를 초기화해 점프 속도에 기존 속도가 누적되는 현상을 방지
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        // 점프, 플랫폼 충돌 무효화
        rb.AddForce((Vector2.up.normalized * selfJumpForce), ForceMode2D.Impulse);
        IgnorePlatformCollisionTemporary(jumpUpDuration);
    }
    public void Jump(float jumpForce)
    {
        // y축 속도를 초기화해 점프 속도에 기존 속도가 누적되는 현상을 방지
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        // 점프, 플랫폼 충돌 무효화
        rb.AddForce((Vector2.up.normalized * jumpForce), ForceMode2D.Impulse);
        IgnorePlatformCollisionTemporary(jumpUpDuration);
    }
    public void IgnorePlatformCollisionTemporary(float duration)
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform"), true);
        Invoke(nameof(EnablePlatformCollision), duration);
    }

    private void EnablePlatformCollision()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform"), false);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.transform.position, groundCheckRadius);
    }

}
