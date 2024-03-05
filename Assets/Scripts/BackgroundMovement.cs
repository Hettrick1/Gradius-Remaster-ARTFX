using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Material[] materials;
    [SerializeField] private float maxPos;
    [SerializeField] private float minPos;

    MeshRenderer mr;
    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mr.material = materials[Random.Range(0, materials.Length)];
        transform.localRotation = Quaternion.Euler(Mathf.RoundToInt(Random.Range(0, 4) * 90), 90, 90);
    }
    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        if(transform.position.x >= maxPos )
        {
            TpBackground();
        }
    }

    private void TpBackground()
    {
        mr.material = materials[Random.Range(0, materials.Length)];
        transform.localRotation = Quaternion.Euler(Mathf.RoundToInt(Random.Range(0, 4) * 90), 90, 90);
        transform.position = new Vector3(minPos, 0f, transform.position.z);
    }
}
