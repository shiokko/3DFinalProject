using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    // Start is called before the first frame update
    private int status;// 0 = ���H�A1 = �~�H�A2 = �|�H�A3=�y���A-1 = �C��(�u����}�l)
    private float rage;//����
    private float scareCooldown = 5f;//�~�H&�|�HCD
    public FirstPersonRaycast Player;
    public void CalmDown()//������call�o��function
    { 
        this.rage = 25;
    }
    public float GetRage()
    {
        return rage;
    }
    public int GetStatus() { return status; }
    private void SetStatus(float rage)//update�H�ɧ�s
    {
        if (rage >= 100) this.status = 3;
        if (rage < 100) this.status = 2;//50~99
        if (rage < 50) this.status = 1;//25~49
        if (rage < 25) this.status = 0;//0~24
    }
    private void RageUp()
    {//�b50�H�W�۵M�W�[��100�A���ӭn�s�W�ھڹ�ڬ�ƦӫD�V��
        if (this.rage >= 50 && this.rage < 100)
            this.rage++;
        //rage+=10;
    }
    public void Kill() //�b�y���Ҧ��I�쪱�a��call��function
    {
        /*if (!Player.GetIsInvincible()) //player ���O�L��
        {
            //EndGame();
        }*/
            Debug.Log("GameOver");
        
    }
    public void BeAngry() //���a��½����ɡA�W�[25���
    {
        this.rage += 25;
    }
    private void Start()
    {
        StartCoroutine(BehaviorRoutine()); // �Ұʦ欰�޿��{
        StartCoroutine(RageUpRoutine());  // �C��W�[���
        StartCoroutine(MoveRoutine());
    }

    private void Update()
    {
        this.SetStatus(this.rage); // �����s���A
    }

    private IEnumerator BehaviorRoutine()
    {
        while (true)
        {
            int status = this.GetStatus();
            Vector3 PlayerPosition = Player.transform.position;

            switch (status)
            {
                case 1: // �~�H
                    yield return ScarePlayer();
                    break;

                case 2: // �|�H
                    yield return YellAtPlayer();
                    break;

                case 3: // �y��
                    yield return HuntPlayer();
                    break;
            }

            yield return null; // ���ݤU�@�V
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

        yield return new WaitForSeconds(scareCooldown); // �N�o�ɶ�
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

        yield return new WaitForSeconds(scareCooldown); // �N�o�ɶ�
    }

    private IEnumerator HuntPlayer()
    {
        while (true)
        {
            int status = this.GetStatus();
            if (status == 3) // �y���Ҧ�
            {
                Vector3 PlayerPosition = Player.transform.position;
                Vector3 direction = (PlayerPosition - transform.position).normalized;

                // ���t
                float speed = 3f;

                // �C�@�V�v�����ʨ쪱�a��V
                transform.position += direction * speed * Time.deltaTime;

                // ���V���a����
                transform.LookAt(PlayerPosition);
            }

            yield return null; // ���ݤU�@�V
        }
    }

    private IEnumerator RageUpRoutine()//
    {
        while (true)
        {
            this.RageUp(); // �W�[���
            yield return new WaitForSeconds(1f); // �C�����@��
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))//�ثeplayer��collider tag�٨S�令Player
        {
            Debug.Log("touch");
            this.Kill();
        }
    }
    private IEnumerator MoveRoutine()//�C����
    {
        while (this.status != 3)//�y���Ҧ����~
        {
            Vector3 PlayerPosition = Player.transform.position;
            Vector3 randomDirection = Random.insideUnitSphere.normalized * 20f;
            randomDirection.y = 0; // �O���b������

            transform.position = PlayerPosition + randomDirection; // �����쪱�a����

            yield return new WaitForSeconds(3f); // �C3�����@��
        }
    }
}