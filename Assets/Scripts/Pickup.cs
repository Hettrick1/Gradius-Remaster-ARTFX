using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    //START
    void Start()
    {

    }

    //COLLISION PLAYER
    void OnCollisionEnter(Collision Player)
    {
       if (Player.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    //PICKUP TYPES
    enum pickupEffect
    {
        Life,           //0
        Speed,          //1
        ChainReaction   //2
    }
}
