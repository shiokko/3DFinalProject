using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private GameObject Camera;
    [SerializeField]
    private GameObject PlayerFollowCamera;
    [SerializeField]
    private GameObject DivinationBlockSpawnPoint;
    [SerializeField]
    private GameObject DivinationBlockPrefab;
    [SerializeField]
    private GameObject CharmFire;
    [SerializeField]
    private PlayerAudioController PlayerAudio;
    [SerializeField]
    private List<GhostController> ghosts;

    [Header("Parameters")]
    [SerializeField]
    private float InvincibleTime = 10f;
    [SerializeField]
    private float PrayTime = 5f;
    [SerializeField]
    private float DeadSpinSpeed = 2f;
    [SerializeField]
    private float DeadWaitForSecond = 0.5f;

    [Header("UI interface")]
    [SerializeField]
    private GameObject PrayBar;

    private bool isKilled = false;

    private bool CanMove;
    private StarterAssetsInputs _input;

    private bool canPray;
    private bool startPraying;
    private float prayCountDown;

    private bool isInvincible;
    private float invCountDown;

    private bool canAskGod;
    private bool startAskGod;
    private bool isAskingGod;

    private bool canPurify;
    private bool startPurifying;

    private bool wantEnchanted;
    private int guessedGhostID;

    private bool firstTouchDeadbody;

    private Vector3 angularVelocityRange = new Vector3(2, 2, 2);

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        CanMove = true;

        canPray = false;
        startPraying = false;
        prayCountDown = PrayTime;
        PrayBar.SetActive(false);

        isInvincible = false;
        invCountDown = InvincibleTime;
        CharmFire.SetActive(false);

        canAskGod = false;
        startAskGod = false;
        isAskingGod = false;

        canPurify = false;
        startPurifying = false;

        wantEnchanted = false;
        guessedGhostID = -1;

        firstTouchDeadbody = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("J");
            PlayerAudio.PlayThrowBue();
        }*/


        if (isKilled)
        {
            _input.move = Vector2.zero;
            return;
        }

        CheckInvincible();

        if (Input.GetKeyDown(KeyCode.X))
        {
            wantEnchanted = true;
        }

        if (!CanMove)
        {
            // lock player movement every frame
            _input.look = Vector2.zero;
            _input.move = Vector2.zero;
        }

        //Debug.Log(transform.rotation.eulerAngles.y);
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
        else if (other.tag == "Deadbody")
        {
            if (firstTouchDeadbody)
            {
                foreach (var ghost in ghosts)
                {
                    if (ghost.gameObject.activeSelf)
                    {
                        Debug.Log("First touch Deadbody");
                        GameObject.Find(ghost.name).GetComponent<GhostController>().BeAngry();
                        break;
                    }
                }
                //GameObject.Find("Ghost").GetComponent<GhostController>().BeAngry();

                firstTouchDeadbody = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "PrayArea")
        {
            if (startPraying)
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
                    foreach (var ghost in ghosts)
                    {
                        if (ghost.gameObject.activeSelf)
                        {
                            GameObject.Find(ghost.name).GetComponent<GhostController>().CalmDown();
                        }
                    }
                    

                    startPraying = false;
                    prayCountDown = PrayTime;
                    PrayBar.SetActive(false);
                }
            }
        }
        else if (other.tag == "PurifyArea")
        {
            if(startPurifying)
            {
                // call end game methods here
                if(guessedGhostID != -1)
                {
                    GM.GetComponent<GameManager>().EndGame(guessedGhostID);

                    startPurifying = false;
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
            if (startAskGod)
            {
                // activate fungus related dialogue
                FungusTrigger.GetComponent<FungusTrigger>().BroadCastAsk();

                startAskGod = false;

                // we are going to reset isAskingGod after the whole fungus message ends
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PrayArea")
        {
            canPray = false;
            startPraying = false;
            prayCountDown = PrayTime;
            PrayBar.SetActive(false);
        }
        else if (other.tag == "PurifyArea")
        {
            canPurify = false;
            startPurifying = false;
        }
        else if (other.tag == "AskArea")
        {
            canAskGod = false;
            startAskGod = false;
            isAskingGod = false;
        }
        else if (other.tag == "Water")
        { 
            GM.GetComponent<GameManager>().GoToGameOverScene();
        }
    }

    private void CheckInvincible()
    {
        if(isInvincible == true)
        {
            // enable UI slide bar
            if(CharmFire.activeSelf == false)
            {
                CharmFire.SetActive(true);
                CharmFire.GetComponent<FireControll>().Init();
                //InvincibleBar.GetComponent<BarController>().InitBar(InvincibleTime, InvincibleTime);
            }

            // count down
            invCountDown -= Time.deltaTime;
            CharmFire.GetComponent<FireControll>().Ignite(InvincibleTime - invCountDown);
            //InvincibleBar.GetComponent<BarController>().SetVal(invCountDown);

            if (invCountDown <= 0)
            {
                // inv mode over
                isInvincible = false;
                invCountDown = InvincibleTime;
                ItemHolder.GetComponent<ItemController>().CharmUsed();
                CharmFire.SetActive(false);
            }
        }
    }

    private IEnumerator DeadRoutine()
    {
        // move the camera to look straight forward
        // disable player follow cam first
        PlayerFollowCamera.SetActive(false);

        // start rotating
        float lastDirection = Camera.transform.rotation.eulerAngles.y;
        float dir = lastDirection;
        
        if(lastDirection >= 180)
        {
            while(Camera.transform.rotation.eulerAngles.y > lastDirection - 180)
            {
                // rotate anti clock wise
                Camera.transform.eulerAngles = new Vector3(0, Camera.transform.eulerAngles.y - DeadSpinSpeed * Time.deltaTime
                                                            , Camera.transform.eulerAngles.z);

                yield return null;
            }
        }
        else
        {
            while (Camera.transform.rotation.eulerAngles.y < lastDirection + 180)
            {
                // rotate clock wise
                Camera.transform.eulerAngles = new Vector3(0, Camera.transform.eulerAngles.y + DeadSpinSpeed * Time.deltaTime
                                                            , Camera.transform.eulerAngles.z);

                yield return null;
            }
        }

        _input.look.x = 0;
        yield return new WaitForSeconds(DeadWaitForSecond);
        
        // call GM to end the game, GM.Win will == false
        GM.GetComponent<GameManager>().GoToGameOverScene();
    }

    // public function here
    // for everyone
    public void SetCanMove()
    {
        CanMove = true;

        // hide mouse and enable screen rotation
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _input.cursorInputForLook = true;
        _input.cursorLocked = true;
    }

    public void ResetCanMove()
    {
        CanMove = false;

        // show mouse and disable screen rotation
        _input.cursorInputForLook = false;
        _input.cursorLocked = false;
        _input.look = Vector2.zero;
        _input.move = Vector2.zero;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public bool Iskilled()
    {
        return isKilled;
    }

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
            startPraying = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SetAskGod()
    {
        if (isAskingGod)
        {
            return false;
        }

        if (canAskGod)
        {
            startAskGod = true;
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
            startPurifying = true;
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

    // for ghost to call when touches player
    public void Killed()
    {
        isKilled = true;

        // disable player movement first
        ResetCanMove();

        // rotate 180 degrees
        StartCoroutine(DeadRoutine());
    }

    // for Fungus trigger call to throw  Divination Block

    public void Throw(bool correct)
    {
        GameObject DivinationBlockA = Instantiate(DivinationBlockPrefab, 
                                                  DivinationBlockSpawnPoint.transform.position + new Vector3(-0.5f, 0, 0), 
                                                  Quaternion.identity);
        GameObject DivinationBlockB = Instantiate(DivinationBlockPrefab, 
                                                  DivinationBlockSpawnPoint.transform.position + new Vector3(0.5f, 0, 0), 
                                                  Quaternion.identity);

        // destroy DivinationBlocks after 10 seconds
        Destroy(DivinationBlockA, 10);
        Destroy(DivinationBlockB, 10);

        if (correct)
            DivinationBlockA.GetComponent<DivinationBlockController>().face = 1;
        else
            DivinationBlockA.GetComponent<DivinationBlockController>().face = 0;

        DivinationBlockB.GetComponent<DivinationBlockController>().face = 0;

        AddRandomAngularVelocity(DivinationBlockA);
        AddRandomAngularVelocity(DivinationBlockB);
    }
    void AddRandomAngularVelocity(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        rb.angularVelocity = new Vector3(
            Random.Range(-angularVelocityRange.x, angularVelocityRange.x) * 4,
            Random.Range(-angularVelocityRange.y, angularVelocityRange.y) * 10,
            Random.Range(-angularVelocityRange.z, angularVelocityRange.z) * 4
        );
    }

    // for fungus trigger to turn off is asking god after the whole dialogue is over
    public void ResetIsAskingGod()
    {
        isAskingGod = false;
    }
}
