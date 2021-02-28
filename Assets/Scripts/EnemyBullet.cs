using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameController gameController;
    public Data data;
    public float speed;
    public float damage;
    public Vector3 targetVector;
    public float spread;
    public Quaternion rotation;

    private void Start()
    {
        rotation = Quaternion.Euler(0, 0, Random.Range(-spread, +spread));
        targetVector = (gameController.player.transform.position - transform.position).normalized;
        targetVector = rotation * targetVector;
        transform.localScale *= data.scaling;
        speed *= data.scaling;
    }

    void Update()
    {
        if (!data.isPaused)
        {
            if (transform.position.y < Screen.height * (0.5 + data.entityBorder))
            {
                transform.Translate(targetVector * speed * Time.deltaTime);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void Hit(Player target)
    {
        target.TakeDamage(damage);
        Destroy(this.gameObject);
    }
}
