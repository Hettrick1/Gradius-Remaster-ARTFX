using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private pickupEffect effect;
    //START
    void Start()
    {

    }

    //COLLISION PLAYER
    void OnCollisionEnter(Collision Player)
    {
       if ( effect == pickupEffect.Life)
       {

       }
        if (effect == pickupEffect.Speed)
        {

        }
        if (effect == pickupEffect.ChainReaction)
        {

        }
        if (effect == pickupEffect.Missile)
        {

        }
        if (effect == pickupEffect.Missile)
        {

        }

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
        ChainReaction,  //2
        Missile,        //3
        Shield          //4
    }
}
