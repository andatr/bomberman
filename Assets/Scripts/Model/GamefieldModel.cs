using System;
using UnityEngine;

namespace Bomberman
{
    public class GamefieldModel : IGamefieldModel_View
    {
        #region Public

        public GamefieldCell[][] Cells { get { return _cells; } }

        public int Width { get { return _width; } }

        public int Height { get { return _height; } }

        public GamefieldCell Cell(Vector2Int position)
        {
            return _cells[position.y][position.x];
        }

        public GamefieldModel(GamefieldConfig config)
        {
            _width  = config.width;
            _height = config.height;
            _density = config.density;
            ValidateSize(ref _width);
            ValidateSize(ref _height);
            AllocateCells();
        }

        public void Generate(int level)
        {
            float density = 0.5f + _density * level;
            density = 1.0f - density;
            int h = _height - 1;
            int w = _width - 1;
            // top and bottom borders
            for (int i = 0; i < _width; ++i) {
                _cells[0][i].CellType = CellType.Border;
                _cells[h][i].CellType = CellType.Border;
            }
            // left and right borders
            for (int i = 0; i < _height; ++i) {
                _cells[i][0].CellType = CellType.Border;
                _cells[i][w].CellType = CellType.Border;
            }
            // pillars, paths and obstacles
            for (int i = 1; i < h; ++i) {
                for (int j = 1; j < w; ++j) {
                    if (i % 2 == 0 && j % 2 == 0) {
                        _cells[i][j].CellType = CellType.Border;
                    }
                    else if (UnityEngine.Random.Range(0.0f, 1.0f) > density) {
                        _cells[i][j].CellType = CellType.Obstacle;
                    }
                    else {
                        _cells[i][j].CellType = CellType.Path;
                    }
                }
            }
            Generated?.Invoke();
        }

        public void ClearCell(Vector2Int gridPosition)
        {
            var cell = Cell(gridPosition);
            if (cell.CellType == CellType.Obstacle) {
                cell.CellType = CellType.Path;
                CellCleared?.Invoke(gridPosition);
            }
        }

        public event Action Generated;

        public event Action<Vector2Int> CellCleared;

        #endregion

        #region Private

        private void ValidateSize(ref int size)
        {
            if (size < 5) {
                Debug.LogWarning($"Gamefield: gamefield size {size} must be > 5");
                size = 5;
            }
            if (size % 2 == 0) {
                Debug.LogWarning($"Gamefield: gamefield size {size} must be an odd number");
                size += 1;
            }
        }

        private void AllocateCells()
        {
            _cells = new GamefieldCell[_height][];
            for (int i = 0; i < _height; ++i) {
                _cells[i] = new GamefieldCell[_width];
                for (int j = 0; j < _width; ++j) {
                    _cells[i][j] = new GamefieldCell();
                }
            }
        }

        #endregion

        #region Fields

        private GamefieldCell[][] _cells;
        private int _width;
        private int _height;
        private float _density;

        #endregion
    }
}