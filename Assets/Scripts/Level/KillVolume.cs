
namespace DashAttack.Level
{
    using DashAttack.GameManager;
    using UnityEngine;

    public class KillVolume : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.Respawn();
            }
        }
    }
}