using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float life;
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
