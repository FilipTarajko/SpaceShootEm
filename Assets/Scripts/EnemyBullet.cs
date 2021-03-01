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
            if (data.CheckIfOut(transform))
            {
                Destroy(this.gameObject);
            }
            else
            {
                transform.Translate(targetVector * speed * Time.deltaTime);
            }
        }
    }

    public void Hit(Player target)
    {
        target.TakeDamage(damage);
        Destroy(this.gameObject);
    }
}
