namespace DashAttack.Characters
{
    using DashAttack.Characters.Movements.Dash;
    using UnityEngine;

    public class PlayerFeedbacks : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer dashRenderer;
        [SerializeField] private Color noDashColor;

        public PlayerInputs Inputs { get; set; }
        private Color BaseColor { get; set; }

        private void Start()
        {
            BaseColor = dashRenderer.color;
        }

        private void LateUpdate()
        {
            dashRenderer.color = Inputs.CanDash ? BaseColor : noDashColor;
            float direction = Mathf.Sign(Inputs.RunInput);

            if (Inputs.RunInput != 0)
            {
                transform.localScale = new Vector3(
                    Mathf.Abs(transform.localScale.x) * direction,
                    transform.localScale.y,
                    transform.localScale.z);
            }
        }
    }
}