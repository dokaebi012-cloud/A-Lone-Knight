using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 응용하지 않은 게임 데이터(코인 등) 갱신 및 관리 스크립트
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int coinCount = 0;

    public Text coinText;

    private const string COIN_KEY = "CoinCount";
    private const string DAMAGE_KEY = "PlayerDamage";
    private const string ATTACK_SPEED_KEY = "PlayerAttackSpeed";
    private const string MOVE_SPEED_KEY = "PlayerMoveSpeed";
    private const string HP_KEY = "PlayerHP";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        coinText.text = coinCount.ToString();
        SaveCoin();
    }

    public bool UseCoin(int amount)
    {
        if(coinCount >= 0)
        {
            coinCount -= amount;
            SaveCoin();
            return true;
        }
        Debug.Log("코인 부족");
        return false;
    }

    public int GetCoinCount()
    {
        return coinCount;
    }
    
    private void SaveCoin()
    {
        PlayerPrefs.SetInt(COIN_KEY, coinCount);
        PlayerPrefs.Save();
    }

    private void LoadCoin()
    {
        coinCount = PlayerPrefs.GetInt(COIN_KEY, 0);
    }
    public void SavePlayerStats(PlayerStats stats)
    {
        PlayerPrefs.SetFloat(DAMAGE_KEY, stats.damage);
        PlayerPrefs.SetFloat(MOVE_SPEED_KEY, stats.moveSpeed);
        PlayerPrefs.SetFloat(ATTACK_SPEED_KEY, stats.attackSpeed);
        PlayerPrefs.SetFloat(HP_KEY, stats.maxHP);

    }

    public void LoadPlayerState(PlayerStats stats)
    {
        if(PlayerPrefs.HasKey(DAMAGE_KEY))
        {
            stats.damage = PlayerPrefs.GetInt(DAMAGE_KEY);
        }
        if (PlayerPrefs.HasKey(ATTACK_SPEED_KEY))
        {
            stats.attackSpeed = PlayerPrefs.GetFloat(ATTACK_SPEED_KEY);
        }
        if (PlayerPrefs.HasKey(HP_KEY))
        {
            stats.maxHP = PlayerPrefs.GetInt(HP_KEY);
        }
        if (PlayerPrefs.HasKey(MOVE_SPEED_KEY))
        {
            stats.moveSpeed = PlayerPrefs.GetFloat(MOVE_SPEED_KEY);
        }
    }

    public void ResetCoin()
    {
        coinCount = 0;
        coinText.text= coinCount.ToString();    
    }
}
