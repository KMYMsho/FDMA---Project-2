using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(int currentValue, int maxValue)
    {
        slider.value = (float) currentValue / maxValue;
        //slider.value = 1;
    }
}
