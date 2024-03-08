using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class BossBehaviors : MonoBehaviour
{
    [SerializeField] private float timeBetweenMissileShoot;
    [SerializeField] private float timeBetweenLaserShoot;
    [SerializeField] private float timeBetweenShootState;
    [SerializeField] private float missileLevel;
    [SerializeField] private float bossMoveSpeed, verticalMoveSpeed;
    [SerializeField] private BossFireState fireState;

    [SerializeField] private Transform[] laserSpawnPoints;
    [SerializeField] private Transform[] missileSpawnPoints;
    [SerializeField] private GameObject laser, missile;
    [SerializeField] private GameObject[] explosions;

    [SerializeField] private AudioSource takeDamage;

    private List<Transform> activeLaserSpawnPoint = new List<Transform>();

    private Rigidbody rb;
    private GameObject spaceshipAsset;
    private GameManager gm;

    float life, missiletimer, laserTimer, verticalMoveDirection = 1, shootStateTimer,angle;
    bool isExploding, hasDied, justSpawned;

    private float maxHeight;
    private float minHeight;

    int explosionNumber;

    enum BossFireState
    {
        SHOOTLASERS,
        SHOOTDIAGONALS,
        SHOOTMISSILES,
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = GameManager.instance;
        maxHeight = transform.position.y + 0.5f;
        minHeight = transform.position.y - 0.5f;
        spaceshipAsset = transform.GetChild(0).gameObject;
        life = GameManager.instance.GetBossLife();
    }

    void Update()
    {
        if (shootStateTimer <= 0 && !isExploding)
        {
            shootStateTimer = timeBetweenShootState;
            ChooseShootState(Random.Range(1, 4));
        }
        if (fireState == BossFireState.SHOOTLASERS || fireState == BossFireState.SHOOTDIAGONALS)
        {
            if (laserTimer <= 0 && !isExploding)
            {
                if(fireState == BossFireState.SHOOTDIAGONALS)
                {
                    laserTimer = timeBetweenLaserShoot / 1.5f;
                    angle = Mathf.Sin(Time.time) * 40;
                }
                else
                {
                    laserTimer = timeBetweenLaserShoot;
                    angle = 0;
                }
                foreach (Transform laserSpawnPoint in activeLaserSpawnPoint)
                {
                    laserSpawnPoint.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    Instantiate(laser, laserSpawnPoint.position, laserSpawnPoint.localRotation);
                }

            }
        }
        if (fireState == BossFireState.SHOOTMISSILES)
        {
            if (missiletimer <= 0 && !isExploding)
            {
                missiletimer = timeBetweenMissileShoot;
                foreach (Transform missileSpawnPoint in missileSpawnPoints)
                {
                    Instantiate(missile, missileSpawnPoint.position, Quaternion.identity);
                }
            }
        }
        shootStateTimer -= Time.deltaTime;
        missiletimer -= Time.deltaTime;
        laserTimer -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (justSpawned)
        {
            rb.velocity += Vector3.right * bossMoveSpeed * Time.fixedDeltaTime;
        }

        if (transform.position.y >= maxHeight)
        {
            verticalMoveDirection = -1;
        }
        else if (transform.position.y <= minHeight)
        {
            verticalMoveDirection = 1;
        }
        rb.velocity += verticalMoveSpeed * verticalMoveDirection * Vector3.up * Time.fixedDeltaTime;
        if (isExploding)
        {
            rb.velocity = Vector3.zero;
        }
        if (justSpawned)
        {
            rb.velocity += Vector3.right * bossMoveSpeed * Time.fixedDeltaTime;
        }

        if (rb.velocity.x < 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(-3, 90, 0);
        }
        else if (rb.velocity.x > 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(3, 90, 0);
        }
        else if (rb.velocity.y < 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(0, 90, 3);
        }
        else if (rb.velocity.y > 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(0, 90, -3);
        }
        else if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SetJustRevive();
        }
    }
    public void ChooseShootState(int state)
    {
        if (state == 1)
        {
            activeLaserSpawnPoint.Clear();
            activeLaserSpawnPoint.Add(laserSpawnPoints[0]);
            activeLaserSpawnPoint.Add(laserSpawnPoints[1]);
            activeLaserSpawnPoint.Add(laserSpawnPoints[2]);
            fireState = BossFireState.SHOOTLASERS;
        }
        else if (state == 2)
        {
            activeLaserSpawnPoint.Clear();
            activeLaserSpawnPoint.Add(laserSpawnPoints[3]);
            activeLaserSpawnPoint.Add(laserSpawnPoints[4]);
            activeLaserSpawnPoint.Add(laserSpawnPoints[5]);
            fireState = BossFireState.SHOOTDIAGONALS;
        }
        else
        {
            fireState = BossFireState.SHOOTMISSILES;
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

    private void Died()
    {
        explosionNumber = 0;
        isExploding = true;
        Invoke(nameof(SetHasDied), 4f);
        InvokeRepeating(nameof(Explosions), 0f, 1f);
    }

    private void Explosions()
    {
        if(explosionNumber < explosions.Length)
        {
            explosions[explosionNumber].gameObject.SetActive(true);
            explosionNumber++;
        }
    }

    private void SetHasDied()
    {
        hasDied = true;
        Destroy(gameObject, 0.1f);
    }

    public bool GetHasDied()
    {
        return hasDied;
    }
    public void SetJustSpawned(bool spawned)
    {
        justSpawned = spawned;
    }
}
