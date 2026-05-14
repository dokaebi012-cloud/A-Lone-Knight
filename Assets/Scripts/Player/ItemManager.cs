using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject itemEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                if (gameObject.CompareTag("Shield"))
                {
                    playerHealth.shieldCount++;
                    SoundManager.Instance.PlaySFX(SFXType.Blocked);
                    PlayEffect(collision);
                    Destroy(gameObject);
                }
                else if (gameObject.CompareTag("Health") && playerHealth.health < 100)
                {
                    playerHealth.health += 20;
                    SoundManager.Instance.PlaySFX(SFXType.GetHealthPotion);
                    PlayEffect(collision);
                    Destroy(gameObject);
                }
            }
        }
    }

    private void PlayEffect(Collider2D collision)
    {
        Vector3 effectPosition = new Vector3(collision.transform.position.x, collision.transform.position.y - 0.25f, 0);

        GameObject effect = Instantiate(itemEffect, effectPosition, Quaternion.identity, collision.transform);

        effect.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);

        // 이펙트가 무조건 지형보다 앞에 보이도록 sorting layer 강제
        ParticleSystemRenderer[] renderers = effect.GetComponentsInChildren<ParticleSystemRenderer>();
        foreach (var r in renderers)
        {
            r.sortingLayerName = "Foreground";
            r.sortingOrder = 10;
        }
        Destroy(effect, 1.0f);

    }
}
