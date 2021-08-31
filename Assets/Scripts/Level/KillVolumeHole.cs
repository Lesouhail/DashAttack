
namespace DashAttack.Level
{
    using DashAttack.GameManager;
    using UnityEngine;

    public class KillVolume : KillVolumeBase
    {
        private void Start()
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}