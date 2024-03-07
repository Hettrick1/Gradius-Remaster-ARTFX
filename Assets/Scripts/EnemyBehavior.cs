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

    float missiletimer, laserTimer;
    bool canShootMissile;
    [SerializeField] private float timeBetweenMissileShoot;
    [SerializeField] private float timeBetweenLaserShoot;
    [SerializeField] private float missileLevel;

    public float radius;
    public float rotationSpeed = 10f;

    private float maxHeight;
    private float minHeight;

    private float camWidth, camHeight;
    private float objectWidth, objectHeight;

    private float verticalMoveDirection = 1;

    private Rigidbody rb;
    private GameObject spaceshipAsset;
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
        maxHeight = transform.position.y + 0.2f;
        minHeight = transform.position.y - 0.2f;

        spaceshipAsset = transform.GetChild(0).gameObject;

        mainCamera = Camera.main;

        camHeight = 2f * mainCamera.orthographicSize;
        camWidth = camHeight * mainCamera.aspect;

        objectHeight = transform.localScale.y;
        objectWidth = transform.localScale.x;

        activeLaserSpawnPoint.Add(laserSpawnPoints[0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LevelUp();
        }
        if (fireState == EnemyFireState.SHOOTLASERS)
        {
            if (laserTimer <= 0)
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
            if (missiletimer <= 0 && canShootMissile)
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
        print(rb.velocity.x);
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

        if (transform.position.x < minX || transform.position.x > maxX || transform.position.y < minY || transform.position.y > maxY)
        {
            Destroy(gameObject, 0.1f);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("PlayerKilled");
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damages)
    {
        if (life > damages) 
        {
            life -= damages;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void LevelUp()
    {
        missileLevel += 1;
        if (missileLevel == 1)
        {
            activeLaserSpawnPoint.Clear();
            activeLaserSpawnPoint.Add(laserSpawnPoints[1]);
            activeLaserSpawnPoint.Add(laserSpawnPoints[2]);
        }
        if (missileLevel == 2)
        {
            activeLaserSpawnPoint.Clear();
            activeLaserSpawnPoint.Add(laserSpawnPoints[0]);
            activeLaserSpawnPoint.Add(laserSpawnPoints[1]);
            activeLaserSpawnPoint.Add(laserSpawnPoints[2]);
        }
        if (missileLevel == 3)
        {
            canShootMissile = true;
        }
    }

}
