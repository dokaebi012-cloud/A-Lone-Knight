using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();    
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement.HandleMovement();
        playerAttack.PerformAttack();
        playerHealth.CheckShieldCount();
    }
}
