using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100;
    public int shieldCount = 0;

    public TextMeshProUGUI shieldCountUI;
    public TextMeshProUGUI healthCountUI;
    private SpriteRenderer spriteRenderer;
    private float colorChangeDuration = 0.1f;
    private Color originalColor;

    private PlayerAnimation playerAnimation;
    public bool isAlive = true;

    private PlayerUI playerUI;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        playerAnimation = GetComponent<PlayerAnimation>();
        playerUI = GetComponent<PlayerUI>();
    }

    public void GetDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            isAlive = false;
            playerAnimation.Die();
            // playerAnimation.Die() 애니메이션이 끝난 후 playerUI.SwitchIsPaused()가 실행되어야 한다.
            StartCoroutine(DieAndPause());
            return;
        }
        PlayHitSound();
        StartCoroutine(HitEffect());
    }
    IEnumerator HitEffect()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(colorChangeDuration);
        spriteRenderer.color = originalColor;
    }
    void PlayHitSound()
    {
        SoundManager.Instance.PlaySFX(SFXType.PlayerDamaged);
    }

    IEnumerator DieAndPause()
    {
        yield return StartCoroutine(playerAnimation.ActionkCooldownByAimation());
        playerUI.SwitchIsPaused();
    }
}
