using NUnit.Framework;
using UnityEngine;

public class TutorialEvent : MonoBehaviour
{
    public GameObject[] tutorialUI;

    private void Start()
    {
        foreach (GameObject go in tutorialUI)
        {
            go.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && tutorialUI != null)
        {
            foreach (GameObject go in tutorialUI)
            {
                go.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player" && tutorialUI != null)
        {
            foreach (GameObject go in tutorialUI)
            {
                if(go != null)
                {
                    if (go.CompareTag("Slime"))
                    {
                        return;
                    }
                    go.SetActive(false);
                }
            }
        }
    }
}
