using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovements : MonoBehaviour
{
    public static PlayerMovements instance;

    private Vector2 input;
    private Rigidbody rb;

    [SerializeField] private GameObject spaceshipAsset;

     [Header("Audio")]
    [SerializeField] AudioSource playerSound;

    [SerializeField] private Transform missileSpawnPoint;
    [SerializeField] private Transform[] laserSpawnPoints;
    private List<Transform> activeLaserSpawnPoint = new List<Transform>();
    [SerializeField] private GameObject missile;
    [SerializeField] private GameObject laser;

    [Header ("Game feel")]
    [SerializeField] private float speedSpaceship;
    [SerializeField] private float dragSpaceship;
    [SerializeField] private float massSpaceship;
    [SerializeField] private float upRotation;
    [SerializeField] private float rightRotation;
    [SerializeField] private float timeBetweenMissileShoot;
    [SerializeField] private float timeBetweenLaserShoot;
    [SerializeField] private float missileLevel;

    float missiletimer, laserTimer;
    bool isShootingMissile, isShootingLasers, canShootMissile;

    float timer;
    bool isShooting;
    Vector2 movement;

    private void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        rb.drag = dragSpaceship;
        rb.mass = massSpaceship;
        activeLaserSpawnPoint.Add(laserSpawnPoints[0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LevelUp();
        }

        if (isShootingMissile && missiletimer <= 0 && canShootMissile)
        {
            missiletimer = timeBetweenMissileShoot;
            Instantiate(missile, missileSpawnPoint.position, Quaternion.identity);
        }
        if (isShootingLasers && laserTimer <= 0)
        {
            laserTimer = timeBetweenLaserShoot;
            foreach (Transform laserSpawnPoint in activeLaserSpawnPoint)
            {
                Instantiate(laser, laserSpawnPoint.position, Quaternion.identity);
            }
            
        }

        missiletimer -= Time.deltaTime;
        laserTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        movement = new Vector2((transform.up.x * -input.x) + (transform.right.x * -input.x), (transform.forward.z * input.y) + (transform.right.z * input.y));
        rb.AddForce(movement.normalized * speedSpaceship * Time.fixedDeltaTime * 500, ForceMode.Force);

        if (movement.x < 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(-rightRotation, 90, 0);
        }
        else if (movement.x > 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(+rightRotation, 90, 0);
        }
        else if (movement.y < 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(0, 90 , upRotation);           
        }
        else if (movement.y > 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(0, 90 , -upRotation);
        }
        else if (movement.x == 0 && movement.y == 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }

    public void playerMovement(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
    public void playerShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isShootingMissile = true;
            isShootingLasers = true;
            playerSound.Play();
            isShooting = true;
        }
            
        if (context.canceled)
        {
            isShootingMissile = false;
            isShootingLasers = false;
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
