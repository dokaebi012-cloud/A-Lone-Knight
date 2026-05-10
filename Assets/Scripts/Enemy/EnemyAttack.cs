using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject attackHitbox;
    public Animator animator;
    private GameObject player;

    public float attackRange = 2.5f; // 공격 발동 거리
    public float attackCooldown = 2.0f; // 공격 간 쿨타임
    private float lastAttackTime;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        // Player 태그로 대상 찾기
        player = GameObject.FindWithTag("Player");
        lastAttackTime = Time.time;
        attackHitbox.SetActive(false);
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }
    }

    // 애니메이션 이벤트에서 호출
    public void StartAttack()
    {
        if (attackHitbox != null)
        {
            SoundManager.Instance.PlaySFX(SFXType.SwordSwing);
            attackHitbox.SetActive(true);
            Debug.Log("Deathbringer attack start");
        }
    }

    // 애니메이션 이벤트에서 호출
    public void StopAttack()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
            Debug.Log("Deathbringer attack stop");
        }
    }

    void OnDrawGizmosSelected()
    {
        // 감지 범위 시각화
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
