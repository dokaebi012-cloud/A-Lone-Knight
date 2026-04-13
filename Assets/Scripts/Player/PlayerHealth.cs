using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void GetDamage(float damage)
    {
        health -= damage;
        StartCoroutine(HitEffect());
        PlayHitSound();
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
}
