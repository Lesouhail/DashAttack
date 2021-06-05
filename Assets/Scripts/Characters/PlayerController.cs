using UnityEngine;

using static DashState;

public class PlayerController : PhysicsObjects
{
    [SerializeField] private LayerMask collidablesMask;

    public DashState DashState => Dash.CurrentState;
    public VerticalState VerticalState => VerticalMovement.CurrentState;
    public HorizontalState HorizontalState => HorizontalMovement.CurrentState;

    public DashState PreviousDashState => Dash.PreviousState;
    public VerticalState PreviousVerticalState => VerticalMovement.PreviousState;
    public HorizontalState PreviousHorizontalState => HorizontalMovement.PreviousState;

    private HorizontalMovement HorizontalMovement { get; set; }
    private VerticalMovement VerticalMovement { get; set; }
    private Dash Dash { get; set; }

    protected override void Start()
    {
        base.Start();
        HorizontalMovement = GetComponent<HorizontalMovement>();
        VerticalMovement = GetComponent<VerticalMovement>();
        Dash = GetComponent<Dash>();
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

        if (Dash.CurrentState == Rest || Dash.CurrentState != Recovering && Dash.PreviousState != Rest)
        {
            HorizontalMovement.IsLocked = false;
        }
    }

    private void OnDashHit(GameObject other)
    {
        if (other.TryGetComponent<ICollidable>(out var collidable))
        {
            collidable.Collide(gameObject);
            Dash.Reset();
        }
    }

    protected override void CollisionEntered(GameObject other)
    {
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
}