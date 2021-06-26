using System;
using UnityEngine;

namespace DashAttack.Physics
{
    public interface ICollisionChecker
    {
        Vector2 Check(Vector2 velocity);

        Collision Collisions { get; }

        Func<GameObject, bool> ShouldIgnoreCollisions { get; set; }
        event Action<GameObject> OnCollision;
    }

    public struct Collision
    {
        public bool Above, Below, Left, Right;

        public Collision Clone()
        {
            var result = new Collision();
            result.Above = Above;
            result.Below = Below;
            result.Left = Left;
            result.Right = Right;
            return result;
        }
    }
}