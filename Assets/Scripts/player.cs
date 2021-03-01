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
    [SerializeField] PlayerBullet bullet;
    [SerializeField] GameObject playerBulletParent;
    [SerializeField] GameObject spriteParent;
    [SerializeField] Healthbar healthbar;
    private Vector3 previousCursorPosition;
    private Vector3 cursorDelta;
    private Vector3 clickedPosition;
    [SerializeField] AudioSource audioSourceShoot;
    [SerializeField] AudioSource audioSourceHit;
    [SerializeField] AudioSource audioSourceDestroyed;
    [SerializeField] AudioSource audioSourceMusic;
    [SerializeField] ParticleSystem particleSystemDestroyed;
    [SerializeField] ParticleSystem particleSystemFire1;
    [SerializeField] ParticleSystem particleSystemFire2;

    private void Start()
    {
        data.isAlive = true;
        healthbar.SetMaxHealth(data.maxHealth);
        healthbar.SetHealth(data.health);
        StartCoroutine(ShootingCoroutine());
        if (data.boolSettings["SwipeMovement"])
        {
            inputLayer.OnBeginDragAction += StartOffsetMovement;
            inputLayer.OnDragAction += DoOffsetMovement;
        }
        else
        {
            inputLayer.OnPointerDownAction += MouseFollowMovement;
            inputLayer.OnDragAction += MouseFollowMovement;
        }
        transform.localScale *= data.scaling;
        if (!data.boolSettings["PlayMusic"])
        {
            audioSourceMusic.volume = 0;
        }
        particleSystemFire1.Play();
        particleSystemFire2.Play();
    }

    private void Update()
    {
        if (!data.isPaused)
        {
            CheckLimits(null);
            if (data.health <= 0 && data.isAlive)
            {
                Die();
            }
            if (!data.boolSettings["SwipeMovement"])
            {
                MoveToLastPressedPosition();
            }
        }
    }


    private void MoveToLastPressedPosition()
    {
        if (data.isAlive && data.clickedPositionExists)
        {
            Vector3 targetPos = clickedPosition;
            targetPos.z = 0;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime*data.followMovementPerSec*data.scaling); 
        }
    }

    private void Die()
    {
        HandlePrefs();
        data.isAlive = false;
        spriteParent.SetActive(false);
        healthbar.gameObject.SetActive(false);
        particleSystemDestroyed.Play();
        StartCoroutine(gameController.Death());
        if (data.boolSettings["PlaySfx"])
        {
            audioSourceDestroyed.Play();
        }
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
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<BasicEnemy>(out var basicEnemy))
            {
                TakeDamage(basicEnemy.damage);
                Destroy(other.gameObject);
            }
        }
        if (other.CompareTag("PowerUp"))
        {
            if (other.TryGetComponent<PowerUp>(out var powerUp))
            {
                powerUp.Collect();
            }
        }
        if (other.CompareTag("EnemyBullet"))
        {
            if (other.TryGetComponent<EnemyBullet>(out var enemyBullet))
            {
                enemyBullet.Hit(this);
            }
        }
    }

    public void TakeDamage(float enemyDamage)
    {
        data.health -= enemyDamage;
        if (data.boolSettings["RedFlash"])
        {
            gameController.redFlash.Flash(data.redFlashMaxAlpha);
        }
        if (data.boolSettings["Vibration"])
        {
            Vibration.Vibrate(data.vibrationDuration);
        }
        healthbar.SetHealth(data.health);
        if (data.isAlive && data.boolSettings["PlaySfx"])
        {
            audioSourceHit.Play();
        }
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
        for (;;)
        {
            yield return new WaitForSeconds(1f / data.attackSpeed);
            if (data.isAlive)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        //print("Shot shot");
        PlayerBullet spawnedBullet = Instantiate(bullet, this.transform);
        SetBulletVariables(spawnedBullet);
        if (data.boolSettings["PlaySfx"])
        {
            audioSourceShoot.Play();
        }
    }

    private void SetBulletVariables(PlayerBullet spawnedBullet)
    {
        spawnedBullet.transform.Translate(new Vector3(0, 0, +1));
        spawnedBullet.transform.SetParent(playerBulletParent.transform);
        spawnedBullet.speed = data.bulletSpeed;
        spawnedBullet.damage = data.damage;
        spawnedBullet.data = data;
        spawnedBullet.gameController = gameController;
    }

    void CheckLimits(PointerEventData eventData)
    {
        Vector3 targetPosition = transform.position;
        if (data.usePercentLimits)
        {
            targetPosition.x = System.Math.Max(-data.sidelimit, targetPosition.x);
            targetPosition.x = System.Math.Min(data.sidelimit, targetPosition.x);
            targetPosition.y = System.Math.Min(data.toplimit, targetPosition.y);
            targetPosition.y = System.Math.Max(-data.bottomlimit, targetPosition.y);
        }
        else
        {
            targetPosition.x = System.Math.Max(-data.sidepxlimit, targetPosition.x);
            targetPosition.x = System.Math.Min(data.sidepxlimit, targetPosition.x);
            targetPosition.y = System.Math.Min(data.toppxlimit, targetPosition.y);
            targetPosition.y = System.Math.Max(-data.bottompxlimit, targetPosition.y);
        }
        transform.position = targetPosition;
    }

    void MouseFollowMovement(PointerEventData eventData)
    {
        clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        data.clickedPositionExists = true;
    }

    void StartOffsetMovement(PointerEventData eventData)
    {
        previousCursorPosition = Camera.main.ScreenToWorldPoint(eventData.position);
    }

    void DoOffsetMovement(PointerEventData eventData)
    {
        cursorDelta = previousCursorPosition - Camera.main.ScreenToWorldPoint(eventData.position);
        previousCursorPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        transform.Translate(-cursorDelta * data.floatSettings["Sensitivity"]);
    }
}
