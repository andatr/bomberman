using System;
using UnityEngine;

namespace Bomberman
{
    public interface IBombModel_View
    {
        Vector2Int GridPosition { get; }

        ICharacterModel_View Player { get; }

        float Time { get; }

        float LiveTime { get; }

        event Action<IBombModel_View> Placed;

        event Action<IBombModel_View> Exploded;

        event Action<IBombModel_View> Triggered;
    }
}