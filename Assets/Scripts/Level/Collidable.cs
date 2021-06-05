namespace DashAttack.Levels
{
    using System.Collections;
    using DashAttack.Physics;
    using UnityEngine;

    public class Collidable : MonoBehaviour, ICollidable
    {
        public float CollisionDisablingTime { get; private set; } = 1;
        private bool IgnoreCollisions { get; set; }
        private SpriteRenderer Renderer { get; set; }
        private BoxCollider2D Collider { get; set; }

        private void Start()
        {
            Renderer = GetComponent<SpriteRenderer>();
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
            yield return new WaitForSeconds(CollisionDisablingTime);
            Renderer.enabled = true;
            IgnoreCollisions = false;
            Collider.enabled = true;
        }
    }
}