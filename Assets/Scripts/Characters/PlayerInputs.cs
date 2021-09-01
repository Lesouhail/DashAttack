namespace DashAttack.Characters
{
    using UnityEngine;

    public class PlayerInputs
    {
        public float RunInput { get; set; }
        public bool JumpInput { get; set; }
        public bool DashInput { get; set; }
        public Vector2 DashDirection { get; set; }

        public float LastFrameRunInput { get; set; }
        public bool LastFrameJumpInput { get; set; }
        public bool LastFrameDashInput { get; set; }

        public float JumpInputBuffer { get; set; }
        public float FallBuffer { get; set; }
        public float WallStickBuffer { get; set; }

        public bool CanWallJump { get; set; }
        public bool CanInAirJump { get; set; }
        public bool CanDash { get; set; }
        public bool IsInDashRecovery { get; set; }
    }
}