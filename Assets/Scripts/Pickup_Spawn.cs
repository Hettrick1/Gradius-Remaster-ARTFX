using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Spawn : MonoBehaviour
{
    //Declare Instance
    public static Pickup_Spawn Instance;

    //Array of Pickups
    public GameObject[] pickups;

    //Start
    public void Start()
    {
        Instance = this;
    }

    //Spawn Pickup Function
    public void spawnPickup()
    {
        if (Random.Range(0, 100) <= 30)
        {
            GameObject pickup = pickups[Random.Range(0, pickups.Length)];

        }
    }
}
