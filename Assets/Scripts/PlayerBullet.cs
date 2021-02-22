using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed;
    public double damage;

    void Update()
    {
        if(transform.position.y < Screen.height / 1.5)
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
