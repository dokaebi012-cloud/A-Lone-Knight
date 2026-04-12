using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private PlayerMovement playerMovement;

    public GameObject startLocation;
    public GameObject endLocation;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();   
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        if((transform.position.x >= startLocation.transform.position.x) && (transform.position.x <= endLocation.transform.position.x))
        {
            transform.position = new Vector3(playerMovement.transform.position.x, transform.position.y, 0);
        }
    }
}
