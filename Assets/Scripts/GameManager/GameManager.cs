namespace DashAttack.GameManager
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.SceneManagement;

    public class GameManager : MonoBehaviour
    {
        private DashAttackUltimateInputs Inputs { get; set; }

        private void Awake()
        {
            Inputs = new DashAttackUltimateInputs();
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            Inputs.Player.Restart.performed += _ => SceneManager.LoadScene(currentScene);
        }

        private void OnEnable()
        {
            Inputs.Enable();
        }
    }
}