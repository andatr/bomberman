using UnityEngine;

namespace Bomberman
{
    public interface IGrid
    {
        Vector2 CellToWorld(Vector2Int gridPosition);

        Vector2Int WorldToCell(Vector2 worldPosition);
    }
}