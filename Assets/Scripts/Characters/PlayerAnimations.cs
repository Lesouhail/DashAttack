namespace DashAttack.Characters
{
    using DashAttack.Characters.Movements.Dash;
    using DashAttack.Characters.Movements.Horizontal;
    using DashAttack.Characters.Movements.Vertical;
    using UnityEngine;

    public class PlayerAnimations : MonoBehaviour
    {
        private const string IsRunning = "Running";
        private const string TakingOff = "TakingOff";
        private const string IsFalling = "Falling";
        private const string DirectionX = "DirectionX";
        private const string DirectionY = "DirectionY";
        private const string Dashing = "Dashing";

        private VerticalMovement VerticalMovement { get; set; }
        private HorizontalMovement HorizontalMovement { get; set; }
        private DashMovement Dash { get; set; }
        private Animator Animator { get; set; }

        private void Start()
        {
            HorizontalMovement = GetComponent<HorizontalMovement>();
            // VerticalMovement = GetComponent<VerticalMovement>();
            // Dash = GetComponent<Dash>();
            // Animator = GetComponent<Animator>();

            // HorizontalMovement.RunStarted += () =>
            // {
            //     Animator.SetBool(
            //         IsRunning,
            //         VerticalMovement.CurrentState == Grounded &&
            //         Dash.CurrentState == DashState.Rest);
            // };
            // HorizontalMovement.RunEnded += () => Animator.SetBool(IsRunning, false);

            // VerticalMovement.TakingOff += () => Animator.SetBool(TakingOff, Dash.CurrentState == DashState.Rest);
            // VerticalMovement.StartedFall += () =>
            // {
            //     if (Dash.CurrentState == DashState.Rest)
            //     {
            //         Animator.SetBool(TakingOff, false);
            //         Animator.SetBool(IsFalling, true);
            //     }
            // };
            // VerticalMovement.Landing += () => Animator.SetBool(IsFalling, false);

            // Dash.DashStarted += (Direction) => Animator.SetBool(Dashing, true);
            // Dash.DashEnded += () =>
            // {
            //     Animator.SetFloat(DirectionX, 0);
            //     Animator.SetFloat(DirectionY, 0);
            //     Animator.SetBool(Dashing, false);
            // };
        }

        //private void Update()
        //{
        //    if (HorizontalMovement.Inputs. != 0)
        //    {
        //        transform.localScale = new Vector3(
        //           Mathf.Sign(HorizontalMovement.Input),
        //           transform.localScale.y,
        //           transform.localScale.z);
        //    }

        //    // if (Dash.CurrentState == Casting)
        //    // {
        //    //     Animator.SetFloat(DirectionX, Mathf.Abs(Dash.Direction.x));
        //    //     Animator.SetFloat(DirectionY, Dash.Direction.y);
        //    // }
        //}
    }
}