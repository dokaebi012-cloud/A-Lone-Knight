using UnityEngine;

public class TutorialEvent : MonoBehaviour
{
    public GameObject tutorialUI;

    private void Start()
    {
        tutorialUI.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            tutorialUI.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.name == "Player")
        {
            tutorialUI.SetActive(false);
        }
    }
}
