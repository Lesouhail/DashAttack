using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : PhysicsObjects
{
    [SerializeField] private Transform target;
    [SerializeField] private float horizontalSmoothTime = 0.2f;
    [SerializeField] private float verticalSmoothTime = 0.2f;

    [SerializeField] private Vector2 playerFrameScale;

    private HorizontalMovement HorizontalMovement { get; set; }
    private VerticalMovement VerticalMovement { get; set; }
    private Dash Dash { get; set; }

    private float currentXVelocity;
    private float currentYVelocity;

    protected override void Start()
    {
        base.Start();
        HorizontalMovement = target.GetComponent<HorizontalMovement>();
        VerticalMovement = target.GetComponent<VerticalMovement>();
        Dash = target.GetComponent<Dash>();
    }

    protected override void Update()
    {
        DebugFrame();

        Vector3 nextPosition = target.position;
        float smoothX = transform.position.x;
        float smoothY = transform.position.y;

        bool isXOutOfFrame = Mathf.Abs(nextPosition.x - transform.position.x) > playerFrameScale.x;
        bool isYOutOfFrame = Mathf.Abs(nextPosition.y - transform.position.y) > playerFrameScale.y;

        if (isXOutOfFrame)
        {
            smoothX = Mathf.SmoothDamp(transform.position.x, nextPosition.x, ref currentXVelocity, horizontalSmoothTime);
        }

        if (VerticalMovement.CurrentState == VerticalState.Grounded)
        {
            smoothY = Mathf.SmoothDamp(transform.position.y, nextPosition.y, ref currentYVelocity, verticalSmoothTime);
        }

        var smoothedNextPosition = new Vector3(smoothX, smoothY, nextPosition.z);
        AddMovement(smoothedNextPosition - transform.position);
        base.Update();
    }

    private void DebugFrame()
    {
        var bottomLeft = new Vector2(transform.position.x - playerFrameScale.x, transform.position.y - playerFrameScale.y);
        var topLeft = new Vector2(transform.position.x - playerFrameScale.x, transform.position.y + playerFrameScale.y);
        var bottomRight = new Vector2(transform.position.x + playerFrameScale.x, transform.position.y - playerFrameScale.y);
        var topRight = new Vector2(transform.position.x + playerFrameScale.x, transform.position.y + playerFrameScale.y);

        Debug.DrawLine(bottomLeft, topLeft, Color.green);
        Debug.DrawLine(topLeft, topRight, Color.green);
        Debug.DrawLine(topRight, bottomRight, Color.green);
        Debug.DrawLine(bottomRight, bottomLeft, Color.green);
    }

    protected override bool IgnoreCollisions(GameObject other)
    {
        return false;
    }
}
