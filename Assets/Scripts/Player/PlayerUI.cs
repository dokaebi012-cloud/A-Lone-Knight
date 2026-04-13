using System;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private PlayerHealth playerHealth;

    public TextMeshProUGUI healthCountUI;
    public TextMeshProUGUI shieldCountUI;

    public GameObject PauseMenu;
    private bool isPaused = false;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }
    public void CheckStatus()
    {
        ChechHealth();
        ChechShieldCount();
        SwitchPause();
    }

    private void SwitchPause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchIsPaused();
        }

        PauseMenu.SetActive(isPaused);
        
        if(isPaused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void SwitchIsPaused()
    {
        isPaused = !isPaused;
    }

    private void ChechHealth()
    {
        if (playerHealth != null)
        {
            healthCountUI.text = $"{playerHealth.health.ToString()}/100";
        }
        else
        {
            Debug.Log("no Playerhealth script");
        }
    }
    private void ChechShieldCount()
    {
        if (playerHealth != null)
        {
            shieldCountUI.text = playerHealth.shieldCount.ToString();
        }
        else
        {
            Debug.Log("no Playerhealth script");
        }
    }
}
