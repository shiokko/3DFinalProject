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
    [SerializeField]
    private AudioClip throwBue; // Clip for throw a bue

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
        audioSource.PlayOneShot(corpseFlipClip);
    }

    // Play relaxing breath sound
    public void PlayRelaxingBreath()
    {       
        audioSource.PlayOneShot(relaxingBreathClip);  
    }
    public void PlayThrowBue()
    {
        audioSource.PlayOneShot(throwBue); 
    }
    
}
