namespace DashAttack.Characters
{
    using System.Collections;
    using DashAttack.Characters.Movements.Dash;
    using DashAttack.Characters.Movements.Vertical;
    using UnityEngine;

    using static DashAttack.Utility.StateCallBack;

    public class PlayerFeedbacks : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer dashRenderer;
        [SerializeField] private Color noDashColor;
        [SerializeField] private float squashTime;
        [SerializeField] private float squashDepth;

        public PlayerInputs Inputs { get; set; }
        private Color BaseColor { get; set; }

        private VerticalMovement VerticalMovement { get; set; }

        private float squashCounter { get; set; }
        private float squashSpeed => squashDepth / squashTime;

        private IEnumerator LandSquash { get; set; }
        private IEnumerator JumpStretch { get; set; }

        private void Start()
        {
            VerticalMovement = GetComponentInParent<VerticalMovement>();
            BaseColor = dashRenderer.color;

            VerticalMovement.Subscribe(VerticalState.Rising, OnStateEnter, () =>
            {
                bool isSquash = VerticalMovement.PreviousState != VerticalState.WallSliding;

                if (JumpStretch != null)
                {
                    StopCoroutine(JumpStretch);
                }
                if (LandSquash != null)
                {
                    StopCoroutine(LandSquash);
                }

                transform.localScale = Vector3.one;
                JumpStretch = Strech(isSquash);
                StartCoroutine(JumpStretch);
            });

            VerticalMovement.Subscribe(VerticalState.WallSliding, OnStateEnter, () =>
            {
                if (LandSquash != null)
                {
                    StopCoroutine(LandSquash);
                }

                transform.localScale = Vector3.one;
                LandSquash = Strech(true);
                StartCoroutine(LandSquash);
            });

            VerticalMovement.Subscribe(VerticalState.Grounded, OnStateEnter, () =>
            {
                if (LandSquash != null)
                {
                    StopCoroutine(LandSquash);
                }

                transform.localScale = Vector3.one;
                LandSquash = Strech(false);
                StartCoroutine(LandSquash);
            });
        }

        private void LateUpdate()
        {
            dashRenderer.color = Inputs.CanDash ? BaseColor : noDashColor;
            float direction = Mathf.Sign(Inputs.RunInput);

            if (Inputs.RunInput != 0)
            {
                transform.parent.localScale = new Vector3(
                    Mathf.Abs(transform.parent.localScale.x) * direction,
                    transform.parent.localScale.y,
                    transform.parent.localScale.z);
            }
        }

        private IEnumerator Strech(bool isSquash)
        {
            int direction = isSquash ? 1 : -1;
            squashCounter = 0;

            while (squashCounter < squashTime)
            {
                transform.localScale = new Vector3(
                    transform.localScale.x - (squashSpeed * direction) * Time.deltaTime,
                    transform.localScale.y + (squashSpeed * direction) * Time.deltaTime,
                    transform.localScale.z);

                squashCounter += Time.deltaTime;
                yield return null;
            }

            while (squashCounter > 0)
            {
                transform.localScale = new Vector3(
                    transform.localScale.x + (squashSpeed * direction) * Time.deltaTime,
                    transform.localScale.y - (squashSpeed * direction) * Time.deltaTime,
                    transform.localScale.z);

                squashCounter -= Time.deltaTime;
                yield return null;
            }
            transform.localScale = Vector3.one;
        }
    }
}