using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    public float colorChangeDuration = 0.1f;

    public GameObject hitEffectPrefab; // 피격 이펙트 프리팹 추가

    public float enemyHealth = 100.0f;

    public Animator animator;
    public bool isAlive = true;

    private CollisionDamage collisionDamage;

    // 몬스터 사망시 실행되는 delegate 방식 이벤트
    public delegate void DeathDelegate();
    public event DeathDelegate OnDeath;

    private DeathbringerManager deathbringerManager;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        if (animator != null)
        {
            animator = GetComponent<Animator>();
        }
        collisionDamage = GetComponentInChildren<CollisionDamage>();
        deathbringerManager = GameObject.FindAnyObjectByType<DeathbringerManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            SpawnHitEffect(); // 피격 이펙트 생성
            PlayHitSound();
            GetDamage(collision);
        }
    }

    IEnumerator HitEffect()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(colorChangeDuration);
        spriteRenderer.color = originalColor;
    }

    void SpawnHitEffect()
    {
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f); // 0.5초 후 자동 삭제
        }
    }

    void PlayHitSound()
    {
        SoundManager.Instance.PlaySFX(SFXType.MonsterDamaged);
    }

    void GetDamage(Collider2D collider2D)
    {
        if (collider2D != null)
        {
            PlayerAttack playerAttack = collider2D.GetComponentInParent<PlayerAttack>(); ;
            enemyHealth -= playerAttack.playerDamage;
            //Debug.Log($"Damage : {playerAttack.playerDamage} \nHealth left : {enemyHealth} ");
            
            if(enemyHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(HitEffect());
            }

        }

    }

    // 사망 판정 시 
    // 1. 히트박스 비활성화
    // 2. 사망 애니메이션 재생
    // 3. 애니메이션을 재생하기 충분한 시간이 지난 후 본체 제거
    private void Die()
    {
        // 죽은 다음 사망 기믹 + 아이템 스폰 중복 실행 방지
        if (!isAlive)
        {
            return;
        }

        isAlive = false;
        collisionDamage.isActive = false;
        animator.SetTrigger("Die");

        // 이벤트 발생
        if (OnDeath != null)
        {
            OnDeath.Invoke();   // 이벤트 발생 → 등록된 모든 함수 실행됨
            OnDeath = null;     // 이벤트는 한 번만 호출되도록
        }

        // 슬라임이 죽을 경우 데스브링어 스폰 대기시간 감소
        if(gameObject.tag == "Slime" && deathbringerManager != null)
        {
            deathbringerManager.OnSlimeDied();
        }

        if (gameObject.tag == "Deathbringer" && deathbringerManager != null)
        {
            VictoryManager.instance.IsVictory();
        }

        Destroy(gameObject, 1.0f);
    }

}
