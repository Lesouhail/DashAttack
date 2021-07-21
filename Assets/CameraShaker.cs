using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DashAttack
{
    public class CameraShaker : MonoBehaviour
    {
        public IEnumerator Shake(float duration, float intensity)
        {
            float timeElapsed = 0;
            float currentIntensity = intensity;
            float decreaseSpeed = intensity / duration;

            while (timeElapsed < duration)
            {
                transform.localPosition = Vector2.zero;
                Vector3 currentPos = transform.position;
                var rnd = Random.insideUnitCircle;

                transform.position = new Vector3(
                    currentPos.x + (rnd.x * currentIntensity),
                    currentPos.y + (rnd.y * currentIntensity),
                    currentPos.z);

                timeElapsed += Time.deltaTime;
                currentIntensity -= decreaseSpeed * Time.unscaledDeltaTime;

                yield return null;
            }

            transform.localPosition = Vector2.zero;
        }
    }
}