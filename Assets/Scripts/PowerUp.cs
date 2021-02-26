using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameController gameController;
    public Data data;
    public float speed = 200;

    void Start()
    {
        
    }

    private void Movement()
    {
        transform.Translate(new Vector3(0, -1, 0) * speed * Time.deltaTime);
    }

    public void Collect()
    {
        data.attackSpeed += 1;
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!data.isPaused)
        {
            Movement();
        }
    }
}
