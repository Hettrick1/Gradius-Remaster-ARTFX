using System.Reflection;
using UnityEngine;
using UnityEngine.Windows;

public class MissileBehavior : MonoBehaviour
{
    [SerializeField] private float speedMissile;
    [SerializeField] private float massMissile;

    private Rigidbody rb;

    private Camera mainCamera;
    private float camWidth, camHeight;
    private float objectWidth, objectHeight;

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
        mainCamera = Camera.main;

        camHeight = 2f * mainCamera.orthographicSize;
        camWidth = camHeight * mainCamera.aspect;

        objectHeight = transform.localScale.y;
        objectWidth = transform.localScale.x;

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
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        rb.AddForce(transform.right * -speedMissile * Time.fixedDeltaTime, forceMode);
    }
    void LateUpdate()
    {
        float minX = mainCamera.transform.position.x - camWidth / 2f + objectWidth / 2f;
        float maxX = mainCamera.transform.position.x + camWidth / 2f - objectWidth / 2f;
        float minY = mainCamera.transform.position.y - camHeight / 2f + objectHeight / 2f;
        float maxY = mainCamera.transform.position.y + camHeight / 2f - objectHeight / 2f;

        if(transform.position.x < minX || transform.position.x > maxX || transform.position.y < minY || transform.position.y > maxY)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
