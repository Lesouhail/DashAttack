
namespace DashAttack.Level
{
    using DashAttack.GameManager;
    using UnityEngine;

    public class KillVolumeBase : MonoBehaviour
    {
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.Respawn();
            }
        }
    }

}
