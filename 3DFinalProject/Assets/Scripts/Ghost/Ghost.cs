using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public GameObject Player;
    [SerializeField]
    private int status;// 0 = 跟隨，1 = 嚇人，2 = 罵人，3=獵殺，-1 = 遊蕩(只有剛開始)
    [SerializeField]
    private float rage;//怒氣值
    [SerializeField]
    private float scareCooldown = 5f;//嚇人&罵人CD
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float distance;
    private void SetStatus(float rage)//update隨時更新
    {
        if (rage >= 100) status = 3;
        if (rage < 100) status = 2;//50~99
        if (rage < 50) status = 1;//25~49
        if (rage < 25) status = 0;//0~24
    }
    private void RageUp()
    {//在50以上自然增加到100，應該要新增根據實際秒數而非幀數
        if (rage >= 50 && rage < 100)
            rage++;
        //rage+=10;
    }
    private void SetDistanceToPlayer()
    {
        distance = Vector3.Distance(transform.position, Player.transform.position);
    }


    private void Start()
    {
        StartCoroutine(BehaviorRoutine()); // 啟動行為邏輯協程
        StartCoroutine(RageUpRoutine());  // 每秒增加怒氣
        StartCoroutine(MoveRoutine());
    }

    private void Update()
    {
        SetStatus(rage); // 持續更新狀態
        SetDistanceToPlayer();
    }

    private IEnumerator BehaviorRoutine()
    {
        while (true)
        {
            Vector3 PlayerPosition = Player.transform.position;

            switch (status)
            {
                case 1: // 嚇人
                    yield return ScarePlayer();
                    break;

                case 2: // 罵人
                    yield return YellAtPlayer();
                    break;

                case 3: // 獵殺
                    yield return HuntPlayer();
                    break;
            }

            yield return null; // 等待下一幀
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

        yield return new WaitForSeconds(scareCooldown); // 冷卻時間
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

        yield return new WaitForSeconds(scareCooldown); // 冷卻時間
    }

    private IEnumerator HuntPlayer()
    {
        while (status == 3)
        {
            Vector3 PlayerPosition = Player.transform.position;
            Vector3 direction = (PlayerPosition - transform.position).normalized;

            // 每一幀逐漸移動到玩家方向
            transform.position += direction * speed * Time.deltaTime;

            // 面向玩家移動
            transform.LookAt(PlayerPosition);
            

            yield return null; // 等待下一幀
        }
    }

    private IEnumerator RageUpRoutine()//
    {
        while (true)
        {
            RageUp(); // 增加怒氣
            yield return new WaitForSeconds(1f); // 每秒執行一次
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))//目前player的collider tag還沒改成Player
        {
            Debug.Log("touch");
            Kill();
        }
    }
    private IEnumerator MoveRoutine()//每3秒順移
    {
        while (status != 3)//獵殺模式除外
        {
            Vector3 PlayerPosition = Player.transform.position;
            Vector3 randomDirection = Random.insideUnitSphere.normalized * 20f;
            randomDirection.y = 0; // 保持在水平面

            transform.position = PlayerPosition + randomDirection; // 順移到玩家附近
            SetDistanceToPlayer();

            yield return new WaitForSeconds(3f); // 每3秒順移一次
        }
    }
    public void BeAngry() //當玩家做翻屍體時，增加25怒氣
    {
        rage += 25;
    }
    public void Kill() //在獵殺模式碰到玩家時call此function
    {
        /*if (!Player.GetIsInvincible()) //player 不是無敵
        {
            //EndGame();
        }*/
        Debug.Log("GameOver");

    }
    public void CalmDown()//拜拜時call這個function
    {
        rage = 50;
    }
    public float GetRage()
    {
        return rage;
    }
    public int GetStatus() 
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