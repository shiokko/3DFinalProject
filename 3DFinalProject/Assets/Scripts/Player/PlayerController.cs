using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private GameObject GM;
    [SerializeField]
    private GameObject FungusTrigger;
    [SerializeField]
    private GameObject ItemHolder;
    [SerializeField]
    private GameObject DivinationBlockSpawnPoint;
    [SerializeField]
    private GameObject DivinationBlockPrefab;

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

    private bool canAskGod;
    private bool isAskingGod;

    private bool canPurify;
    private bool isPurifying;

    private bool wantEnchanted;
    private int guessedGhostID;

    private Vector3 angularVelocityRange = new Vector3(2, 2, 2);

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

        canAskGod = false;
        isAskingGod = false;

        canPurify = false;
        isPurifying = false;

        wantEnchanted = false;
        guessedGhostID = -1;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInvincible();

        if (Input.GetKeyDown(KeyCode.X))
        {
            wantEnchanted = true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PrayArea")
        {
            canPray = true;
        }
        else if (other.tag == "PurifyArea")
        {
            canPurify = true;
        }
        else if (other.tag == "AskArea")
        {
            canAskGod = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "PrayArea")
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
        else if (other.tag == "PurifyArea")
        {
            if(isPurifying)
            {
                // call end game methods here
                if(guessedGhostID != -1)
                {
                    GM.GetComponent<GameManager>().EndGame(guessedGhostID);

                    isPurifying = false;
                }
            }
        }
        else if (other.tag == "EnchantArea")
        {
            if (wantEnchanted)
            {
                guessedGhostID = other.gameObject.GetComponent<GodID>().g_ID; // ghost id is the same meaning as god id and deadbody id

                // give the wood sword to item holder so camera can see it
                ItemHolder.GetComponent<ItemController>().GetWoodSword();

                wantEnchanted = false;
            }
        }
        else if (other.tag == "AskArea")
        {
            if (isAskingGod)
            {
                // activate fungus related dialogue
                FungusTrigger.GetComponent<FungusTrigger>().BroadCastAsk();

                isAskingGod = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PrayArea")
        {
            canPray = false;
            isPraying = false;
            prayCountDown = PrayTime;
            PrayBar.SetActive(false);
        }
        else if (other.tag == "PurifyArea")
        {
            canPurify = false;
            isPurifying = false;
        }
        else if (other.tag == "AskArea")
        {
            canAskGod = false;
            isAskingGod = false;
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

    public bool SetAskGod()
    {
        if (canAskGod)
        {
            isAskingGod = true;
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


    // for Fungus trigger call to throw  Divination Block
    void AddRandomAngularVelocity(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        rb.angularVelocity = new Vector3(
            Random.Range(-angularVelocityRange.x, angularVelocityRange.x) * 4,
            Random.Range(-angularVelocityRange.y, angularVelocityRange.y) * 10,
            Random.Range(-angularVelocityRange.z, angularVelocityRange.z) * 4
        );
    }
    public void Throw(bool ans)
    {
        GameObject bueA = Instantiate(DivinationBlockPrefab, DivinationBlockSpawnPoint.transform.position + new Vector3(-0.5f, 0, 0), Quaternion.identity);
        GameObject bueB = Instantiate(DivinationBlockPrefab, DivinationBlockSpawnPoint.transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);

        if (ans)
            bueA.GetComponent<DivinationBlockController>().face = 1;
        else
            bueA.GetComponent<DivinationBlockController>().face = 0;
        bueB.GetComponent<DivinationBlockController>().face = 0;

        AddRandomAngularVelocity(bueA);
        AddRandomAngularVelocity(bueB);
    }
}
