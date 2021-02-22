using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] InputLayer inputLayer;
    [SerializeField] GameController gameController;
    Vector3 previousCursorPosition;
    Vector3 cursorDelta;
    public float swipeFactor;
    public bool useSwipeMovement;
    public float toplimit;
    public float sidelimit;
    public float bottomlimit;
    public float attackSpeed;
    public PlayerBullet bullet;
    public GameObject dynamic;
    public float bulletSpeed;
    public double damage;
    public double health;

    private void Start()
    {
        StartCoroutine(ShootingCoroutine());
        inputLayer.OnBeginDragAction += StartOffsetMovement;
        inputLayer.OnDragAction += DoOffsetMovement;
        inputLayer.OnDragAction += MouseFollowMovement;
        //inputLayer.OnDragAction += CheckLimits;
    }

    private void Update()
    {
        CheckLimits(null);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        HandlePrefs();
        SceneManager.LoadScene(0);
    }

    private void HandlePrefs()
    {
        PlayerPrefs.SetInt("Last score", gameController.wave);
        if (PlayerPrefs.HasKey("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", System.Math.Max(gameController.wave, PlayerPrefs.GetInt("Highscore")));
        }
        else
        {
            PlayerPrefs.SetInt("Highscore", gameController.wave);
        }
        PlayerPrefs.Save();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Equals("PlayerBullet"))
        {
            Debug.Log($"{this.name} collided with {other.name}");
        }
        if(other.tag.Equals("Enemy"))
        {
            if (other.TryGetComponent<BasicEnemy>(out var basicEnemy))
            {
                DealDamage(basicEnemy.damage);
                Destroy(other.gameObject);
            }
        }
    }

    private void DealDamage(double enemyDamage)
    {
        health -= enemyDamage;
    }

    IEnumerator ShootingCoroutine()
    {
        // int c=0;
        for (;;) // (bool yes = false;!(-(-(-System.Math.Sqrt(5)))>-2);c++) // the perfectest possible loop in existence
        {
            // if (c != 1000) { //surprise mechanic
                yield return new WaitForSeconds(1/attackSpeed);
                Shoot();
                // print(yes = !yes); // works as intended
            //}
        }
    }

    private void Shoot()
    {
        //print("Shot shot");
        PlayerBullet spawnedBullet = Instantiate(bullet, this.transform);
        spawnedBullet.transform.SetParent(dynamic.transform);
        spawnedBullet.speed = bulletSpeed;
        spawnedBullet.damage = damage;
    }

    void CheckLimits(PointerEventData eventData)
    {
        Vector3 targetPosition = transform.position;
        targetPosition.x = System.Math.Max(-sidelimit, targetPosition.x);
        targetPosition.x = System.Math.Min(sidelimit, targetPosition.x);
        targetPosition.y = System.Math.Min(toplimit, targetPosition.y);
        targetPosition.y = System.Math.Max(-bottomlimit, targetPosition.y);
        transform.position = targetPosition;
    }

    void MouseFollowMovement(PointerEventData eventData)
    {
        if (!useSwipeMovement)
        {
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;
            transform.position = targetPos;
        }
    }

    void StartOffsetMovement(PointerEventData eventData)
    {
        if (useSwipeMovement)
        {
            previousCursorPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        }
    }

    void DoOffsetMovement(PointerEventData eventData)
    {
        if (useSwipeMovement)
        {
            cursorDelta = previousCursorPosition - Camera.main.ScreenToWorldPoint(eventData.position);
            previousCursorPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            transform.Translate(-cursorDelta * swipeFactor);
        }
    }
}
