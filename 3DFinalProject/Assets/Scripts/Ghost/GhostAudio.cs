using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAudio : MonoBehaviour
{
    // Clips
    public AudioClip Cry = default;
    public AudioClip Laugh = default;
    public AudioClip Scream = default;
    public AudioClip Flow = default;
    public AudioClip Badwords1 = default;
    public AudioClip Badwords2  = default;
    public AudioClip Badwords3  = default;
    public AudioClip Dead = default;

    [SerializeField]
    private AudioSource audioSource = default;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing from this object.");
        }
    }

    
    public void PlayCry()
    {
        audioSource.PlayOneShot(Cry);
    }
    public void PlayDead()
    {
        audioSource.PlayOneShot(Dead);
    }
    
    public void PlayLaugh()
    {
        audioSource.PlayOneShot(Laugh);
    }

    
    public void PlayScream()
    {
        audioSource.PlayOneShot(Scream);
    }

    
    public void PlayFlow()
    {
        audioSource.PlayOneShot(Flow);
    }

    
    public void PlayBadwords1()
    {
        audioSource.PlayOneShot(Badwords1);
    }

    
    public void PlayBadwords2()
    {
        audioSource.PlayOneShot(Badwords2);
    }

    
    public void PlayBadwords3()
    {
        audioSource.PlayOneShot(Badwords3);
    }
}
