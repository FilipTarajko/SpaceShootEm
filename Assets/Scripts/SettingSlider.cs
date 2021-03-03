using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingSlider : MonoBehaviour
{
    public GameObject parent;
    public Slider slider;
    public string conditionKey;
    public bool conditionRequiredValue;
    public TMP_Text textDisplay;
    public float defaultValue;
}
