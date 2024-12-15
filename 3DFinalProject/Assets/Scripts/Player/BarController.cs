using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    public void InitBar(float max, float init)
    {
        _slider.value = init;
        _slider.maxValue = max;
    }

    public void SetVal(float val) 
    {
        _slider.value = val;
    }
}
