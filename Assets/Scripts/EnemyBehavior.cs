using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float life;
    [SerializeField] private AudioSource takeDamage;
    [SerializeField] private AudioSource blowUp;
    public void TakeDamage(float damages)
    {
        if (life > damages) 
        {
            takeDamage.Play();
            life -= damages;
        }
        else
        {
            blowUp.Play();
            Destroy(gameObject);
        }
    }
}
