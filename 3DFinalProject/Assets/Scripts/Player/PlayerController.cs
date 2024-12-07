using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField]
    private float InvincibleTime = 10f;
    [SerializeField]
    private float PrayTime = 5f;

    private bool canPray;
    private bool isPraying;
    private float prayCountDown;

    private bool isInvincible;
    private float invCountDown;

    // Start is called before the first frame update
    void Start()
    {
        canPray = false;
        isPraying = false;
        prayCountDown = PrayTime;

        isInvincible = false;
        invCountDown = InvincibleTime;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInvincible();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PrayArea")
        {
            canPray = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.name == "PrayArea")
        {
            if (isPraying)
            {
                // start counting down
                prayCountDown -= Time.deltaTime;

                if(prayCountDown <= 0)
                {
                    // pray over
                    // call function here to lower the ghost's anger


                    isPraying = false;
                    prayCountDown = PrayTime;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "PrayArea")
        {
            canPray = false;
            isPraying = false;
            prayCountDown = PrayTime;
        }
    }

    private void CheckInvincible()
    {
        if(isInvincible == true)
        {
            // count down
            invCountDown -= Time.deltaTime;

            if(invCountDown <= 0)
            {
                // inv mode over
                isInvincible = false;
                invCountDown = InvincibleTime;
            }
        }
    }

    // public function here
    // for item controller
    public void SetInvincible()
    {
        isInvincible = true;
        invCountDown = InvincibleTime;
    }

    public bool SetPraying()
    {
        if (canPray)
        {
            isPraying = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    // for ghost to see if player is invincible
    public bool GetIsInvincible()
    {
        return isInvincible;
    }
}
