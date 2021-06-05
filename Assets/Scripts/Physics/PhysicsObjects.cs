using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class PhysicsObjects : MonoBehaviour
{
    [SerializeField]
    private float maxHorizontalVelocity = 10;
    [SerializeField]
    private float maxVerticalVelocity = 10;
    [SerializeField]
    private ICollisionChecker checker;

    private Rigidbody2D rb;
    public Collision Collisions => checker.Collisions;

    private Vector2 deltaPosition;
    // Make sure velocity does not exceeds maximum values
    public Vector2 DeltaPosition
    {
        get => deltaPosition;
        private set
        {
            deltaPosition.x = Mathf.Clamp(value.x, -maxHorizontalVelocity, maxHorizontalVelocity);
            deltaPosition.y = Mathf.Clamp(value.y, -maxVerticalVelocity, maxVerticalVelocity);
        }
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        checker = GetComponent<BoxCheck>();
        checker.OnCollision += (other) => CollisionEntered(other);
        checker.ShouldIgnoreCollisions = (other) => IgnoreCollisions(other);
    }

    // Gatter all the movement request and add them to velocity
    public void AddMovement(Vector2 movement) => DeltaPosition += movement;

    private void CollisionCheck()
    {
        if (DeltaPosition != Vector2.zero)
        {
            DeltaPosition = checker.Check(DeltaPosition);
        }
    }

    // Move the body and reset movementrequest and velocity
    private void Move()
    {
        //transform.Translate(deltaPosition);
        rb.MovePosition(rb.position + deltaPosition);
        DeltaPosition = Vector2.zero;
    }

    protected virtual void Update()
    {
        CollisionCheck();
        Move();
    }

    protected virtual void CollisionEntered(GameObject other) { }
    protected abstract bool IgnoreCollisions(GameObject other);
}