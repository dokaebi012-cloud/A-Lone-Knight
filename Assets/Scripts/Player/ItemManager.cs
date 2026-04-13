using UnityEngine;

public class ItemManager : MonoBehaviour
{
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
                    Destroy(gameObject);
                }
                else if (gameObject.CompareTag("Health") && playerHealth.health < 100)
                {
                    playerHealth.health += 20;
                    SoundManager.Instance.PlaySFX(SFXType.GetHealthPotion);
                    Destroy(gameObject);
                }

            }

        }
    }
}
