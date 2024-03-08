using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Pickup_Spawn.Instance.spawnPickup(transform.position);
        Invoke(nameof(DestroyItem), 2f);
    }

    void DestroyItem()
    {
        Destroy(gameObject);
    }
}
