using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class setText_N : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    private float currentSliderValue;
    // Start is called before the first frame update
    void Start()
    {
        set_N();
        currentSliderValue = slider.value;
    }

    public void redoSliderValue()
    {
        slider.value = currentSliderValue;
    }
    public void changeSliderValue()
    {
        currentSliderValue = slider.value;
    }
    public void set_N()
    {
        this.GetComponent<TMP_Text>().text = $"{slider.value.ToString()}*{slider.value.ToString()}";
    }
}
