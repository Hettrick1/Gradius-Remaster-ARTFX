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

    [SerializeField] private GameObject GameOver;

    [Header ("Game feel")]
    [SerializeField] private float speedSpaceship;
    [SerializeField] private float dragSpaceship;
    [SerializeField] private float massSpaceship;
    [SerializeField] private float upRotation;
    [SerializeField] private float rightRotation;
    [SerializeField] private float timeBetweenMissileShoot;
    [SerializeField] private float timeBetweenLaserShoot;
    [SerializeField] private float missileLevel;
    [SerializeField] private float life;

    float missiletimer, laserTimer;
    bool isShootingMissile, isShootingLasers, canShootMissile, paused;

    Vector2 movement;

    private void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        rb.drag = dragSpaceship;
        rb.mass = massSpaceship;
        activeLaserSpawnPoint.Add(laserSpawnPoints[0]);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        paused = false;
        GameOver.SetActive(false);
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (isShootingMissile && missiletimer <= 0 && canShootMissile)
        {
            missiletimer = timeBetweenMissileShoot;
            Instantiate(missile, missileSpawnPoint.position, Quaternion.identity);
        }
        if (isShootingLasers && laserTimer <= 0)
        {
            laserTimer = timeBetweenLaserShoot;
            playerSound.Play();
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
        movement = new Vector2((Vector3.up.x * input.x) + (Vector3.right.x * -input.x), (transform.forward.z * input.y) + (transform.right.z * -input.y));
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
            spaceshipAsset.transform.rotation = Quaternion.Euler(0, 90, upRotation);
        }
        else if (movement.y > 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(0, 90, -upRotation);
        }
        else if (movement.x == 0 && movement.y == 0)
        {
            spaceshipAsset.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }

    public void playerMovement(InputAction.CallbackContext context)
    {
        if (!paused)
        {
            input = context.ReadValue<Vector2>();
        }
    }
    public void playerShoot(InputAction.CallbackContext context)
    {
        if (context.performed && !paused)
        {
            isShootingMissile = true;
            isShootingLasers = true;
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
        if (missileLevel ==5)
        {
            activeLaserSpawnPoint.Clear();
            activeLaserSpawnPoint.Add(laserSpawnPoints[1]);
            activeLaserSpawnPoint.Add(laserSpawnPoints[2]);
        }
        if (missileLevel == 10)
        {
            activeLaserSpawnPoint.Clear();
            activeLaserSpawnPoint.Add(laserSpawnPoints[0]);
            activeLaserSpawnPoint.Add(laserSpawnPoints[1]);
            activeLaserSpawnPoint.Add(laserSpawnPoints[2]);
        }
        if (missileLevel == 20)
        {
            canShootMissile = true;
        }
    }

    public void SetLife()
    {
        if (life < 5)
        {
            life += 1;
        }
    }
    public void LessLife()
    {
        if (life > 0)
        {
            life -= 1;
        }
        else
        {
            GameOver.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            paused = true;
            gameObject.SetActive(false);
        }
    }
    public void SetPlayerSpeed()
    {
        if (speedSpaceship < 8)
        {
            speedSpaceship += 0.5f;
        }
    }
    public void SetMissileLevel()
    {
        if (timeBetweenLaserShoot > 0.3)
        {
            timeBetweenLaserShoot -= 0.05f;
        }
        if (timeBetweenMissileShoot > 0.5f && canShootMissile)
        {
            timeBetweenMissileShoot -= 0.05f;
        }
        LevelUp();
    }
    public void SetPaused(bool gamePause)
    {
        paused = gamePause;
    }
}
