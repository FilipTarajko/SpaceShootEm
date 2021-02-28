using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] Data data;
    [SerializeField] GameObject parent;
    [SerializeField] Background backgroundPrefab;
    public bool isParent = false;
    public float speed;

    private void Start()
    {
        transform.localScale *= data.scaling;
        if (isParent == true)
        {
            speed *= data.scaling;
            Background secondOne = Instantiate(backgroundPrefab, parent.transform);
            secondOne.transform.Translate(new Vector3(0, 2400*data.scaling, 0));
            secondOne.speed = speed;
            secondOne.data = data;
        }
    }

    private void Update()
    {
        if (!data)
        {
            BackgroundMovement();
        }
        else if (!data.isPaused)
        {
            BackgroundMovement();
        }
    }
    
    private void BackgroundMovement()
    {
        transform.Translate(0, -speed * Time.deltaTime, 0);
        if (transform.position.y <= -2400*data.scaling)
        {
            transform.Translate(new Vector3(0, 2 * 2400 *data.scaling, 0));
        }
    }
}
