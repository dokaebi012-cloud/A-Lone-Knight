using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    public int attackAnimationIndex;
    public bool isDoingAction = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        attackAnimationIndex = 0;
    }

    public IEnumerator ActionkCooldownByAimation()
    {
        isDoingAction = true;

        yield return null;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        float animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);

        isDoingAction = false;
    }
    public void TriggerAttack()
    {
        if (attackAnimationIndex%3==1)
        {
            animator.SetTrigger("Attack1");
            Debug.Log("Attack1");
        }
        else if (attackAnimationIndex%3==2)
        {
            animator.SetTrigger("Attack2");
            Debug.Log("Attack2");

        }
        else 
        {
            animator.SetTrigger("Attack3");
            Debug.Log("Attack3");

        }

        attackAnimationIndex++;
        attackAnimationIndex %= 3;

    }
    public void Die()
    {
        animator.Play("PlayerDeathAnimation");
    }
    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
    }
    public void SetWalking()
    {
        
    }
    public void SetRunning(bool isRunning)
    {
        animator.SetBool("IsRunning", isRunning);
    }
    public void TriggerBlock()
    {
        animator.SetTrigger("Block");
    }

}
