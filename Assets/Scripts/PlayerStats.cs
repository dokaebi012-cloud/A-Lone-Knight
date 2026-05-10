using UnityEngine;

/// <summary>
/// 응용하지 않은 플레이어 스탯 스크립트
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("플레이어 능력치")]
    public int maxHP = 100;
    public int currentHP;
    public float damage = 10.0f;
    public float attackSpeed = 1.0f;
    public float moveSpeed = 3.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
        currentHP = maxHP;
    }
    void Start()
    {
        GameManager.Instance.LoadPlayerState(this);
    }

    public void TakeDamage(int amount)
    {
        SoundManager.Instance.PlaySFX(SFXType.PlayerDamaged);
        currentHP -= amount;
        if (currentHP <= 0)
        {
            //죽음
        }
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public void Die()
    {
        //Gameover 창 열기
    }

    public float GetDamage() => damage;

    public float GetAttackSpeed() => attackSpeed;

    public void UpgradeDamage(float amount)
    {
        damage += amount;
        GameManager.Instance.SavePlayerStats(this);
    }
    public void UpgradeAttackSpeed(int amount)
    {
        attackSpeed += amount;
        GameManager.Instance.SavePlayerStats(this);
    }
    public void UpgradeHP(int amount)
    {
        maxHP += amount;
        GameManager.Instance.SavePlayerStats(this);
    }
    public void UpgradeMoveSpeed(int amount)
    {
        moveSpeed += amount;
        GameManager.Instance.SavePlayerStats(this);
    }
}
