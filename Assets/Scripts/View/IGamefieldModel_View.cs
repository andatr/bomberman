using System;
using UnityEngine;

namespace Bomberman
{
    public interface IGamefieldModel_View
    {
        GamefieldCell[][] Cells { get; }

        GamefieldCell Cell(Vector2Int position);

        int Width { get; }

        int Height { get; }

        event Action Generated;

        event Action<Vector2Int> CellCleared;
    }
}