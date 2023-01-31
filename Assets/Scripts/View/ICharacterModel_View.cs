using UnityEngine;
using System;

namespace Bomberman
{
    public interface ICharacterModel_View
    {
        Vector2 Position { get; }

        event Action<Vector2> Moved;

        event Action<Vector2> DirectionChanged;

        event Action Died;
    }
}