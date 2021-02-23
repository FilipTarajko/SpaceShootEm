using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] Background backgroundPrefab;
    public bool isParent = false;
    public float speed;

    private void Start()
    {
        if (isParent == true)
        {
            Background secondOne = Instantiate(backgroundPrefab, parent.transform);
            secondOne.transform.Translate(new Vector3(0, Screen.height, 0));
            secondOne.speed = this.speed;
        }
    }

    private void Update()
    {
        transform.Translate(0, -speed * Time.deltaTime, 0);
        if (this.transform.position.y <= -Screen.height)
        {
            transform.Translate(new Vector3(0, 2*Screen.height, 0));
        }
    }
}
