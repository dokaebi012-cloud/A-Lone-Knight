using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip swordSwing;
    public AudioClip jump;
    public AudioClip blockWithShield;
    public AudioClip runFootstep;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySwordSwing()
    {
        audioSource.PlayOneShot(swordSwing);
    }

    public void PlayBoingJump()
    {
        audioSource.PlayOneShot(jump);
    }

    public void PlayBlockWithShield()
    {
        audioSource.PlayOneShot(blockWithShield);
    }
    public void PlayRunFootstep()
    {
        audioSource.PlayOneShot(runFootstep);
    }
}
