using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Material[] materials;

    private void Start()
    {

    }
    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        if(transform.position.x >= 25)
        {
            TpBackground();
        }
    }

    private void TpBackground()
    {
        GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Length)] ;
        transform.localRotation = Quaternion.Euler(Mathf.RoundToInt(Random.Range(0, 4) * 90), 90, 90);
        transform.position = new Vector3(-15f, 0f, transform.position.z);
    }
}
