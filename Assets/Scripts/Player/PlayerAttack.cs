using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private PlayerAnimation playerAnimation;
    private PlayerAudio playerAudio;

    public GameObject attack1Hitbox;
    public GameObject attack2Hitbox;
    public GameObject attack3Hitbox;

    public LayerMask enemyLayer;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAudio = GetComponent<PlayerAudio>();

        attack1Hitbox = GetComponent<GameObject>();
        attack2Hitbox = GetComponent<GameObject>();
        attack3Hitbox = GetComponent<GameObject>();

    }

    public void PerformAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!playerAnimation.isDoingAction)
            {
                playerAnimation.TriggerAttack();
                //playerAudio.PlaySwordSwing();
                SoundManager.Instance.PlaySFX(SoundManager.SFXType.SwordSwing);
            }
            else
            {
                return;
            }
            StartCoroutine(playerAnimation.ActionkCooldownByAimation());
        }
    }



}
