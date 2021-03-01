using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public GameController gameController;
    public Data data;
    public float speed;
    public float damage;

    void Update()
    {
        if (!data.isPaused)
        {
            if (!data.CheckIfOut(transform))
            {
                transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
            }
            else
            {
                Destroy(gameObject);
            }
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
