using System.Reflection;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.GraphicsBuffer;

public class MissileBehavior : MonoBehaviour
{
    [SerializeField] private float speedMissile;
    [SerializeField] private float massMissile;
    [SerializeField] private float damages;

    [SerializeField] private float detectionRange;
    [SerializeField] private float turnRadius;

    private Rigidbody rb;
    private Transform target;

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

        FindTarget();
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            float distance = direction.magnitude;

            Vector3 desiredVelocity = direction * speedMissile;
            Vector3 steeringForce = (desiredVelocity - rb.velocity).normalized * speedMissile * 0.5f;
            rb.AddForce(steeringForce * Time.deltaTime, forceMode);
           
            if (rb.velocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                targetRotation *= Quaternion.Euler(0, 90, 0);

                transform.rotation = targetRotation;
            }
        }
        else
        {
            rb.AddForce(transform.right * -speedMissile * Time.deltaTime, forceMode);
        }
    }
    void LateUpdate()
    {
        float minX = mainCamera.transform.position.x - camWidth / 2f + objectWidth / 2f;
        float maxX = mainCamera.transform.position.x + camWidth / 2f - objectWidth / 2f;
        float minY = mainCamera.transform.position.y - camHeight / 2f + objectHeight / 2f;
        float maxY = mainCamera.transform.position.y + camHeight / 2f - objectHeight / 2f;

        if(transform.position.x < minX || transform.position.x > maxX || transform.position.y < minY || transform.position.y > maxY)
        {
            Destroy(gameObject,0.1f);
        }
    }
    private void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = enemy.transform;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBehavior>().TakeDamage(damages);
            Destroy(gameObject);
        }
    }
}
