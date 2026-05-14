using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance { get; private set; }

    public Image panel;
    public float fadeDuration = 1.0f;
    public string nextSceneName;
    private bool isFading = false;
    private PlayerUI playerUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        playerUI = FindAnyObjectByType<PlayerUI>();
        if (playerUI != null)
        {
            Debug.Log("PlayerUI Get");
        }
    }
    public void StartSceneTransition(string sceneName)
    {
        Debug.Log("Scene Transition Called: " + sceneName);

        // 페이드 인/아웃 중이 아닐 때 페이드 인/아웃
        if (!isFading)
        {
            nextSceneName = sceneName;
            StartCoroutine(FadeInAndLoadScene());
        }
    }

    IEnumerator FadeInAndLoadScene()
    {
        isFading = true;
        panel.gameObject.SetActive(true);
        yield return StartCoroutine(FadeImage(0, 1, fadeDuration));
        yield return StartCoroutine(LoadLoadingAndNextScene());
        yield return StartCoroutine(FadeImage(1, 0, fadeDuration));
        isFading = false;
        panel.gameObject.SetActive(false);
    }

    IEnumerator FadeImage(float startAlpha, float endAlpha, float fadeDuration)
    {
        float elapsedTime = 0f;
        Color panelColor = panel.color;

        while (elapsedTime < fadeDuration)
        {

            // 강제 Pause 해제
            if (playerUI != null)
            {
                playerUI.ForceUnpause();
            }

            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            panelColor.a = newAlpha;
            panel.color = panelColor;
            yield return null;
        }

        panelColor.a = endAlpha;
        panel.color = panelColor;
    }

    IEnumerator LoadLoadingAndNextScene()
    {
        AsyncOperation loadingSceneOp = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
        loadingSceneOp.allowSceneActivation = false;

        while (!loadingSceneOp.isDone)
        {
            if (loadingSceneOp.progress >= 0.9f)
            {
                loadingSceneOp.allowSceneActivation = true;
            }
            yield return null;
        }

        Slider loadingSlider = null;
        GameObject sliderObj = GameObject.Find("LoadingSlider");
        if (sliderObj != null)
        {
            loadingSlider = sliderObj.GetComponent<Slider>();
        }

        AsyncOperation nextSceneOp = SceneManager.LoadSceneAsync(nextSceneName);
        while (!nextSceneOp.isDone)
        {
            if (loadingSlider != null)
            {
                loadingSlider.value = nextSceneOp.progress;
            }
            yield return null;
        }
        if (SceneManager.GetSceneByName("LoadingScene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("LoadingScene");
        }
        else
        {
            Debug.Log("Loading Scene not loaded");
        }
    }

    //public void StartGame()
    //{
    //    StartSceneTransition("Stage1");
    //}


}
