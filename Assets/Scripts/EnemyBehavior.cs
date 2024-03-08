using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float life;
    [SerializeField] private EnemyFireState fireState;
    [SerializeField] private EnemyMoveState moveState;

    [SerializeField] private float enemyMoveSpeed;
    [SerializeField] private float verticalMoveSpeed;
    [SerializeField] private float enemyShootSpeed;

    [SerializeField] private Transform missileSpawnPoint;
    [SerializeField] private Transform[] laserSpawnPoints;
    private List<Transform> activeLaserSpawnPoint = new List<Transform>();
    [SerializeField] private GameObject missile;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject sound;

    float missiletimer, laserTimer;
    bool canShootMissile;
    [SerializeField] private float timeBetweenMissileShoot;
    [SerializeField] private float timeBetweenLaserShoot;
    [SerializeField] private float missileLevel;

    [SerializeField] private AudioSource takeDamage;

    public float radius;
    public float rotationSpeed = 10f;

    private float maxHeight;
    private float minHeight;

    private float camWidth, camHeight;
    private float objectWidth, objectHeight;

    private float verticalMoveDirection = 1;

    bool hasDied, justSpawned;

    private Rigidbody rb;
    private GameObject spaceshipAsset;
    private GameManager gm;
    private Camera mainCamera;

    enum EnemyFireState
    {
        SHOOTLASERS,
        SHOOTMISSILES,
        NOTHING
    }
    enum EnemyMoveState
    {
        LEFT,
        UPDOWN,
        ROUND,
        STATIC
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = GameManager.instance;
        LevelUp();
        maxHeight = transform.position.y + 0.2f;
        minHeight = transform.position.y - 0.2f;

        spaceshipAsset = transform.GetChild(0).gameObject;

        mainCamera = Camera.main;

        camHeight = 2f * mainCamera.orthographicSize;
        camWidth = camHeight * mainCamera.aspect;

        objectHeight = transform.localScale.y;
        objectWidth = transform.localScale.x;

        activeLaserSpawnPoint.Add(laserSpawnPoints[0]);
        life = gm.GetEnemiesLife();
    }

    private void Update()
    {
        if (fireState == EnemyFireState.SHOOTLASERS)
        {
            if (laserTimer <= 0 && !hasDied)
            {
                laserTimer = timeBetweenLaserShoot;
                foreach (Transform laserSpawnPoint in activeLaserSpawnPoint)
                {
                    Instantiate(laser, laserSpawnPoint.position, Quaternion.identity);
                }

            }
        }
        if(fireState == EnemyFireState.SHOOTMISSILES)
        {
            if (missiletimer <= 0 && canShootMissile && !hasDied)
            {
                missiletimer = timeBetweenMissileShoot;
                Instantiate(missile, missileSpawnPoint.position, Quaternion.identity);
            }
        }
        missiletimer -= Time.deltaTime;
        laserTimer -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (hasDied)
        {
            rb.velocity = Vector3.zero;
        }
        if (justSpawned)
        {
            rb.velocity +=  Vector3.right * enemyMoveSpeed * Time.fixedDeltaTime;
        }

        if (moveState == EnemyMoveState.LEFT)
        {
            rb.AddForce(Vector2.right * enemyMoveSpeed * Time.fixedDeltaTime * 500, ForceMode.Force);
        }
        if (moveState == EnemyMoveState.UPDOWN)
        {
            if (transform.position.y >= maxHeight)
            {
                verticalMoveDirection = -1;
            }
            else if (transform.position.y <= minHeight)
            {
                 verticalMoveDirection = 1;
            }
            rb.velocity += verticalMoveSpeed * verticalMoveDirection * Vector3.up * Time.fixedDeltaTime;
        }
        if (moveState == EnemyMoveState.ROUND)
        {
            float angle = Time.time * rotationSpeed;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            transform.position += new Vector3(x, y, 0) * Time.fixedDeltaTime;
        }

        if (rb.velocity.x < 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(-5, 90, 0);
        }
        else if (rb.velocity.x > 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(5, 90, 0);
        }
        else if (rb.velocity.y < 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(0, 90, 5);
        }
        else if (rb.velocity.y > 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(0, 90, -5);
        }
        else if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
    void LateUpdate()
    {
        float minX = mainCamera.transform.position.x - camWidth / 2f + objectWidth / 2f;
        float maxX = mainCamera.transform.position.x + camWidth / 2f - objectWidth / 2f;
        float minY = mainCamera.transform.position.y - camHeight / 2f + objectHeight / 2f;
        float maxY = mainCamera.transform.position.y + camHeight / 2f - objectHeight / 2f;

        if (transform.position.x > maxX)
        {
            Invoke(nameof(Died), 0.1f);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("PlayerKilled");
            Died();
        }
    }

    public void TakeDamage(float damages)
    {
        if (life > damages) 
        {
            takeDamage.Play();
            life -= damages;
        }
        else
        {
            Died();
        }
    }
    public void LevelUp()
    {
        missileLevel = gm.GetWaveNumber() + 1;
        timeBetweenLaserShoot = gm.GetTimeBetweenLaserShoot();
        timeBetweenMissileShoot = gm.GetTimeBetweenMissileShoot();
        enemyMoveSpeed = gm.GetEnemyMoveSpeed();
        if (missileLevel == 4)
        {
            activeLaserSpawnPoint.Clear();
            activeLaserSpawnPoint.Add(laserSpawnPoints[1]);
            activeLaserSpawnPoint.Add(laserSpawnPoints[2]);
        }
        if (missileLevel == 8)
        {
            activeLaserSpawnPoint.Clear();
            activeLaserSpawnPoint.Add(laserSpawnPoints[0]);
            activeLaserSpawnPoint.Add(laserSpawnPoints[1]);
            activeLaserSpawnPoint.Add(laserSpawnPoints[2]);
        }
        if (missileLevel == 12)
        {
            canShootMissile = true;
        }
    }

    private void Died()
    {
        hasDied = true;
        Instantiate(sound, transform.position, Quaternion.identity);
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
        transform.position = new Vector3(0,0,-1000);
        moveState = EnemyMoveState.STATIC;
    }
    public void Revive()
    {
        hasDied = false;
        life = gm.GetEnemiesLife();
        transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;
    }

    public void ChooseMoveState(int state)
    {
        if (state == 1)
        {
            moveState = EnemyMoveState.LEFT;
        }
        else if (state == 2)
        {
            moveState = EnemyMoveState.UPDOWN;
        }
        else
        {
            moveState = EnemyMoveState.ROUND;
        }
    }
    public void ChooseShootState(int state)
    {
        if (state == 1)
        {
            fireState = EnemyFireState.NOTHING;
        }
        else if (state == 2)
        {
            fireState = EnemyFireState.SHOOTLASERS;
        }
        else
        {
            fireState = EnemyFireState.SHOOTMISSILES;
        }
    }
    public bool HasDied() {  return hasDied; }

    public void SetJustSpawned(bool spawned) {  justSpawned = spawned; }

}
