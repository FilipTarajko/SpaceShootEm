using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fires : MonoBehaviour
{
    public Data data;
    public float direction = 1;
    public Vector3 scale;

    private void Start()
    {
        scale = transform.localScale;
    }

    void Update()
    {
        if (!data.isPaused)
        {
            if (scale.y >= data.fireMaxYScale)
            {
                direction = -1;
            }
            else if (scale.y <= data.fireMinYScale)
            {
                direction = 1;
            }
            scale.y += direction * Time.deltaTime * data.fireYScaleChangePerSec;
            scale.y = Mathf.Clamp(scale.y, data.fireMinYScale, data.fireMaxYScale);
            transform.localScale = scale;
        }
    }
}
