using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuButtonManager : MonoBehaviour
{
    [Header("Main Buttons")]
    public Button startButton;
    public Button exitToMenuButton;
    public Button optionButton;

    [Header("Option Panel")]
    public GameObject optionPanel;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public TextMeshProUGUI bgmValue;
    public TextMeshProUGUI sfxValue;
    public Button optionPanelCloseButton;

    void Start()
    {
        // SceneTransitionManager는 씬 시작 시 새로 생성되며 DontDestroyOnLoad 설정으로 유지되므로,
        // 버튼이 참조하던 씬 내 오브젝트는 런타임 중 교체되며 참조 상실
        // 따라서 Start() 단계에서 살아있는 SceneTransitionManager를 코드로 참조해 연결해야 한다
        startButton.onClick.AddListener(() => SceneTransitionManager.instance.StartSceneTransition("Stage1"));
        exitToMenuButton.onClick.AddListener(() => ExitGame());
        optionButton.onClick.AddListener(SwitchOptionPanelActivation);
        optionPanelCloseButton.onClick.AddListener(SwitchOptionPanelActivation);

        // 초기화: 옵션창 숨김
        if (optionPanel != null)
        {
            optionPanel.SetActive(false);
        }

        // 슬라이더 초기값 설정
        if (bgmSlider != null)
        {
            bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
            bgmSlider.onValueChanged.AddListener(UpdateBGMVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = SoundManager.Instance.sfxSource.volume;
            sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
        }

        // 텍스트 초기화
        UpdateVolumeTexts();
    }

    void Update()
    {
        // 텍스트 실시간 반영 (슬라이더 이벤트로도 충분하지만, 보조로 유지)
        UpdateVolumeTexts();
    }

    void SwitchOptionPanelActivation()
    {
        if (optionPanel != null)
        {
            optionPanel.SetActive(!optionPanel.activeSelf);
        }
    }

    void UpdateBGMVolume(float value)
    {
        SoundManager.Instance.bgmSource.volume = value;
        UpdateVolumeTexts();
    }

    void UpdateSFXVolume(float value)
    {
        SoundManager.Instance.sfxSource.volume = value;
        UpdateVolumeTexts();
    }

    void UpdateVolumeTexts()
    {
        if (bgmValue != null)
        {
            bgmValue.text = Mathf.RoundToInt(bgmSlider.value * 100).ToString();
        }

        if (sfxValue != null)
        {
            sfxValue.text = Mathf.RoundToInt(sfxSlider.value * 100).ToString();
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif        
    }
}
