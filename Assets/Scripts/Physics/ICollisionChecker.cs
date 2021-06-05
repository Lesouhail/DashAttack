﻿using System;
using UnityEngine;

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
}