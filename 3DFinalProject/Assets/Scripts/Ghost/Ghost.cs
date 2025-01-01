using Fungus;
using sc.terrain.proceduralpainter;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GhostController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerController Player;
    [SerializeField]
    private GameObject GodTempleFloor;
    [SerializeField]
    private GameObject Ghostface; // ghost face Prefab
    [SerializeField]
    private Transform CameraTransform; // Camera's Transform
    [SerializeField] 
    private Renderer GhostRenderer_body; //ghost meshrender
    [SerializeField]
    private Renderer GhostRenderer_pants;
    [SerializeField]
    private Renderer GhostRenderer_top;
    [SerializeField]
    private GhostAudio GhostAudio;


    [Header("Parameters")]
    [SerializeField]
    private Status status;
    [SerializeField]
    private float rage;
    [SerializeField]
    private float OutsideTempleOffset = 10f;
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
    [SerializeField]
    private float GhostFaceDistance = 2f;

    private Vector3[] godTempleArea = new Vector3[2];

    private float distance;

    private bool isHunting = false;
    private bool isPlayingMusic = false;
    private bool isEnd = false;
  
    private void Start()
    {
        CalculateTempleArea();

        GhostAudio = GetComponent<GhostAudio>();
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

    private void CalculateTempleArea()
    {
        // calculate God temple area
        Vector3 centralPoint = GodTempleFloor.transform.position;
        float width = GodTempleFloor.GetComponent<MeshRenderer>().bounds.size.x;
        float length = GodTempleFloor.GetComponent<MeshRenderer>().bounds.size.z;
        float height = GodTempleFloor.GetComponent<MeshRenderer>().bounds.size.y / 2;

        godTempleArea[0] = new Vector3(centralPoint.x - width / 2, GodTempleFloor.transform.position.y + height, centralPoint.z - length / 2);
        godTempleArea[1] = new Vector3(centralPoint.x + width / 2, GodTempleFloor.transform.position.y + height, centralPoint.z + length / 2);
    }

    private bool CheckInGodTemple(Vector3 futurePos)
    {
        // add border offset to avoid spawn in walls, inward borders
        // templeTL(BR) stands for god temple top left(botton right)
        float templeTL_x = godTempleArea[0].x - OutsideTempleOffset;
        float templeTL_z = godTempleArea[0].z - OutsideTempleOffset;
        float templeBR_x = godTempleArea[1].x + OutsideTempleOffset;
        float templeBR_z = godTempleArea[1].z + OutsideTempleOffset;

        if (futurePos.x > templeTL_x && futurePos.z > templeTL_z && futurePos.x < templeBR_x && futurePos.z < templeBR_z)
        {
            return true;
        }
        else
        {
            return false;
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
        if (GhostRenderer_body != null) 
            GhostRenderer_body.enabled = (status == Status.Hunt);
        if (GhostRenderer_pants != null) 
            GhostRenderer_pants.enabled = (status == Status.Hunt);
        if (GhostRenderer_top != null)  
            GhostRenderer_top.enabled = (status == Status.Hunt);
    }

    private IEnumerator BehaviorRoutine()
    {
        while (true)
        {
            switch (status)
            {
                case Status.Follow:
                    isPlayingMusic = false;
                    GhostAudio.StopLooping();
                    break;
                case Status.Scare:
                    GhostAudio.StopLooping();
                    yield return ScarePlayer();
                    break;

                case Status.YellAt:
                    GhostAudio.StopLooping();
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
            GhostAudio.PlayCry();
            Debug.Log("HU");
        }
        else if (randomValue == 1)
        {
            GhostAudio.PlayFlow();
            Debug.Log("HEHE");
        }
        else
        {
            GhostAudio.PlayLaugh();
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
            GhostAudio.PlayScream();
            Debug.Log("FK");
        }
        else if (randomValue == 1)
        {
            GhostAudio.PlayBadwords1();
            Debug.Log("GD");
        }
        else
        {
            GhostAudio.PlayBadwords2();
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
            if (!isPlayingMusic)
            {
                GhostAudio.PlayOneShot("HzNoise");
                Debug.Log("1000Hz");
                GhostAudio.PlayOneShot("Wind");
                Debug.Log("wind");
                GhostAudio.PlayLooping("Coming");
                isPlayingMusic = true;
            }

            // check if future position will be in temple
            if ( !CheckInGodTemple(transform.position + direction * speed * Time.deltaTime) )
            {
                transform.position += direction * speed * Time.deltaTime;
                transform.LookAt(PlayerPosition);
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
        if (Ghostface != null && CameraTransform != null)
        {
            Debug.Log("GameOver");
            Player.GetComponent<PlayerController>().Killed();
            isEnd = true;

            // instantiate prefab in front of camera
            Vector3 spawnPosition = CameraTransform.position - CameraTransform.forward * GhostFaceDistance;
            GameObject spawnedHint = Instantiate(Ghostface, spawnPosition, Quaternion.identity);
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