
namespace DashAttack.Level
{
    using UnityEngine;
    using UnityEngine.Tilemaps;
    public class CameraColliderTilemap : MonoBehaviour
    {
        void Start()
        {
            GetComponent<TilemapRenderer>().enabled = false;
        }
    }

}
