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

        private void Start()
        {
            Renderer = GetComponentInChildren<SpriteRenderer>();
            Light = GetComponentInChildren<Light2D>();
            Collider = GetComponent<BoxCollider2D>();
        }

        public void Collide(GameObject other)
        {
            if (IgnoreCollisions)
            {
                return;
            }

            if (other.CompareTag("Player"))
            {
                StartCoroutine("DisableCollisions");
            }
        }

        private IEnumerator DisableCollisions()
        {
            IgnoreCollisions = true;
            Renderer.enabled = false;
            Collider.enabled = false;
            Light.enabled = false;
            yield return new WaitForSeconds(CollisionDisablingTime);
            Renderer.enabled = true;
            IgnoreCollisions = false;
            Collider.enabled = true;
            Light.enabled = true;
        }
    }
}