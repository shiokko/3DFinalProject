using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // �ޤJ UI �R�W�Ŷ�

public class ShowRage : MonoBehaviour
{
    public GameObject Ghost; // �����C������
    public Text rageText;    // �Ω���ܫ�� UI Text

    private GhostController ghostController; // ���}�����ޥ�

    void Start()
    {
        // �q Ghost �C��������� GhostController �}��
        ghostController = Ghost.GetComponent<GhostController>();
    }

    void Update()
    {
        // �����������
        float currentRage = ghostController.GetRage();

        // ��s Text ���
        rageText.text = "Rage: " + currentRage.ToString("F0"); // F0 �榡�Ƭ��L�p���I
    }
}