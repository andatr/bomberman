using UnityEngine;

namespace Bomberman
{
    public interface IExplosionlModel_View
    {
        Vector2Int GridPosition { get; }

        int Strength { get; }

        ExplosionDirection Direction { get; }
    }
}