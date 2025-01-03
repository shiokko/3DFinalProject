using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonFootStep : MonoBehaviour
{
    [Header("Foot Step Params")]
    [SerializeField]
    private float baseFootStepSpeed = 0.35f;
    [SerializeField]
    private float walkMultiplyer = 1.6f;

    [Header("Foot Step Audio")]
    [SerializeField]
    private AudioSource FootStepAudioSource = default;
    [SerializeField]
    private AudioClip defaultFootStep = default;

    private float footStepTimer;
    private float curOffset;

    private bool _isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        footStepTimer = 0;

        _isGrounded = GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>().Grounded;

    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>().Grounded;
        if (!_isGrounded) 
        {
            return;
        }

        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            return;
        }

        // give sound
        if (Input.GetKey(KeyCode.LeftShift))
        {
            curOffset = baseFootStepSpeed * walkMultiplyer;
        }
        else
        {
            curOffset = baseFootStepSpeed;
        }

        footStepTimer -= Time.deltaTime;

        if(footStepTimer <= 0)
        {
            FootStepAudioSource.PlayOneShot(defaultFootStep);

            footStepTimer = curOffset;
        }
    }
}
