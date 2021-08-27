
namespace DashAttack.Level
{
    using DashAttack.GameManager;
    using UnityEngine;

    public class KillVolume : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.Restart();
            }
        }
    }
}