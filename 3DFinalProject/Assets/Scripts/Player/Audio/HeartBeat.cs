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
                heartbeatAudio.Play(); // 如果音效未播放，開始播放
            }

            // 根據怒氣計算音效音調
            float normalizedRage = Mathf.Clamp01((rage - rageThreshold) / (100f - rageThreshold));
            heartbeatAudio.pitch = Mathf.Lerp(minPitch, maxPitch, normalizedRage);

            // 根據怒氣計算音效音量
            heartbeatAudio.volume = Mathf.Lerp(minVolume, maxVolume, normalizedRage);
        }
        else
        {
            if (heartbeatAudio.isPlaying)
            {
                heartbeatAudio.Stop(); // 如果怒氣低於閾值，停止播放
            }
        }
    }
}
