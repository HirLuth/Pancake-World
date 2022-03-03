using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = transform.localPosition.x + Random.Range(-1f, 1f) * magnitude;
            float y = transform.localPosition.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, transform.position.z);

            elapsed += Time.deltaTime;
            
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
