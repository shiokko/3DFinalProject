using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    [SerializeField] 
    private GhostController ghost; // 
    [SerializeField] 
    private AudioSource heartbeatAudio; // 
    [SerializeField] 
    private float minPitch = 0.8f; // 
    [SerializeField] 
    private float maxPitch = 2.0f; //
    [SerializeField] 
    private float rageThreshold = 50f; //
    [SerializeField] 
    private float minVolume = 0.5f; // 
    [SerializeField] 
    private float maxVolume = 1.0f; // 

    void Start()
    {
        
    }

    void Update()
    {
        float rage = ghost.GetRage();

        if (rage > rageThreshold)
        {
            if (!heartbeatAudio.isPlaying)
            {
                heartbeatAudio.Play();
            }

            float normalizedRage = Mathf.Clamp01((rage - rageThreshold) / (100f - rageThreshold));
            heartbeatAudio.pitch = Mathf.Lerp(minPitch, maxPitch, normalizedRage);

            heartbeatAudio.volume = Mathf.Lerp(minVolume, maxVolume, normalizedRage);
        }
        else
        {
            if (heartbeatAudio.isPlaying)
            {
                heartbeatAudio.Stop();
            }
        }
    }
}
