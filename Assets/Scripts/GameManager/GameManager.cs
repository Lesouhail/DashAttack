namespace DashAttack.GameManager
{
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        public void OnApplicationQuit()
        {
        }
    }
}