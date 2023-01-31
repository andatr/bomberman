using UnityEngine;

namespace Bomberman
{
    public interface IPlayerModel_Controller
    {
        float Speed { get; }

        Vector2 Direction { get; set; }

        Vector2 Position { get; set; }

        bool PlaceBomb(Vector2Int gridPosition);
    }
}