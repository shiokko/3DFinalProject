using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField]
    private float InvincibleTime = 10f;
    [SerializeField]
    private float PrayTime = 5f;

    [Header("UI interface")]
    [SerializeField]
    private GameObject InvincibleBar;
    [SerializeField]
    private GameObject PrayBar;

    private bool canPray;
    private bool isPraying;
    private float prayCountDown;

    private bool isInvincible;
    private float invCountDown;

    private bool canPurify;
    private bool isPurifying;

    // Start is called before the first frame update
    void Start()
    {
        canPray = false;
        isPraying = false;
        prayCountDown = PrayTime;
        PrayBar.SetActive(false);

        isInvincible = false;
        invCountDown = InvincibleTime;
        InvincibleBar.SetActive(false);

        canPurify = false;
        isPurifying = false;
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
        else if (other.name == "PurifyArea")
        {
            canPurify = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.name == "PrayArea")
        {
            if (isPraying)
            {
                // enable UI slide bar
                if (PrayBar.activeSelf == false)
                {
                    PrayBar.SetActive(true);
                    PrayBar.GetComponent<BarController>().InitBar(PrayTime, PrayTime);
                }

                // start counting down
                prayCountDown -= Time.deltaTime;
                PrayBar.GetComponent<BarController>().SetVal(prayCountDown);

                if (prayCountDown <= 0)
                {
                    // pray over
                    // call function here to lower the ghost's anger


                    isPraying = false;
                    prayCountDown = PrayTime;
                    PrayBar.SetActive(false);
                }
            }
        }
        else if (other.name == "PurifyArea")
        {
            if(isPurifying)
            {
                // call end game methods here
                Debug.Log("kill the ghost!");
                isPurifying = false;
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
            PrayBar.SetActive(false);
        }
        else if (other.name == "PurifyArea")
        {
            canPurify = false;
            isPurifying = false;
        }
    }

    private void CheckInvincible()
    {
        if(isInvincible == true)
        {
            // enable UI slide bar
            if(InvincibleBar.activeSelf == false)
            {
                InvincibleBar.SetActive(true);
                InvincibleBar.GetComponent<BarController>().InitBar(InvincibleTime, InvincibleTime);
            }

            // count down
            invCountDown -= Time.deltaTime;
            InvincibleBar.GetComponent<BarController>().SetVal(invCountDown);

            if (invCountDown <= 0)
            {
                // inv mode over
                isInvincible = false;
                invCountDown = InvincibleTime;

                InvincibleBar.SetActive(false);
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

    public bool SetPurifying()
    {
        if (canPurify)
        {
            isPurifying = true;
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
