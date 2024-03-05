using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Start()
    {

    }
    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        if(transform.position.x >= 25)
        {
            transform.position = new Vector3(-15f, 0f, transform.position.z);
        }
    }
}
