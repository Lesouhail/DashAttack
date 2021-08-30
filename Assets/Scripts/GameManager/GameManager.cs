namespace DashAttack.GameManager
{
    using DashAttack.Characters;

    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        private DashAttackUltimateInputs Inputs { get; set; }

        public Vector2 RespawnPosition { get; set; }

        private void Awake()
        {
            Inputs = new DashAttackUltimateInputs();
            Application.targetFrameRate = 60;

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            Inputs.Player.Restart.performed += _ => Restart();
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Respawn()
        {
            Player.Instance.transform.position = RespawnPosition;
        }

        private void OnEnable()
        {
            Inputs.Enable();
        }
    }
}