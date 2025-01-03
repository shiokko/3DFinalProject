using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class GhostAudio : MonoBehaviour
{
    // Clips
    public AudioClip Cry = default;
    public AudioClip Laugh = default;
    public AudioClip Scream = default;
    public AudioClip Flow = default;
    public AudioClip Badwords1 = default;
    public AudioClip Badwords2 = default;
    public AudioClip Badwords3 = default;
    public AudioClip Dead = default;
    public AudioClip Wind = default;
    public AudioClip HzNoise = default;
    public AudioClip Coming = default;

    // Dictionary to store audio clips
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    [SerializeField]
    private AudioSource audioSource = default;

    private float deadPitch = 2.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing from this object.");
        }

        // Initialize the dictionary with the clips
        audioClips.Add("Cry", Cry);
        audioClips.Add("Laugh", Laugh);
        audioClips.Add("Scream", Scream);
        audioClips.Add("Flow", Flow);
        audioClips.Add("Badwords1", Badwords1);
        audioClips.Add("Badwords2", Badwords2);
        audioClips.Add("Badwords3", Badwords3);
        audioClips.Add("Dead", Dead);
        audioClips.Add("Wind", Wind);
        audioClips.Add("HzNoise", HzNoise);
        audioClips.Add("Coming", Coming);
    }

    // Play looping sound
    public void PlayLooping(string soundName)
    {
        if (audioSource != null && audioClips.ContainsKey(soundName))
        {
            audioSource.clip = audioClips[soundName];
            audioSource.loop = true; // Set to loop
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }

    // Stop looping sound
    public void StopLooping()
    {
        if (audioSource != null && audioSource.loop)
        {
            audioSource.Stop();
            audioSource.loop = false; // Stop looping
        }
    }

    // Play one-shot sound
    public void PlayOneShot(string soundName, float pitch = 1f)
    {
        if (audioSource != null && audioClips.ContainsKey(soundName))
        {

            // Set the pitch and play the sound
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClips[soundName]);

        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }

    // Specific methods for playing sounds
    public void PlayCry() => PlayOneShot("Cry");
    public void PlayLaugh() => PlayOneShot("Laugh");
    public void PlayScream() => PlayOneShot("Scream");
    public void PlayFlow() => PlayOneShot("Flow");
    public void PlayBadwords1() => PlayOneShot("Badwords1");
    public void PlayBadwords2() => PlayOneShot("Badwords2");
    public void PlayBadwords3() => PlayOneShot("Badwords3");
    public void PlayWind() => PlayOneShot("Wind");
    public void PlayHzNoise() => PlayOneShot("HzNoise");
    public void PlayComing() => PlayLooping("Coming");
    public void PlayDead() => PlayOneShot("Dead", deadPitch);
    
}
