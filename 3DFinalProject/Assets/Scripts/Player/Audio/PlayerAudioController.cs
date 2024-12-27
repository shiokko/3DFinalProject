using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    // AudioSource component to play sounds
    [SerializeField]
    private AudioSource audioSource = default;

    // Audio clips for corpse flipping and relaxing breathing
    [SerializeField]
    private AudioClip corpseFlipClip; // Clip for corpse flipping sound
    [SerializeField]
    private AudioClip relaxingBreathClip; // Clip for relaxing breath sound

    void Start()
    {
        // Ensure AudioSource is assigned
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {

    }

    // Play corpse flip sound
    public void PlayCorpseFlip()
    {
        if (corpseFlipClip != null)
        {
            audioSource.PlayOneShot(corpseFlipClip);
        }
    }

    // Play relaxing breath sound
    public void PlayRelaxingBreath()
    {
        if (relaxingBreathClip != null)
        {
            audioSource.PlayOneShot(relaxingBreathClip);
        }
    }
    
}
