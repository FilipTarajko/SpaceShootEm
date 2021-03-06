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
    //[SerializeField] AudioSource audioSourceMusic;
    [SerializeField] ParticleSystem particleSystemDestroyed;
    [SerializeField] ParticleSystem particleSystemFire1;
    [SerializeField] ParticleSystem particleSystemFire2;
    [SerializeField] GameObject particleSystemDestroyedParent;
    [SerializeField] Vector3 particleSystemDestroyedParentSpeed;
    [SerializeField] SpriteRenderer shipSprite;

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
        //if (!data.boolSettings["PlayMusic"])
        //{
        //    audioSourceMusic.volume = 0;
        //}
        particleSystemFire1.Play();
        particleSystemFire2.Play();
        SetVolumes();
        SetPlayerColor();
    }

    void SetPlayerColor()
    {
        if (Methods.IntToBool(PlayerPrefs.GetInt("CustomPlayerColor")) || !PlayerPrefs.HasKey("CustomPlayerColor"))
        {
            shipSprite.color = new Color(PlayerPrefs.GetFloat("Red"), PlayerPrefs.GetFloat("Green"), PlayerPrefs.GetFloat("Blue"));
            var main = particleSystemDestroyed.main;
            main.startColor = shipSprite.color;
        }
    }

    private void SetVolumes()
    {
        float sfxVolume = data.floatSettings["SfxVolume"];
        audioSourceShoot.volume = data.sfxShootDefault * sfxVolume;
        audioSourceHit.volume = data.sfxHitDefault * sfxVolume;
        audioSourceDestroyed.volume = data.sfxDestroyedDefault * sfxVolume;
    }

    private void Update()
    {
        if (!data.isPaused)
        {
            CheckLimits(null);
            for (int i = data.previousPositions.Length - 1; i > 0; i--)
            {
                data.previousPositions[i] = data.previousPositions[i - 1];
            }
            data.previousPositions[0] = transform.position;
            if (data.health <= 0 && data.isAlive)
            {
                Die();
            }
            if (!data.boolSettings["SwipeMovement"])
            {
                MoveToLastPressedPosition();
            }
            if (!data.isAlive)
            {
                particleSystemDestroyedParent.transform.Translate(particleSystemDestroyedParentSpeed * Time.deltaTime *144f/5);
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
        particleSystemFire1.gameObject.SetActive(false);
        particleSystemFire2.gameObject.SetActive(false);
        HandlePrefs();
        data.isAlive = false;
        spriteParent.SetActive(false);
        healthbar.gameObject.SetActive(false);
        particleSystemDestroyedParentSpeed = (transform.position - data.previousPositions[5]);

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
        if (data.isAlive)
        {
            if (other.CompareTag("Enemy"))
            {
                if (other.TryGetComponent<BasicEnemy>(out var basicEnemy))
                {
                    TakeDamage(basicEnemy.damage);
                    basicEnemy.DealDamage(10);
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
        if (!data.isPaused)
        {
        cursorDelta = previousCursorPosition - Camera.main.ScreenToWorldPoint(eventData.position);
        previousCursorPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        if (data.isAlive)
        {
            transform.Translate(-cursorDelta * data.floatSettings["Sensitivity"]);
        }
        }
    }
}
