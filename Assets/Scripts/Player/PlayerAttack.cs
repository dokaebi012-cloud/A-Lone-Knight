using Cinemachine;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private PlayerAnimation playerAnimation;
    private PlayerAudio playerAudio;
    private PlayerHealth playerHealth;

    [Header("Hitbox")]
    public GameObject attackHitbox;
    public GameObject blockHitbox;

    [Header("Effect")]
    public GameObject attackEffectPrefab;
    public GameObject blockEffectPrefab;

    [Header("UI")]
    public GameObject shieldIcon;

    [Header("Combat")]
    public float playerDamage = 20f;
    public float parryInvincibilityDuration = 5.0f;

    [Header("Input Lock")]
    public float attackCooldown = 0.4f;
    public float blockCooldown = 0.4f;

    [Header("Camera Shake")]
    public CinemachineImpulseSource impulseSource;

    public LayerMask enemyLayer;

    // 입력 중복 방지
    private bool isAttackLocked;
    private bool isBlockLocked;

    // 패리 중복 처리 방지
    private bool hasParried;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAudio = GetComponent<PlayerAudio>();
        playerHealth = GetComponent<PlayerHealth>();

        attackHitbox.SetActive(false);
        blockHitbox.SetActive(false);
        shieldIcon.SetActive(false);
    }

    void Update()
    {
        Combat();
    }

    public void Combat()
    {
        PerformAttack();
        PerformBlock();
    }

    // 공격 입력
    public void PerformAttack()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        if (!playerHealth.isAlive)
            return;

        // 공격 중복 입력 방지
        if (isAttackLocked)
            return;

        // 다른 액션 중 차단
        if (playerAnimation.isDoingAction)
            return;

        Debug.Log("Attack");

        isAttackLocked = true;
        isBlockLocked = true;

        playerAnimation.TriggerAttack();

        SoundManager.Instance.PlaySFX(SFXType.SwordSwing);

        StartCoroutine(playerAnimation.ActionkCooldownByAimation());
        StartCoroutine(AttackCooldownRoutine());
    }

    IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);

        isAttackLocked = false;
        isBlockLocked = false;
    }

    // 패리 입력
    public void PerformBlock()
    {
        if (!Input.GetKeyDown(KeyCode.LeftShift))
            return;

        if (!playerHealth.isAlive)
            return;

        if (playerHealth.isInvincible)
            return;

        // 패리 중복 입력 방지
        if (isBlockLocked)
            return;

        // 다른 액션 중 차단
        if (playerAnimation.isDoingAction)
            return;

        // 방패 부족
        if (playerHealth.shieldCount <= 0)
            return;

        Debug.Log("Parry");

        isBlockLocked = true;
        isAttackLocked = true;

        playerAnimation.TriggerBlock();

        StartCoroutine(playerAnimation.ActionkCooldownByAimation());
        StartCoroutine(BlockCooldownRoutine());
    }

    IEnumerator BlockCooldownRoutine()
    {
        yield return new WaitForSeconds(blockCooldown);

        isBlockLocked = false;
        isAttackLocked = false;
    }

    // 공격 판정 시작
    // 애니메이션 이벤트
    public void GiveDamage()
    {
        // 중복 활성화 방지
        if (attackHitbox.activeSelf)
            return;

        GenerateCameraImpulse();

        attackHitbox.SetActive(true);

        // 공격 이펙트 생성
        GameObject effect = Instantiate(
            attackEffectPrefab,
            attackHitbox.transform.position,
            Quaternion.identity,
            transform
        );

        effect.transform.localScale =
            new Vector3(0.5f, 0.5f, 0.5f);

        ParticleSystemRenderer[] renderers =
            effect.GetComponentsInChildren<ParticleSystemRenderer>();

        foreach (var r in renderers)
        {
            r.sortingLayerName = "Foreground";
            r.sortingOrder = 10;
        }

        ParticleSystem particleSystem =
            effect.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            Destroy(effect, particleSystem.main.duration);
        }
    }

    // 공격 판정 종료
    public void StopGivingDamage()
    {
        attackHitbox.SetActive(false);
    }

    // 패리 시작
    // 애니메이션 이벤트
    public void StartBlock()
    {
        // 중복 실행 방지
        if (blockHitbox.activeSelf)
            return;

        // 패리 성공 여부 초기화
        hasParried = false;

        blockHitbox.SetActive(true);

        GameObject effect = Instantiate(
            blockEffectPrefab,
            transform.position,
            Quaternion.identity,
            transform
        );

        effect.transform.localScale =
            new Vector3(0.18f, 0.18f, 0.18f);

        ParticleSystemRenderer[] renderers =
            effect.GetComponentsInChildren<ParticleSystemRenderer>();

        foreach (var r in renderers)
        {
            r.sortingLayerName = "Foreground";
            r.sortingOrder = 10;
        }

        ParticleSystem particleSystem =
            effect.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            Destroy(effect, particleSystem.main.duration);
        }

        Debug.Log("Block start");
    }

    // 패리 종료
    public void StopBlock()
    {
        blockHitbox.SetActive(false);

        Debug.Log("Block stop");
    }

    // 카메라 쉐이크
    private void GenerateCameraImpulse()
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
        else
        {
            Debug.LogWarning("ImpulseSource가 연결되지 않았습니다");
        }
    }

    // 패리 성공 판정
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!playerHealth.isAlive)
            return;

        // 이미 패리 성공했으면 무시
        if (hasParried)
            return;

        if (blockHitbox.activeSelf &&
            (
                other.gameObject.layer ==
                LayerMask.NameToLayer("SlimeAttack")
                ||
                other.gameObject.layer ==
                LayerMask.NameToLayer("DeathbringerAttack")
            ))
        {
            // 패리 성공 처리
            hasParried = true;

            // 추가 충돌 차단
            blockHitbox.SetActive(false);

            // 방패 1회만 소모
            if (playerHealth.shieldCount > 0)
            {
                playerHealth.shieldCount--;
            }

            GenerateCameraImpulse();

            // 패리 성공 무적
            StartCoroutine(
                TemporaryInvincibility(parryInvincibilityDuration)
            );

            // 효과음
            SoundManager.Instance.PlaySFX(SFXType.Blocked);

            Debug.Log("Parry Success");
        }
    }

    // 임시 무적
    IEnumerator TemporaryInvincibility(float duration)
    {
        playerHealth.isInvincible = true;

        shieldIcon.SetActive(true);

        Debug.Log("Invincible Enabled");

        yield return new WaitForSeconds(duration);

        playerHealth.isInvincible = false;

        shieldIcon.SetActive(false);

        Debug.Log("Invincible Disabled");
    }
}