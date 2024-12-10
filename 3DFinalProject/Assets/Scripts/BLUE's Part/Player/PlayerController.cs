using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

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

    // for ghost to see if player is invincible
    public bool GetIsInvincible()
    {
        return isInvincible;
    }
}
