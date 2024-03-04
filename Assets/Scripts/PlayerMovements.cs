using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovements : MonoBehaviour
{
    public static PlayerMovements instance;

    private Vector2 input;
    private Rigidbody rb;

    private GameObject spaceshipAssets;

    [SerializeField] private GameObject missileSpowanPoint;

    [Header ("Game feel")]
    [SerializeField] private float speedSpaceship;
    [SerializeField] private float dragSpaceship;
    [SerializeField] private float massSpaceship;
    [SerializeField] private float upRotation;
    [SerializeField] private float rightRotation;

    void Start()
    {
        instance = this;
        spaceshipAssets = transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody>();
        rb.drag = dragSpaceship;
        rb.mass = massSpaceship;
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2((transform.up.x * -input.x) + (transform.right.x * -input.x), (transform.forward.z * input.y) + (transform.right.z * input.y));
        rb.AddForce(movement.normalized * speedSpaceship * Time.fixedDeltaTime * 500, ForceMode.Force);

        if (movement.x < 0)
        {
            spaceshipAssets.transform.rotation = Quaternion.Euler(rightRotation, 0, 0);
        }
        else if (movement.x > 0)
        {
            spaceshipAssets.transform.rotation = Quaternion.Euler(-rightRotation, 0, 0);
        }
        else if (movement.y < 0)
        {
            spaceshipAssets.transform.rotation = Quaternion.Euler(0, 0, upRotation);           
        }
        else if (movement.y > 0)
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
        input = context.ReadValue<Vector2>();
    }
}
