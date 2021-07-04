using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DashAttack.Characters
{
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
        public bool CanDash { get; set; }
    }

    //public struct DashInput
    //{
    //    public DashInput(bool button, Vector2 direction)
    //    {
    //        Button = button;
    //        Direction = direction;
    //    }

    //    public bool Button { get; set; }
    //    public Vector2 Direction { get; set; }
    //}
}