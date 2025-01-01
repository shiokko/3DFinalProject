using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class ShakeWhileWalk : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    public GameObject Player;

    [Header("Shake Setting")]
    [SerializeField]
    public float ShakeIntensity = 1.0f; // 搖晃強度
    [SerializeField]
    public float ShakeFrequency = 10.0f; // 搖晃頻率
    

    private float shakeOffsetY = 0;

    private float counter;
    private Vector3 lastPosition;
    private Vector3 velocity;

    private Vector3 initialPosition; // 初始位置

    void Start()
    {
        // 初始化位置
        lastPosition = Player.transform.position;
        initialPosition = this.transform.localPosition;
    }

    void Update()
    {
        // 計算速度
        velocity = (Player.transform.position - lastPosition) / Time.deltaTime;
        lastPosition = Player.transform.position;

        // 根據速度計算上下搖晃
        //float shakeAmount = velocity.magnitude * shakeIntensity;
        if (velocity.magnitude != 0)
        {
            counter += ShakeFrequency * Time.deltaTime;
            shakeOffsetY = Mathf.Sin(counter);
        }

        // 僅改變 Y 軸的位置
        this.transform.localPosition = new Vector3(
            initialPosition.x,            // 保持初始 X 軸位置
            initialPosition.y + shakeOffsetY * ShakeIntensity, // 添加 Y 軸搖晃
            initialPosition.z             // 保持初始 Z 軸位置
        );
    }
}

    