using UnityEngine;
using UnityEngine.Tilemaps;

namespace Bomberman
{
    [CreateAssetMenu(fileName = "Explosion", menuName = "ScriptableObjects/RotatedTile", order = 5)]
    public class RotatedTile : ScriptableObject
    {
        public TileBase tile;
        public Vector3 rotation;
    }
}
