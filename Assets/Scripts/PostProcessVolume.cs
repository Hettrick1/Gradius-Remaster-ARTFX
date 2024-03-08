using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessVolume : MonoBehaviour
{
    public PostProcessVolume v;
    void Start()
    {
        v.enabled = false;
        v.enabled = true;
    }
}
