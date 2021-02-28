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
        transform.localScale *= Screen.height / 2400f;
        if (isParent == true)
        {
            speed *= Screen.height / 2400f;
            Background secondOne = Instantiate(backgroundPrefab, parent.transform);
            secondOne.transform.Translate(new Vector3(0, 2400 * Screen.height / 2400f, 0));
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
        if (transform.position.y <= -2400* Screen.height / 2400f)
        {
            transform.Translate(new Vector3(0, 2 * 2400 * Screen.height / 2400f, 0));
        }
    }
}
