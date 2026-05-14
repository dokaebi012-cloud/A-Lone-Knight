using TMPro;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance {  get; private set; }
    private PlayerUI playerUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    private void Start()
    {
        playerUI = FindAnyObjectByType<PlayerUI>();
    }

    public void IsVictory()
    {
        Invoke("VictoryPause", 3.0f);
    }

    private void VictoryPause()
    {
        playerUI.pauseText.text = "Victory";
        playerUI.resumeButton.SetActive(false);
        playerUI.SwitchIsPaused();

    }
}
