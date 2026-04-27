using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private PlayerHealth playerHealth;

    public TextMeshProUGUI healthCountUI;
    public TextMeshProUGUI shieldCountUI;

    public GameObject PauseMenu;
    private bool isPaused = false;
    public TextMeshProUGUI pauseText;
    public GameObject resumeButton;

    public Slider healthBar;

    // onclick()에SceneTransitionManager를 동적으로 할당하기 위한 필드
    public Button restartButton;
    public Button exitToMenuButton;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        healthBar.maxValue = playerHealth.health;
        healthBar.value = playerHealth.health;
        healthBar.minValue = 0;

        // SceneTransitionManager는 씬 시작 시 새로 생성되며 DontDestroyOnLoad 설정으로 유지되므로,
        // 버튼이 참조하던 씬 내 오브젝트는 런타임 중 교체되며 참조 상실
        // 따라서 Start() 단계에서 살아있는 SceneTransitionManager를 코드로 참조해 연결해야 한다
        restartButton.onClick.AddListener(() => SceneTransitionManager.instance.StartSceneTransition("Stage1"));
        exitToMenuButton.onClick.AddListener(() => SceneTransitionManager.instance.StartSceneTransition("Menu"));
    }
    public void CheckStatus()
    {
        CheckHealth();
        ChechShieldCount();
        SwitchPause();
    }

    private void SwitchPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchIsPaused();
        }

        PauseMenu.SetActive(isPaused);

        if (isPaused)
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

    private void CheckHealth()
    {
        if (playerHealth != null)
        {
            if (!playerHealth.isAlive)
            {
                pauseText.text = "You Died";
                resumeButton.SetActive(false);
            }
            healthCountUI.text = $"{playerHealth.health.ToString()}/100";
            healthBar.value = playerHealth.health;
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
