namespace DashAttack.GameManager
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.SceneManagement;

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance => instance;
        private static GameManager instance;
        private DashAttackUltimateInputs Inputs { get; set; }

        private void Awake()
        {
            Inputs = new DashAttackUltimateInputs();
            Application.targetFrameRate = 60;

            if (instance == null)
            {
                instance = this;
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

        private void OnEnable()
        {
            Inputs.Enable();
        }
    }
}