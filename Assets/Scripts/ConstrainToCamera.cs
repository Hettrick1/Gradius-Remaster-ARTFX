using UnityEngine;

public class ConstrainToCamera : MonoBehaviour
{
    private Camera mainCamera;
    private float camWidth, camHeight;
    private float objectWidth, objectHeight;
    private Rigidbody rb;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();

        camHeight = 2f * mainCamera.orthographicSize;
        camWidth = camHeight * mainCamera.aspect;

        objectHeight = transform.GetChild(0).localScale.y;
        objectWidth = transform.GetChild(0).localScale.x;
    }

    void LateUpdate()
    {
        float minX = mainCamera.transform.position.x - camWidth / 2f + objectWidth / 2f;
        float maxX = mainCamera.transform.position.x + camWidth / 2f - objectWidth / 2f;
        float minY = mainCamera.transform.position.y - camHeight / 2f + objectHeight / 2f;
        float maxY = mainCamera.transform.position.y + camHeight / 2f - objectHeight / 2f;

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        transform.position = clampedPosition;
    }
}
