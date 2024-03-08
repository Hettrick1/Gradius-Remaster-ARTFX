using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float damages;

    private Camera mainCamera;

    private float camWidth, camHeight;
    private float objectWidth, objectHeight;

    private Rigidbody rb;

    void Start()
    {
        mainCamera = Camera.main;

        camHeight = 2f * mainCamera.orthographicSize;
        camWidth = camHeight * mainCamera.aspect;

        objectHeight = transform.localScale.y;
        objectWidth = transform.localScale.x;

        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.right * -acceleration;
    }

    void FixedUpdate()
    {
        rb.velocity += transform.right * -acceleration * Time.fixedDeltaTime;
    }

    void LateUpdate()
    {
        float minX = mainCamera.transform.position.x - camWidth / 2f + objectWidth / 2f;
        float maxX = mainCamera.transform.position.x + camWidth / 2f - objectWidth / 2f;
        float minY = mainCamera.transform.position.y - camHeight / 2f + objectHeight / 2f;
        float maxY = mainCamera.transform.position.y + camHeight / 2f - objectHeight / 2f;

        if (transform.position.x < minX || transform.position.x > maxX || transform.position.y < minY || transform.position.y > maxY)
        {
            GetComponent<BoxCollider>().enabled = false;
            Destroy(gameObject, 0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBehavior>().TakeDamage(damages);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("EnemyProjectile"))
        {
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            other.GetComponent<BossBehaviors>().TakeDamage(damages);
            Destroy(gameObject);
        }
    }
}
