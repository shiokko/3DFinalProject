using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // �ޤJ TextMeshPro �R�W�Ŷ�

public class CloseWarning : MonoBehaviour
{
    public GhostController Ghost; // �s���������
    public TextMeshProUGUI WarningText; // TextMeshPro ����r����

    [SerializeField]
    private float warningDistance = 10f; // ĵ�i�Z��

    void Update()
    {
        // �P�_���O�_���񪱮a
        if (Ghost.GetDistanceToPlayer() <= warningDistance)
        {
            ShowWarning(true);
        }
        else
        {
            ShowWarning(false);
        }
    }

    private void ShowWarning(bool isActive)
    {
        if (isActive)
        {
            // �ҥ�ĵ�i��r�ó]�m���e
            WarningText.color = new Color(WarningText.color.r, WarningText.color.g, WarningText.color.b, 1f); // �]�������i��
        }
        else
        {
            // �]�m��r���z���ӫD�T��
            WarningText.color = new Color(WarningText.color.r, WarningText.color.g, WarningText.color.b, 0f); // �]�������z��
        }
    }
}
