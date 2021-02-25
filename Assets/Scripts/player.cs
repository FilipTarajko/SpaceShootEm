using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] InputLayer inputLayer;
    [SerializeField] GameController gameController;
    [SerializeField] Data data;
    Vector3 previousCursorPosition;
    Vector3 cursorDelta;
    public float toplimit;
    public float sidelimit;
    public float bottomlimit;
    public float attackSpeed;
    public PlayerBullet bullet;
    public GameObject dynamic;

    [SerializeField] Healthbar healthbar;

    private void Start()
    {
        data.isAlive = true;
        healthbar.SetMaxHealth(data.maxHealth);
        healthbar.SetHealth(data.health);
        StartCoroutine(ShootingCoroutine());
        inputLayer.OnBeginDragAction += StartOffsetMovement;
        inputLayer.OnDragAction += DoOffsetMovement;
        inputLayer.OnDragAction += MouseFollowMovement;
    }

    private void Update()
    {
        CheckLimits(null);
        if (data.health <= 0 && data.isAlive)
        {
            Die();
        }
    }

    private void Die()
    {
        HandlePrefs();
        data.isAlive = false;
        StartCoroutine(gameController.Death()); 
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
        if (other.tag.Equals("Enemy"))
        {
            if (other.TryGetComponent<BasicEnemy>(out var basicEnemy))
            {
                TakeDamage(basicEnemy.damage);
                Destroy(other.gameObject);
            }
        }
    }

    private void TakeDamage(float enemyDamage)
    {
        data.health -= enemyDamage;
        //if (data.boolSettings["FlashOnDamage"])
        //{
            gameController.redFlash.Flash(0.3f);
        //}
        if (data.boolSettings["Vibration"])
        {
            Vibration.Vibrate(data.vibrationDuration);
        }
        healthbar.SetHealth(data.health);
    }

    public void GetHealing(float healingAmount)
    {
        if (data.health < data.maxHealth && data.isAlive)
        {
            data.health = System.Math.Min(data.health + healingAmount, data.maxHealth);
        }
        healthbar.SetHealth(data.health);
    }

    IEnumerator ShootingCoroutine()
    {
        // int c=0;
        for (;;) // (bool yes = false;!(-(-(-System.Math.Sqrt(5)))>-2);c++) // the perfectest possible loop in existence
        {
            // if (c != 1000) { //surprise mechanic
            yield return new WaitForSeconds(1 / attackSpeed);
            if (data.isAlive)
            {
                Shoot();
            }
                // print(yes = !yes); // works as intended
            //}
        }
    }

    private void Shoot()
    {
        //print("Shot shot");
        PlayerBullet spawnedBullet = Instantiate(bullet, this.transform);
        SetBulletVariables(spawnedBullet);
    }

    private void SetBulletVariables(PlayerBullet spawnedBullet)
    {
        spawnedBullet.transform.Translate(new Vector3(0, 0, +1));
        spawnedBullet.transform.SetParent(dynamic.transform);
        spawnedBullet.speed = data.bulletSpeed;
        spawnedBullet.damage = data.damage;
        spawnedBullet.data = data;
        spawnedBullet.gameController = gameController;
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
        if (!data.boolSettings["SwipeMovement"] && data.isAlive)
        {
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;
            transform.position = targetPos;
        }
    }

    void StartOffsetMovement(PointerEventData eventData)
    {
        if (data.boolSettings["SwipeMovement"] && data.isAlive)
        {
            previousCursorPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        }
    }

    void DoOffsetMovement(PointerEventData eventData)
    {
        if (data.boolSettings["SwipeMovement"] && data.isAlive)
        {
            cursorDelta = previousCursorPosition - Camera.main.ScreenToWorldPoint(eventData.position);
            previousCursorPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            transform.Translate(-cursorDelta * data.floatSettings["Sensitivity"]);
        }
    }
}
