using UnityEngine;
using UnityEngine.UI;

public class MenuButtonManager : MonoBehaviour
{
    // onclick()에SceneTransitionManager를 동적으로 할당하기 위한 필드
    public Button startButton;
    public Button exitToMenuButton;
    void Start()
    {
        // SceneTransitionManager는 씬 시작 시 새로 생성되며 DontDestroyOnLoad 설정으로 유지되므로,
        // 버튼이 참조하던 씬 내 오브젝트는 런타임 중 교체되며 참조 상실
        // 따라서 Start() 단계에서 살아있는 SceneTransitionManager를 코드로 참조해 연결해야 한다
        startButton.onClick.AddListener(() => SceneTransitionManager.instance.StartSceneTransition("Stage1"));
        exitToMenuButton.onClick.AddListener(() => ExitGame());

    }

    // Update is called once per frame
    void Update()
    {
        
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
