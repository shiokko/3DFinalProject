using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public GameObject Player;
    public enum Status
    {
        Follow = 0,
        Scare = 1,
        YellAt = 2,
        Hunt = 3
    }
    [SerializeField]
    private Status status;
    [SerializeField]
    private float rage;
    [SerializeField]
    private float scareCooldown = 5f;//CD for Yell & Scare
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float savedDistance = 20f;
    private void SetStatus(float rage)//update should call
    {
        if (rage >= 100) status = Status.Hunt;
        if (rage < 100) status = Status.YellAt;//50~99
        if (rage < 50) status = Status.Scare;//25~49
        if (rage < 25) status = Status.Follow;//0~24
    }
    private void RageUp()//Incresingly up to 100 when rage > 50
    {
        if (rage >= 50 && rage < 100)
            rage++;
        //rage += 1;
        Debug.Log(status);
    }
    private void SetDistanceToPlayer()
    {
        distance = Vector3.Distance(transform.position, Player.transform.position);
    }


    private void Start()
    {
        StartCoroutine(BehaviorRoutine()); 
        StartCoroutine(RageUpRoutine());  
        StartCoroutine(MoveRoutine());
    }

    private void Update()
    {
        SetStatus(rage); 
        SetDistanceToPlayer();
        if (GetDistanceToPlayer() < 5f)
        {
            TeleportAwayFromPlayer();
        }
    }

    private IEnumerator BehaviorRoutine()
    {
        while (true)
        {
            Vector3 PlayerPosition = Player.transform.position;

            switch (status)
            {
                case Status.Scare: 
                    yield return ScarePlayer();
                    break;

                case Status.YellAt: 
                    yield return YellAtPlayer();
                    break;
                case Status.Hunt:
                    yield return HuntPlayer();
                    break;
            }

            yield return null; // wait next frame
        }
    }

    private IEnumerator ScarePlayer()
    {
        int randomValue = Random.Range(0, 3);

        if (randomValue == 0)
        {
            Debug.Log("HU");
        }
        else if (randomValue == 1)
        {
            Debug.Log("HEHE");
        }
        else
        {
            Debug.Log("HU and HEHE");
        }

        yield return new WaitForSeconds(scareCooldown); // CD
    }

    private IEnumerator YellAtPlayer()
    {
        int randomValue = Random.Range(0, 3);

        if (randomValue == 0)
        {
            Debug.Log("FK");
        }
        else if (randomValue == 1)
        {
            Debug.Log("GD");
        }
        else
        {
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

            // Look At Player
            transform.LookAt(PlayerPosition);
            

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

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && status == Status.Hunt)
        {
            Debug.Log("touch");
            Kill();
        }
        else if (collision.gameObject.tag == "Player" && status != Status.Hunt)
        {
            TeleportAwayFromPlayer();
            Debug.Log("Teleport because status != Hunt");
        }
        //Debug.Log("Touch");
    }
    private IEnumerator MoveRoutine()//Move every 3s
    {
        while (status != Status.Hunt)//except for Hunt
        {
            Vector3 PlayerPosition = Player.transform.position;
            Vector3 randomDirection = Random.insideUnitSphere.normalized * savedDistance;
            randomDirection.y = Player.transform.position.y;

            transform.position = PlayerPosition + randomDirection; // Move near Player
            SetDistanceToPlayer();

            yield return new WaitForSeconds(3f);
        }
    }
    private void TeleportAwayFromPlayer()
    {
        if (status != Status.Hunt)
        {
            Vector3 PlayerPosition = Player.transform.position;
            Vector3 randomDirection = Random.insideUnitSphere.normalized * savedDistance;
            randomDirection.y = Player.transform.position.y;
            Debug.Log("Teleporte");
            transform.position = PlayerPosition + randomDirection; 
        }   
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
        Debug.Log("GameOver");

    }
    public void CalmDown() //
    {
        rage = 50;
    }
    public float GetRage()
    {
        return rage;
    }
    public Status GetStatus() 
    { 
        return status; 
    }
    public bool IsClose()
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
}