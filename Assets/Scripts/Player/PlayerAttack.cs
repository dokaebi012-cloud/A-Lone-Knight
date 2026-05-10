using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private PlayerAnimation playerAnimation;
    private PlayerAudio playerAudio;

    public GameObject attackHitbox;
    public GameObject blockHitbox;
    public float parryInvincibilityDuration = 5.0f; // 무적 시간

    public LayerMask enemyLayer;

    private bool isAttacking;

    //함수를 이용한 camera shake
    //float shakeDuration = 0.5f;
    //float shakeMagnitude = 0.1f;
    private Vector3 originalPos;

    //cinemachine의 impulse listener와 impulse source를 이용한 camera shake
    public CinemachineImpulseSource impulseSource;

    public float playerDamage = 20;

    private PlayerHealth playerHealth;
    public GameObject attackEffectPrefab; // 공격 이펙트 프리팹
    public GameObject blockEffectPrefab;

    public GameObject shieldIcon;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAudio = GetComponent<PlayerAudio>();

        attackHitbox.SetActive(false);
        blockHitbox.SetActive(false);
        shieldIcon.SetActive(false);
        playerHealth = GetComponent<PlayerHealth>();

    }
    public void Combat()
    {
        PerformAttack();
        PerformBlock();
    }
    public void PerformAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerHealth.isAlive)
        {
            if (!playerAnimation.isDoingAction)
            {
                playerAnimation.TriggerAttack();
                //playerAudio.PlaySwordSwing();
                SoundManager.Instance.PlaySFX(SFXType.SwordSwing);
            }
            else
            {
                return;
            }
            StartCoroutine(playerAnimation.ActionkCooldownByAimation());
            //StartCoroutine(Shake(shakeDuration, shakeMagnitude));
            GenerateCameraImpulse();

        }
    }
    public void PerformBlock()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && playerHealth.isAlive && !playerHealth.isInvincible)
        {
            if (!playerAnimation.isDoingAction && playerHealth.shieldCount > 0)
            {
                playerAnimation.TriggerBlock();
                StartCoroutine(playerAnimation.ActionkCooldownByAimation());
            }
        }
    }

    // 애니메이션에서 호출
    public void GiveDamage()
    {
        // 히트박스 활성화
        attackHitbox.SetActive(true);
        
        // 이펙트 생성(대상, 위치, 회전, 부모)
        GameObject effect = Instantiate(attackEffectPrefab, attackHitbox.transform.position, Quaternion.identity, transform);
        
        // 이펙트 크기 조절
        effect.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // 크기 조절 (필요에 따라 조정)

        // 이펙트가 무조건 지형보다 앞에 보이도록 sorting layer 강제
        ParticleSystemRenderer[] renderers = effect.GetComponentsInChildren<ParticleSystemRenderer>();
        foreach (var r in renderers)
        {
            r.sortingLayerName = "Foreground";
            r.sortingOrder = 10;
        }

        // 이펙트 재생 시간을 ParticleSystem 기반으로 감지해 종료 시 자동 제거
        ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
        if(particleSystem != null)
        {
            Destroy(effect, particleSystem.main.duration);
        }
    }

    public void StopGivingDamage()
    {
        attackHitbox.SetActive(false);
    }

    public void StartBlock()
    {

        blockHitbox.SetActive(true);

        GameObject effect = Instantiate(blockEffectPrefab, transform.position, Quaternion.identity, transform);

        effect.transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);

        ParticleSystemRenderer[] renderers = effect.GetComponentsInChildren<ParticleSystemRenderer>();
        foreach (var r in renderers)
        {
            r.sortingLayerName = "Foreground";
            r.sortingOrder = 10;
        }

        ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            Destroy(effect, particleSystem.main.duration);
        }
        Debug.Log("Block start");
    }
    public void StopBlock()
    {
        blockHitbox.SetActive(false);
        Debug.Log("Block stop");
    }

    private void GenerateCameraImpulse()
    {
        if(impulseSource != null)
        {
            Debug.Log("카메라 임펄스 발생");
            impulseSource.GenerateImpulse();
        }
        else
        {
            Debug.LogWarningFormat("ImpulseSource가 연결되어 있지 않습니다");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!playerHealth.isAlive) return;

        if (blockHitbox.activeSelf && 
            (other.gameObject.layer == LayerMask.NameToLayer("SlimeAttack") || 
            other.gameObject.layer == LayerMask.NameToLayer("DeathbringerAttack")))
        {
            if(playerHealth.shieldCount > 0) playerHealth.shieldCount--;
            GenerateCameraImpulse();
            // 무적 처리
            StartCoroutine(TemporaryInvincibility(parryInvincibilityDuration));

            // 적 애니메이터에 Blocked 트리거 발동
            Animator enemyAnimator = other.GetComponentInParent<Animator>();
            if (enemyAnimator != null)
            {
                enemyAnimator.SetTrigger("Blocked");
            }

            // 효과음
            SoundManager.Instance.PlaySFX(SFXType.Blocked);
        }
    }
    IEnumerator TemporaryInvincibility(float duration)
    {
        playerHealth.isInvincible = true;
        shieldIcon.SetActive(true);
        Debug.Log("invincible enabled");
        yield return new WaitForSeconds(duration);
        playerHealth.isInvincible = false;
        shieldIcon.SetActive(false);
        Debug.Log("invincible disabled");
    }

}
