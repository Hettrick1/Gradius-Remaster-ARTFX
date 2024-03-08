using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Spawn : MonoBehaviour
{
    //Declare Instance
    public static Pickup_Spawn Instance;

    //Array of Pickups
    public GameObject[] pickups;

    [SerializeField] private float dropRate;

    //Start
    public void Start()
    {
        Instance = this;
    }

    //Spawn Pickup Function
    public void spawnPickup(Vector3 position)
    {
        if (Random.Range(0, 100) <= dropRate)
        {
            Instantiate(pickups[Random.Range(0, pickups.Length)], position, Quaternion.Euler(90,0,0));
        }
    }
}
