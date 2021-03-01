using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameController gameController;
    public Data data;
    public float speed;

    void Start()
    {
        speed *= data.scaling;
        transform.localScale *= data.scaling;
    }
    void Update()
    {
        if (!data.CheckIfOut(transform))
        {
            if (!data.isPaused)
            {
                Movement();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Movement()
    {
        transform.Translate(new Vector3(0, -1, 0) * speed * Time.deltaTime);
    }

    public void Collect()
    {
        data.attackSpeed += data.attackSpeedPerPowerUp;
        Destroy(this.gameObject);
    }
}
