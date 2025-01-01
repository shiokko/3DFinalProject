using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    // AudioSource component to play sounds
    [SerializeField]
    private AudioSource _AudioSource = default;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip corpseFlipClip; // Clip for corpse flipping sound
    [SerializeField]
    private AudioClip relaxingBreathClip; // Clip for relaxing breath sound
    [SerializeField]
    private AudioClip throwBue; // Clip for throw a bue

    void Start()
    {
        // Ensure AudioSource is assigned
        _AudioSource = GetComponent<AudioSource>();
    }
    void Update()
    {

    }

    // Play corpse flip sound
    public void PlayCorpseFlip()
    {
        _AudioSource.PlayOneShot(corpseFlipClip);
    }

    // Play relaxing breath sound
    public void PlayRelaxingBreath()
    {
        _AudioSource.PlayOneShot(relaxingBreathClip);  
    }
    public void PlayThrowBue()
    {
        _AudioSource.PlayOneShot(throwBue); 
    }
    
}
