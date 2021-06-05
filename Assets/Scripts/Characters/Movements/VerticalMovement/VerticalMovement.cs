using System;
using UnityEngine;

using static VerticalState;
using static StateCallBack;

[RequireComponent(typeof(PhysicsObjects))]
public class VerticalMovement : VerticalMovementBase<VerticalMovement, VerticalState>
{
    [SerializeField] private float jumpEarlyBuffer;
    [SerializeField] private float jumpLateBuffer;
    [SerializeField] private float wallSlideMultiplier;
    [SerializeField] private float wallStickTime;

    public float JumpEarlyBuffer => jumpEarlyBuffer;
    public float JumpLateBuffer => jumpLateBuffer;
    public float WallStickTime => wallStickTime;

    public float EarlyJumpCounter { get; private set; }
    public float LateJumpCounter { get; private set; }
    public float WallStickCounter { get; private set; }

    public override float Gravity => 2 * MaxJumpHeight / Mathf.Pow(JumpTime, 2);
    public bool WallJumpThisFrame { get; protected set; }

    public bool LastFrameInput { get; private set; }

    public event Action TakingOff;
    public event Action StartedFall;
    public event Action Landing;

    public override bool Input
    {
        get => base.Input;
        set
        {
            base.Input = value;
            if (value == true && LastFrameInput == false)
            {
                EarlyJumpCounter = 0;
            }
        }
    }

    public bool IsLocked
    {
        get => StateMachine.IsLocked;
        set
        {
            if (value)
            {
                CurrentVerticalVelocity = 0;
                CurrentWallJumpVelocity = Vector2.zero;
            }
            StateMachine.IsLocked = value;
        }
    }

    protected Vector2 CurrentWallJumpVelocity { get; set; }

    protected override void Update()
    {
        base.Update();
        if (Input)
        {
            EarlyJumpCounter += Time.deltaTime;
        }

        LastFrameInput = Input;
    }

    protected override void InitStateMachine()
    {
        StateMachine.Init(
            Rest,
            new VerticalRestState(this, StateMachine),
            new GroundedState(this, StateMachine),
            new RisingState(this, StateMachine),
            new FallingState(this, StateMachine),
            new WallSlidingState(this, StateMachine));

        // --- GROUNDED STATE ---
        Subscribe(Grounded, OnStateEnter, () => Landing?.Invoke());
        Subscribe(Grounded, OnUpdate, () =>
        {
            // Apply a constant downward force equals to one frame of gravity in order to trigger collision detection
            CurrentVerticalVelocity = 0;
            Fall(true);
        });

        // --- FALLING STATE ---
        Subscribe(Falling, OnStateEnter, () =>
        {
            StartedFall?.Invoke();
            LateJumpCounter = 0;
        });
        Subscribe(Falling, OnUpdate, () =>
        {
            LateJumpCounter += Time.deltaTime;
            Fall(true);
        });

        Subscribe(Falling, OnStateExit, () => LateJumpCounter = 0);

        // --- RISING ---
        Subscribe(Rising, OnStateEnter, () =>
        {
            TakingOff?.Invoke();
            InitiateJump();
        });
        Subscribe(Rising, OnUpdate, () => Fall(false));
        Subscribe(Rising, OnStateExit, () => CurrentVerticalVelocity = 0);

        // --- WALL SLIDING STATE ---
        StateMachine.Subscribe(WallSliding, OnUpdate, () => WallSlide());
        Subscribe(WallSliding, OnStateExit, () => CurrentVerticalVelocity = 0);
    }

    private void InitiateJump()
    {
        JumpStartPosition = transform.position.y;
        CurrentVerticalVelocity = JumpVelocity;
    }

    private void Fall(bool allowFallMultiplier)
    {
        var fallMultiplier = allowFallMultiplier ? FallMultiplier : 1;
        CurrentVerticalVelocity -= Gravity * Time.deltaTime * fallMultiplier;

        if (CurrentVerticalVelocity < -MaxFallVelocity)
        {
            CurrentVerticalVelocity = -MaxFallVelocity;
        }
        physicsComponent.AddMovement(new Vector2(0, DeltaPosition(CurrentVerticalVelocity)));
    }

    private void WallSlide()
    {
        CurrentVerticalVelocity -= Gravity * wallSlideMultiplier * Time.deltaTime;
        var wallSlideMaxVelocity = -MaxFallVelocity * wallSlideMultiplier;
        if (CurrentVerticalVelocity < wallSlideMaxVelocity)
        {
            CurrentVerticalVelocity = wallSlideMaxVelocity;
        }
        physicsComponent.AddMovement(new Vector2(0, DeltaPosition(CurrentVerticalVelocity)));
    }
}

public enum VerticalState
{
    Rest,
    Falling,
    Grounded,
    Rising,
    WallSliding,
}