namespace DashAttack.Levels
{
    using System.Collections;
    using DashAttack.Physics;
    using UnityEngine;
    using UnityEngine.Experimental.Rendering.Universal;

    public class Collidable : MonoBehaviour, ICollidable
    {
        [SerializeField] private float flashDuration = 0.15f;
        [SerializeField] private float maxFlashIntensity = 1;
        [SerializeField] private float maxFlashRadius = 2;
        [SerializeField] private float freezeFrameDuration = 0.1f;
        [SerializeField] private float freezeFrameModifier = 0.2f;

        public float CollisionDisablingTime { get; private set; } = 1;
        private bool IgnoreCollisions { get; set; }
        private SpriteRenderer Renderer { get; set; }
        private Light2D Light { get; set; }
        private BoxCollider2D Collider { get; set; }
        private ParticleSystem ParticleSystem { get; set; }

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
            Time.timeScale = freezeFrameModifier;
            yield return new WaitForSecondsRealtime(freezeFrameDuration);
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

            var intensitySpeed = maxFlashIntensity / flashDuration;
            var radiusSpeed = maxFlashRadius / flashDuration;

            Light.intensity = 0;
            Light.pointLightOuterRadius = 0;

            var timer = 0f;
            while (timer < flashDuration)
            {
                Light.intensity += intensitySpeed * Time.deltaTime;
                Light.pointLightOuterRadius += radiusSpeed * Time.deltaTime;
                timer += Time.unscaledDeltaTime;

                yield return null;
            }

            Light.enabled = false;
            Time.timeScale = 1;

            yield return new WaitForSeconds(CollisionDisablingTime - flashDuration);

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