using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    // Start is called before the first frame update
    private int status;// 0 = 跟隨，1 = 嚇人，2 = 罵人，3=獵殺，-1 = 遊蕩(只有剛開始)
    private float rage;//怒氣值
    public FirstPersonFootStep Player;
    void Start()
    {
        this.status = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Behavior();
    }
    public void CalmDown()
    { //拜拜時call這個function
        this.rage = 25;
    }
    public float GetRage()
    {
        return rage;
    }
    public int GetStatus() { return status; }
    public void SetStatus(int rage)//update隨時更新
    {
        if (rage >= 100) this.status = 3;
        if (rage < 75) this.status = 2;
        if (rage < 50) this.status = 1;
        if (rage < 25) this.status = 0;
    }
    private void RageUp()
    {//在50以上自然增加到100，應該要新增根據實際秒數而非幀數
        if (this.GetRage() >= 50 && this.GetRage() < 100)
            this.rage++;
        rage++;
    }
    public void Kill() //在獵殺模式碰到玩家時call此function
    {
        //if player != 無敵 EndGame()
    }
    public void BeAngry() //當玩家做翻屍體時，增加25怒氣
    {
        this.rage += 25;
    }
    private void Behavior()
    {
        int status = this.GetStatus();
        //this.RageUp();
        Vector3 PlayerPosition = Player.transform.position;
        if (status == 0)//跟隨
        {

        }
        if (status == 1)//嚇人
        {
            int randomValue = Random.Range(0, 3);

            // 根據隨機數輸出對應的字符串
            if (randomValue == 0) //吹氣
            {
                Debug.Log("HU");
            }
            else if (randomValue == 1) //奸笑
            {
                Debug.Log("HEHE");
            }
            else //both
            {
                Debug.Log("HU and HEHE");
            }
        }
        if (status == 2)//罵人
        {

        }
        if (status == 3) //殺人
        {
            Vector3 direction = (PlayerPosition - transform.position).normalized;

            //移速
            float speed = 3f;

            
            transform.position += direction * speed * Time.deltaTime;

            //面相玩家移動
            transform.LookAt(PlayerPosition);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))//
        {
            Debug.Log("touch");
            this.Kill();
        }
    }

}