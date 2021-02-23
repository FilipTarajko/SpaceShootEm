using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public GameController gameController;
    public Data data;
    public float speed;
    public double damage;

    void Update()
    {
        if(transform.position.y < Screen.height*(0.5+data.entityBorder))
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Enemy"))
        {
            if(other.TryGetComponent<BasicEnemy>(out var basicEnemy))
            {
                basicEnemy.DealDamage(damage);
            }
            Destroy(this.gameObject);
        }
    }
}
