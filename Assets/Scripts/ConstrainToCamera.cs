using UnityEngine;

public class ConstrainToCamera : MonoBehaviour
{
    private Camera mainCamera;
    private float camWidth, camHeight;
    private float objectWidth, objectHeight;

    [SerializeField] private GameObject ship;

    void Start()
    {
        mainCamera = Camera.main;

        camHeight = 2f * mainCamera.orthographicSize;
        camWidth = camHeight * mainCamera.aspect;

        objectHeight = ship.transform.localScale.y;
        objectWidth = ship.transform.localScale.z;
    }
    void LateUpdate()
    {
        float minX = mainCamera.transform.position.x - camWidth / 2f - objectWidth;
        float maxX = mainCamera.transform.position.x + camWidth / 2f + objectWidth;
        float minY = mainCamera.transform.position.y - camHeight / 2f + objectHeight /2;
        float maxY = mainCamera.transform.position.y + camHeight / 2f - objectHeight /2;

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        transform.position = clampedPosition;
    }
}
