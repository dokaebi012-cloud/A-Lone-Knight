using System.Collections.Generic;
using System.Linq;
using Unity.Behavior;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] patrolPoints; // 순찰 지점
    public float patrolSpeed = 2f;
    public float chaseSpeed = 2f;

    public GameObject player;
    public float detectionRange = 5f;

    private int currentPatrolIndex = 0;
    private Rigidbody2D rb;

    private EnemyHealth enemyHealth;
    public Animator animator;

    public Transform targetPoint;
    private EnemyJump enemyJump;

    public GameObject hitbox;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        if (animator != null)
        {
            animator = GetComponent<Animator>();
        }
        player = GameObject.FindWithTag("Player");
        enemyJump = GetComponent<EnemyJump>();
    }

    void Update()
    {
        if (enemyHealth.isAlive && enemyJump.isGrounded)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer < detectionRange)
            {
                ChasePlayer();
            }
            else
            {
                Patrol();
            }
        }
        //NoAttackOnAir();
    }

    void Patrol()
    {
        if (patrolPoints.Length > 0)
        {
            targetPoint = patrolPoints[currentPatrolIndex];
            Vector2 direction = (targetPoint.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * patrolSpeed, rb.linearVelocity.y);

            JumpTowardTarget();

            // 지점에 도달했는지 확인
            if (Vector2.Distance(transform.position, targetPoint.position) < 0.5f)
            {
                currentPatrolIndex = ++currentPatrolIndex % patrolPoints.Length;
            }
            animator.SetBool("isRunning", true);

        }
        else
        {
            animator.SetBool("isRunning", false);
            return;
        }
    }

    void ChasePlayer()
    {
        targetPoint = player.transform;
        Vector2 direction = (targetPoint.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * chaseSpeed, rb.linearVelocity.y);
        animator.SetBool("isRunning", true);

        // 추적 이동
        Vector2 moveDirection = (targetPoint.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(moveDirection.x * chaseSpeed, rb.linearVelocity.y);

        JumpTowardTarget();

        if (animator != null)
        {
            animator.SetBool("isRunning", true);
        }
    }

    void JumpTowardTarget()
    {
        // 표적이 위/아래에 있을 경우 아래점프/점프
        if (Mathf.Abs(targetPoint.transform.position.x - transform.position.x) < 2)
        {
            if (((targetPoint.transform.position.y - transform.position.y) < -2) && enemyJump.isGrounded)
            {
                enemyJump.IgnorePlatformCollisionTemporary(0.5f);
            }
            else if (((targetPoint.transform.position.y - transform.position.y) > 2) && enemyJump.isGrounded)
            {
                enemyJump.Jump();
            }

        }
    }

    void NoAttackOnAir()
    {
        hitbox.SetActive(enemyJump.isGrounded);
    }

    void OnDrawGizmosSelected()
    {
        // 감지 범위 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

