using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisapearOverTime : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(Disappear), 2f);
    }

    void Disappear()
    {
        StartCoroutine(ScaleDownOverTime(1f));
    }

    IEnumerator ScaleDownOverTime(float duration)
    {
        float elapsedTime = 0f;
        Vector3 originalScale = transform.localScale;

        while (elapsedTime < duration)
        {
            float scaleFactor = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            transform.localScale = originalScale * scaleFactor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
