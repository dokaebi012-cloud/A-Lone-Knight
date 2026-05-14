using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    public PlayerUI playerUI;
    private PlayerHealth playerHealth;

    public TextMeshProUGUI healthCountUI;
    public TextMeshProUGUI shieldCountUI;

    public GameObject PauseMenu;
    public bool isPaused = false;
    public TextMeshProUGUI pauseText;
    public GameObject resumeButton;

    public Slider healthBar;

    // onclick()에SceneTransitionManager를 동적으로 할당하기 위한 필드
    public Button restartButton;
    public Button exitToMenuButton;

    public string restartSceneName;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        healthBar.maxValue = playerHealth.health;
        healthBar.value = playerHealth.health;
        healthBar.minValue = 0;
        isPaused = false;
        // 현재 씬 이름 자동 저장
        restartSceneName = SceneManager.GetActiveScene().name;
        // 버튼이 참조하던 씬 내 오브젝트는 런타임 중 교체되며 참조 상실
        // 따라서 Start() 단계에서 살아있는 SceneTransitionManager를 코드로 참조해 연결해야 한다
        restartButton.onClick.AddListener(() => SceneTransitionManager.instance.StartSceneTransition(restartSceneName));
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

    // 강제로 unpause할 수 있도록 메서드 추가
    public void ForceUnpause()
    {
        isPaused = false;
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
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
