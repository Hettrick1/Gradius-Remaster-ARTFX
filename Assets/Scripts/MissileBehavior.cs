using System.Reflection;
using UnityEngine;
using UnityEngine.Windows;

public class MissileBehavior : MonoBehaviour
{
    [SerializeField] private float speedMissile;
    [SerializeField] private float massMissile;

    private Rigidbody rb;

    enum MissileType
    {
        ACCELERATION,
        FORCE,
        IMPULSE,
        VELOCITY
    }

    [SerializeField] private MissileType missileType;
    private ForceMode forceMode;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = massMissile;
        if (missileType == MissileType.ACCELERATION)
        {
            forceMode = ForceMode.Acceleration;
        }
        else if (missileType == MissileType.FORCE)
        {
            forceMode = ForceMode.Force;
        }
        else if (missileType == MissileType.IMPULSE)
        {
            forceMode = ForceMode.Impulse;
        }
        else if (missileType == MissileType.VELOCITY)
        {
            forceMode = ForceMode.VelocityChange;
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject, 2);
    }
    private void FixedUpdate()
    {
        

        rb.AddForce(transform.right * -speedMissile * Time.fixedDeltaTime, forceMode);
    }
}
