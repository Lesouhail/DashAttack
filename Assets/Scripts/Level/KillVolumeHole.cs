
namespace DashAttack.Level
{
    using DashAttack.GameManager;
    using UnityEngine;

    public class KillVolumeHole : KillVolumeBase
    {
        private void Start()
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}