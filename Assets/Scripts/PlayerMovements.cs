using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovements : MonoBehaviour
{
    public static PlayerMovements instance;

    private Vector2 input;
    private Rigidbody rb;

    private GameObject spaceshipAssets;

    [SerializeField] private Transform missileSpawnPoint;
    [SerializeField] private GameObject missile;

    [Header ("Game feel")]
    [SerializeField] private float speedSpaceship;
    [SerializeField] private float dragSpaceship;
    [SerializeField] private float massSpaceship;
    [SerializeField] private float upRotation;
    [SerializeField] private float rightRotation;
    [SerializeField] private float timeBetweenShoot;

    [Header("Audio")]
    [SerializeField] AudioSource playerSound;

    float timer;
    bool isShooting;
    Vector2 movement;

    private void Start()
    {
        instance = this;
        spaceshipAssets = transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody>();
        rb.drag = dragSpaceship;
        rb.mass = massSpaceship;
    }

    private void Update()
    {
        if (isShooting && timer <= 0)
        {
            timer = timeBetweenShoot;
            Instantiate(missile, missileSpawnPoint.position, Quaternion.identity);
        }
        timer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        movement = new Vector2((transform.up.x * -input.x) + (transform.right.x * -input.x), (transform.forward.z * input.y) + (transform.right.z * input.y));
        rb.AddForce(movement.normalized * speedSpaceship * Time.fixedDeltaTime * 500, ForceMode.Force);

        if (movement.y < 0)
        {
            spaceshipAssets.transform.rotation = Quaternion.Euler(-rightRotation, 0, 0);
        }
        else if (movement.y > 0)
        {
            spaceshipAssets.transform.rotation = Quaternion.Euler(rightRotation, 0, 0);
        }
        else if (movement.x < 0)
        {
            spaceshipAssets.transform.rotation = Quaternion.Euler(0, 0, upRotation);           
        }
        else if (movement.x > 0)
        {
            spaceshipAssets.transform.rotation = Quaternion.Euler(0, 0, -upRotation);
        }
        else if (movement.x == 0 && movement.y == 0)
        {
            spaceshipAssets.transform.rotation = Quaternion.Euler(0, 0, 0);
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
            playerSound.Play();
            isShooting = true;
        }
            
        if (context.canceled)
        {
            isShooting = false;
        }
    }
}
