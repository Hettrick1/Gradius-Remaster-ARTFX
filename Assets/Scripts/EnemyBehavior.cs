using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float life;
    [SerializeField] private EnemyFireState fireState;
    [SerializeField] private EnemyMoveState moveState;

    [SerializeField] private float enemyMoveSpeed;
    [SerializeField] private float verticalMoveSpeed;
    [SerializeField] private float enemyShootSpeed;

    public float radius;
    public float rotationSpeed = 10f;

    private float maxHeight;
    private float minHeight;

    private float verticalMoveDirection = 1;

    private Rigidbody rb;

    enum EnemyFireState
    {
        SHOOTLASERS,
        SHOOTMISSILES,
        NOTHING
    }
    enum EnemyMoveState
    {
        LEFT,
        UPDOWN,
        ROUND,
        STATIC
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        maxHeight = transform.position.y + 0.2f;
        minHeight = transform.position.y - 0.2f; 
    }

    private void Update()
    {
        if(fireState == EnemyFireState.SHOOTLASERS)
        {

        }
        if(fireState == EnemyFireState.SHOOTMISSILES)
        {

        }
        
    }
    private void FixedUpdate()
    {
        if (moveState == EnemyMoveState.LEFT)
        {
            rb.AddForce(Vector2.right * enemyMoveSpeed * Time.fixedDeltaTime * 500, ForceMode.Force);
        }
        if (moveState == EnemyMoveState.UPDOWN)
        {
            if (transform.position.y >= maxHeight)
            {
                verticalMoveDirection = -1;
            }
            else if (transform.position.y <= minHeight)
            {
                 verticalMoveDirection = 1;
            }
            rb.velocity += verticalMoveSpeed * verticalMoveDirection * Vector3.up * Time.fixedDeltaTime;
        }
        if (moveState == EnemyMoveState.ROUND)
        {
            float angle = Time.time * rotationSpeed;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            transform.position += new Vector3(x, y, 0) * Time.fixedDeltaTime;
        }
    }

    public void TakeDamage(float damages)
    {
        if (life > damages) 
        {
            life -= damages;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
