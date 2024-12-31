using Fungus;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerController Player;
    [SerializeField]
    private GameObject ghostface; // ghost face Prefab
    [SerializeField]
    private Transform cameraTransform; // Camera's Transform
    [SerializeField] 
    private Renderer ghostRenderer; //ghost meshrender
    [SerializeField]
    private GhostAudio ghostAudio;


    [Header("Parameters")]
    [SerializeField]
    private Status status;
    [SerializeField]
    private float rage;
    [SerializeField]
    private float scareCooldown = 5f;  //CD for Yell & Scare
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float savedDistance = 20f;
    [SerializeField]
    private float appearHight = 3f;
    [SerializeField]
    private float rageUp1stStage = 0.5f;
    [SerializeField]
    private float rageUp2ndStage = 1f;

    private float distance;

    private bool isHunting = false;
    private bool isPlayingMusic = false;
    private bool isEnd = false;
  
    private void Start()
    {
        ghostAudio = GetComponent<GhostAudio>();
        StartCoroutine(BehaviorRoutine());
        StartCoroutine(RageUpRoutine());
        StartCoroutine(MoveRoutine());
    }

    private void Update()
    {
        SetStatus(rage);
        SetDistanceToPlayer();
        UpdateVisibility();
        if (GetDistanceToPlayer() < 5f)
        {
            TeleportAwayFromPlayer();
        }
        if (status != Status.Hunt && isHunting)
        {
            StartCoroutine(BehaviorRoutine());
            StartCoroutine(RageUpRoutine());
            StartCoroutine(MoveRoutine());
            isHunting = false;
        }
        if (status == Status.Hunt && !isHunting)
        {
            isHunting = true;
            StopAllCoroutines();
            StartCoroutine(HuntPlayer());
        }
    }

    private void SetStatus(float rage)//update should call
    {
        if (rage >= 100) status = Status.Hunt;
        if (rage < 100) status = Status.YellAt;//50~99
        if (rage < 50) status = Status.Scare;//25~49
        if (rage < 25) status = Status.Follow;//0~24
    }

    private void RageUp()//Incresingly up to 100 when rage > 50
    {
        if(rage >= 25 && rage < 50)
        {
            rage += rageUp1stStage;
        }else if (rage >= 50 && rage < 100)
        {
            rage += rageUp2ndStage;
        }
    }

    private void SetDistanceToPlayer()
    {
        distance = Vector3.Distance(transform.position, Player.transform.position);
    }

    private void TeleportAwayFromPlayer()
    {
        if (status != Status.Hunt)
        {
            Vector3 PlayerPosition = Player.transform.position;
            Vector3 randomDirection = Random.insideUnitSphere.normalized * savedDistance;
            while (randomDirection.y < 3 || randomDirection.y > 5)
            {
                randomDirection = Random.insideUnitSphere.normalized * savedDistance;
            }
            randomDirection.y = appearHight;
            Debug.Log("Teleporte");
            transform.position = PlayerPosition + randomDirection;
        }
    }
    private void UpdateVisibility()
    {
        if (ghostRenderer == null) return;

        // 
        ghostRenderer.enabled = (status == Status.Hunt);
    }

    private IEnumerator BehaviorRoutine()
    {
        while (true)
        {
            switch (status)
            {
                case Status.Follow:
                    isPlayingMusic = false;
                    ghostAudio.StopLooping();
                    break;
                case Status.Scare:
                    ghostAudio.StopLooping();
                    yield return ScarePlayer();
                    break;

                case Status.YellAt:
                    ghostAudio.StopLooping();
                    yield return YellAtPlayer();
                    break;
            }
            yield return null; // wait for next frame
        }

    }

    private IEnumerator ScarePlayer()
    {
        int randomValue = Random.Range(0, 3);
        isPlayingMusic = false;
        if (randomValue == 0)
        {
            ghostAudio.PlayCry();
            Debug.Log("HU");
        }
        else if (randomValue == 1)
        {
            ghostAudio.PlayFlow();
            Debug.Log("HEHE");
        }
        else
        {
            ghostAudio.PlayLaugh();
            Debug.Log("HU and HEHE");
        }

        yield return new WaitForSeconds(scareCooldown); // CD
    }

    private IEnumerator YellAtPlayer()
    {
        int randomValue = Random.Range(0, 3);
        isPlayingMusic = false;
        
        if (randomValue == 0)
        {
            ghostAudio.PlayScream();
            Debug.Log("FK");
        }
        else if (randomValue == 1)
        {
            ghostAudio.PlayBadwords1();
            Debug.Log("GD");
        }
        else
        {
            ghostAudio.PlayBadwords2();
            Debug.Log("FKYM");
        }

        yield return new WaitForSeconds(scareCooldown); // CD
    }

    private IEnumerator HuntPlayer()
    {
        while (status == Status.Hunt)
        {
            Vector3 PlayerPosition = Player.transform.position;
            Vector3 direction = (PlayerPosition - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            transform.LookAt(PlayerPosition);
            if (!isPlayingMusic)
            {
                ghostAudio.PlayOneShot("HzNoise");
                Debug.Log("1000Hz");
                ghostAudio.PlayOneShot("Wind");
                Debug.Log("wind");
                ghostAudio.PlayLooping("Coming");
                isPlayingMusic = true;
            }
            yield return null; // wait for next frame
        }
    }

    private IEnumerator RageUpRoutine()//rage up 1 point / second
    {
        while (true)
        {
            RageUp();
            yield return new WaitForSeconds(1f); 
        }
    }

    
    private IEnumerator MoveRoutine()//Move every 3s
    {
        while (status != Status.Hunt)//except for Hunt
        {
            Vector3 PlayerPosition = Player.transform.position;
            Vector3 randomDirection = Random.insideUnitSphere.normalized * savedDistance;
            while (randomDirection.y < 3 || randomDirection.y > 5)
            {
                randomDirection = Random.insideUnitSphere.normalized * savedDistance;
            }
            randomDirection.y = appearHight;

            transform.position = PlayerPosition + randomDirection; // Move near Player
            SetDistanceToPlayer();

            yield return new WaitForSeconds(3f);
        }

    }
    public enum Status
    {
        Follow = 0,
        Scare = 1,
        YellAt = 2,
        Hunt = 3
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision");
        if (other.gameObject.tag == "Player" && status == Status.Hunt && !Player.GetIsInvincible() && !isEnd)
        {
            isEnd = true;
            Debug.Log("touch");
            Kill();
        }
        else if (other.gameObject.tag == "Player" && status != Status.Hunt)
        {
            TeleportAwayFromPlayer();
            Debug.Log("Teleport because status != Hunt");
        }
        //Debug.Log("Touch");
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && status == Status.Hunt && !Player.GetIsInvincible() && !isEnd)
        {
            Debug.Log("touch");
            Kill();
        }
        else if (other.gameObject.tag == "Player" && status != Status.Hunt)
        {
            TeleportAwayFromPlayer();
            Debug.Log("Teleport because status != Hunt");
        }
        //Debug.Log("Touch");
    }

    public void BeAngry() //Rage up 25 point when Player touch the deadbody
    {
        rage += 25;
    }

    public void Kill() //Call this function when Ghost in status Hunt and touch Player
    {
        /*if (!Player.GetIsInvincible()) //player isn't invincible
        {
            //EndGame();
        }*/
        if (ghostface != null && cameraTransform != null)
        {
            Debug.Log("GameOver");
            Player.GetComponent<PlayerController>().Killed();
            isEnd = true;

            // instantiate prefab in front of camera
            Vector3 spawnPosition = cameraTransform.position - cameraTransform.forward * 1f;
            GameObject spawnedHint = Instantiate(ghostface, spawnPosition, Quaternion.identity);
        }
    }

    public void ShowGhostFace()
    {
        // instantiate prefab in front of camera
        //Vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * 1f;
        //GameObject spawnedHint = Instantiate(ghostface, spawnPosition, Quaternion.identity);
    }

    public void CalmDown()
    {
        if(rage > 50)
        {
            rage = 50;
        }
    }
    public float GetRage()
    {
        return rage;
    }
    public bool IsClose()// for waning hint
    {
        if (distance < 10)
        {
            return true;
        }
        return false;
    }
    public float GetDistanceToPlayer() 
    {
        return distance;
    }
    
    public Status GetStatus()
    { 
        return status; 
    }
}