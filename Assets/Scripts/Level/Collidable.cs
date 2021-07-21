namespace DashAttack.Levels
{
    using System.Collections;
    using DashAttack.Physics;
    using UnityEngine;
    using UnityEngine.Experimental.Rendering.Universal;

    public class Collidable : MonoBehaviour, ICollidable
    {
        public float CollisionDisablingTime { get; private set; } = 1;
        private bool IgnoreCollisions { get; set; }
        private SpriteRenderer Renderer { get; set; }
        private Light2D Light { get; set; }
        private BoxCollider2D Collider { get; set; }
        private ParticleSystem ParticleSystem { get; set; }
        private float lightFlashDuration { get; set; } = .3f;

        private void Start()
        {
            Renderer = GetComponentInChildren<SpriteRenderer>();
            Light = GetComponentInChildren<Light2D>();
            Collider = GetComponent<BoxCollider2D>();
            ParticleSystem = GetComponent<ParticleSystem>();
        }

        public void Collide(GameObject other)
        {
            if (IgnoreCollisions)
            {
                return;
            }

            if (other.CompareTag("Player"))
            {
                ParticleSystem.Emit(50);
                StartCoroutine("DisableCollisions");
                StartCoroutine(FreezeFrame());
            }
        }

        private IEnumerator FreezeFrame()
        {
            var baseTimeScale = Time.timeScale;
            Time.timeScale = 0.18f;
            yield return new WaitForSecondsRealtime(0.085f);
            Time.timeScale = baseTimeScale;
        }

        private IEnumerator DisableCollisions()
        {
            IgnoreCollisions = true;
            Renderer.enabled = false;
            Collider.enabled = false;

            var baseIntensity = Light.intensity;
            var baseRadius = Light.pointLightOuterRadius;
            var baseColor = Light.color;

            Light.intensity = 1;
            var lightDecreaseSpeed = Light.color.a / lightFlashDuration;

            var timer = 0f;
            while (timer < lightFlashDuration)
            {
                Light.color = new Color(baseColor.r, baseColor.g, baseColor.b, Light.color.a - (lightDecreaseSpeed * Time.deltaTime));
                Light.pointLightOuterRadius += 2 * Time.deltaTime;
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
            Light.enabled = false;

            Time.timeScale = 1;

            yield return new WaitForSeconds(CollisionDisablingTime - lightFlashDuration);

            Renderer.enabled = true;
            IgnoreCollisions = false;
            Collider.enabled = true;

            Light.color = baseColor;
            Light.intensity = baseIntensity;
            Light.pointLightOuterRadius = baseRadius;
            Light.enabled = true;
        }
    }
}