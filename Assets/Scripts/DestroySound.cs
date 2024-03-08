using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(DestroyItem), 2f);
    }

    void DestroyItem()
    {
        Destroy(gameObject);
    }
}
