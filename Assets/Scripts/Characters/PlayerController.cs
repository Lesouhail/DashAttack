namespace DashAttack.Characters
{
    using DashAttack.Characters.Movements.Dash;
    using DashAttack.Characters.Movements.Horizontal;
    using DashAttack.Characters.Movements.Vertical;
    using DashAttack.Extensions;
    using DashAttack.Physics;
    using UnityEngine;

    using static DashAttack.Characters.Movements.Dash.DashState;

    public class PlayerController : PhysicsObjects
    {
        [SerializeField] private LayerMask collidablesMask;

        private HorizontalMovement HorizontalMovement { get; set; }
        private VerticalMovement VerticalMovement { get; set; }
        private DashMovement Dash { get; set; }

        protected override void Start()
        {
            base.Start();
            HorizontalMovement = GetComponent<HorizontalMovement>();
            VerticalMovement = GetComponent<VerticalMovement>();
            Dash = GetComponent<DashMovement>();
        }

        protected override void Update()
        {
            base.Update();
            if (Dash.CurrentState != Rest && Dash.PreviousState == Rest)
            {
                HorizontalMovement.IsLocked = true;
                VerticalMovement.IsLocked = true;
            }

            if (Dash.CurrentState == Rest && Dash.PreviousState != Rest)
            {
                VerticalMovement.IsLocked = false;
            }

            if (Dash.CurrentState == Rest || (Dash.CurrentState != Recovering && Dash.PreviousState != Rest))
            {
                HorizontalMovement.IsLocked = false;
            }
        }

        protected override bool IgnoreCollisions(GameObject other)
        {
            if (Dash.CurrentState == Dashing && collidablesMask.ContainsLayer(other.layer))
            {
                OnDashHit(other);
                return true;
            }
            return false;
        }

        protected override void CollisionEntered(GameObject other)
        {
        }

        private void OnDashHit(GameObject other)
        {
            if (other.TryGetComponent<ICollidable>(out var collidable))
            {
                collidable.Collide(gameObject);
                Dash.Reset();
            }
        }
    }
}