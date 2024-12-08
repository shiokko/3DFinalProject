using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    // Start is called before the first frame update
    private int status;// 0 = ���H�A1 = �~�H�A2 = �|�H�A3=�y���A-1 = �C��(�u����}�l)
    private float rage;//����
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
    { //������call�o��function
        this.rage = 25;
    }
    public float GetRage()
    {
        return rage;
    }
    public int GetStatus() { return status; }
    public void SetStatus(int rage)//update�H�ɧ�s
    {
        if (rage >= 76) this.status = 3;
        if (rage < 75) this.status = 2;
        if (rage < 50) this.status = 1;
        if (rage < 25) this.status = 0;
    }
    private void RageUp()
    {//�b50�H�W�۵M�W�[��100
        if (this.GetRage() >= 50 && this.GetRage() < 100)
            this.rage++;
    }
    public void Kill() //�b�y���Ҧ��I�쪱�a��call��function
    {

    }
    public void BeAngry() //���a��½����ɡA�W�[25���
    {
        this.rage += 25;
    }
    private void Behavior()
    {
        int status = this.GetStatus();
        Vector3 PlayerPosition = Player.GetPlayerPosition();
        if (status == 0)//���H
        {

        }
        if (status == 1)//�~�H
        {

        }
        if (status == 2)//�|�H
        {
        }
        if (status == 3) //���H
        {
            Vector3 direction = (PlayerPosition - transform.position).normalized;

            //���t
            float speed = 3f;

            
            transform.position += direction * speed * Time.deltaTime;

            //���۪��a����
            transform.LookAt(PlayerPosition);
        }
    }
}