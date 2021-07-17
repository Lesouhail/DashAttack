namespace DashAttack.Characters
{
    using System.Collections;
    using System.Linq;
    using DashAttack.Characters.Movements.Dash;
    using DashAttack.Characters.Movements.Vertical;
    using UnityEngine;
    using UnityEngine.Experimental.Rendering.Universal;
    using static DashAttack.Utility.StateCallBack;

    public class PlayerFeedbacks : MonoBehaviour
    {
        [SerializeField] private Color activeFragmentColor;
        [SerializeField] private Color unActiveFragmentColor;
        [SerializeField] private GameObject[] fragments;
        [SerializeField] private float squashTime;
        [SerializeField] private float squashDepth;

        public PlayerInputs Inputs { get; set; }

        private VerticalMovement VerticalMovement { get; set; }
        private DashMovement Dash { get; set; }

        private float squashCounter { get; set; }
        private float squashSpeed => squashDepth / squashTime;

        private IEnumerator LandSquash { get; set; }
        private IEnumerator JumpStretch { get; set; }
        private IEnumerator DashStretch { get; set; }

        private Fragment[] Fragments { get; set; }

        private void Start()
        {
            VerticalMovement = GetComponentInParent<VerticalMovement>();
            Dash = GetComponentInParent<DashMovement>();

            Fragments = fragments.Select(f => new Fragment()
            {
                Renderer = f.GetComponent<SpriteRenderer>(),
                Light = f.GetComponent<Light2D>()
            })
            .ToArray();

            foreach (var fragment in Fragments)
            {
                fragment.Light.enabled = false;
                fragment.Renderer.color = unActiveFragmentColor;
            }

            VerticalMovement.Subscribe(VerticalState.Rising, OnStateEnter, () =>
            {
                Reset();
                bool isSquash = VerticalMovement.PreviousState != VerticalState.WallSliding;
                JumpStretch = Stretch(isSquash);
                StartCoroutine(JumpStretch);
            });

            VerticalMovement.Subscribe(VerticalState.Grounded, OnStateEnter, () =>
            {
                Reset();
                LandSquash = Stretch(false);
                StartCoroutine(LandSquash);
            });

            Dash.Subscribe(DashState.Dashing, OnStateEnter, () =>
            {
                Reset();
                bool isSquash = Mathf.Abs(Inputs.DashDirection.y) == 1;
                DashStretch = Stretch(isSquash);
                StartCoroutine(DashStretch);
            });
        }

        private void Reset()
        {
            if (LandSquash != null)
            {
                StopCoroutine(LandSquash);
            }

            if (JumpStretch != null)
            {
                StopCoroutine(JumpStretch);
            }

            if (DashStretch != null)
            {
                StopCoroutine(DashStretch);
            }

            transform.localScale = Vector3.one;
        }

        private void LateUpdate()
        {
            float direction = Mathf.Sign(Inputs.RunInput);

            if (Inputs.RunInput != 0)
            {
                transform.parent.localScale = new Vector3(
                    Mathf.Abs(transform.parent.localScale.x) * direction,
                    transform.parent.localScale.y,
                    transform.parent.localScale.z);
            }

            if (Inputs.CanDash)
            {
                Fragments[0].Renderer.color = activeFragmentColor;
                Fragments[0].Light.enabled = true;
            }
            else
            {
                Fragments[0].Renderer.color = unActiveFragmentColor;
                Fragments[0].Light.enabled = false;
            }
        }

        private IEnumerator Stretch(bool isSquash)
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

    internal struct Fragment
    {
        public SpriteRenderer Renderer;
        public Light2D Light;
    }
}