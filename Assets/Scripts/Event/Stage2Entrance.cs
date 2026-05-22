using UnityEngine;

public class Stage2Entrance : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneTransitionManager.instance.StartSceneTransition("Stage2");

        }
    }
}
