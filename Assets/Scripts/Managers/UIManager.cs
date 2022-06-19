using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIManager : MonoBehaviour
{

    public Slider slider;
    public Image fill;
    
    
    private void Start()
    {
        
       //slider.value = 1;

    }

    private void Update()
    {
    }

    public void FillSlider()
    {
        fill.fillAmount = 1;
    }
}