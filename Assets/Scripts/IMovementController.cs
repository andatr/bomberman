using UnityEngine;
using System;

public interface IMovementController
{
    public event Action<Vector2> DirectionChanged;
}
