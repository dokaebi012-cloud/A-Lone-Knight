using UnityEngine;

public class DeadZoneManager : MonoBehaviour
{
    public GameObject RestartPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            collision.transform.position = RestartPosition.transform.position;
        }
    }
}
