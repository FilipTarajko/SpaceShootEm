using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthographicSizeAuto : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.orthographicSize = Screen.height / 2;
    }
}
