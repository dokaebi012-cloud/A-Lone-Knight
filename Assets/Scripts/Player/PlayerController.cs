using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;
    private PlayerUI playerUI;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();    
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();
        playerUI = GetComponent<PlayerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement.HandleMovement();
        playerAttack.Combat();
        playerUI.CheckStatus();

    }
}
