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

        public Vector2Int KeyPosition { get { return _keyPosition; } }

        public Vector2Int ExitPosition { get { return _exitPosition; } }

        public Vector2Int BonusPosition { get { return _bonusPosition; } }

        public Vector2Int PlayerStartPosition { get { return _playerStartPosition; } }

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
            int height = _height - 1;
            int width  = _width - 1;
            int halfHeight = height / 2;
            int halfWidth = width / 2;
            // top and bottom borders
            for (int i = 0; i < _width; ++i) {
                _cells[0][i].CellType = CellType.Border;
                _cells[height][i].CellType = CellType.Border;
            }
            // left and right borders
            for (int i = 0; i < _height; ++i) {
                _cells[i][0].CellType = CellType.Border;
                _cells[i][width].CellType = CellType.Border;
            }
            // pillars, paths and obstacles
                                 FillQuadrant(1,         halfWidth, 1,          halfHeight, density); // top left
            int obstacleCount1 = FillQuadrant(halfWidth, width,     1,          halfHeight, density); // top right
            int obstacleCount2 = FillQuadrant(1,         halfWidth, halfHeight, height,     density); // bottom left
            int obstacleCount3 = FillQuadrant(halfWidth, width,     halfHeight, height,     density); // bottom right
            // positions of exit, key and bonus
            Vector2Int pos1 = GetPositionInQuadrant(halfWidth, width,     1,          halfHeight, obstacleCount1);
            Vector2Int pos2 = GetPositionInQuadrant(1,         halfWidth, halfHeight, height,     obstacleCount2);
            Vector2Int pos3 = GetPositionInQuadrant(halfWidth, width,     halfHeight, height,     obstacleCount3);
            GenerateObjects(pos1, pos2, pos3);
            _playerStartPosition = new Vector2Int(1, 1);
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

        private int FillQuadrant(int xStart, int xEnd, int yStart, int yEnd, float density)
        {
            int count = 0;
            for (int i = yStart; i < yEnd; ++i) {
                for (int j = xStart; j < xEnd; ++j) {
                    if (FillCell(j, i, density) == CellType.Obstacle) ++count;
                }
            }
            return count;
        }

        private CellType FillCell(int x, int y, float density)
        {
            if (x % 2 == 0 && y % 2 == 0) {
                _cells[y][x].CellType = CellType.Border;
            }
            else if (UnityEngine.Random.Range(0.0f, 1.0f) > density) {
                _cells[y][x].CellType = CellType.Obstacle;
            }
            else {
                _cells[y][x].CellType = CellType.Path;
            }
            return _cells[y][x].CellType;
        }

        private Vector2Int GetPositionInQuadrant(int xStart, int xEnd, int yStart, int yEnd, int obstacles)
        {
            int index = UnityEngine.Random.Range(0, obstacles);
            int count = 0;
            for (int i = yStart; i < yEnd; ++i) {
                for (int j = xStart; j < xEnd; ++j) {
                    if (_cells[i][j].CellType == CellType.Obstacle) {
                        if (count++ == index) return new Vector2Int(j, i);
                    }
                }
            }
            // should never rich this
            Debug.LogWarning("GetPositionInQuadrant error");
            return new Vector2Int((xStart + xStart) / 2, (yStart + yEnd) / 2);
        }

        private void GenerateObjects(Vector2Int pos1, Vector2Int pos2, Vector2Int pos3)
        {
            int variant = UnityEngine.Random.Range(0, 6);
            switch (variant) {
                case 0:
                    _keyPosition   = pos1;
                    _exitPosition  = pos2;
                    _bonusPosition = pos3;
                    break;
                case 1:
                    _keyPosition   = pos1;
                    _exitPosition  = pos3;
                    _bonusPosition = pos2;
                    break;
                case 2:
                    _keyPosition   = pos2;
                    _exitPosition  = pos1;
                    _bonusPosition = pos3;
                    break;
                case 3:
                    _keyPosition   = pos2;
                    _exitPosition  = pos3;
                    _bonusPosition = pos1;
                    break;
                case 4:
                    _keyPosition   = pos3;
                    _exitPosition  = pos1;
                    _bonusPosition = pos2;
                    break;
                case 5:
                    _keyPosition   = pos3;
                    _exitPosition  = pos2;
                    _bonusPosition = pos1;
                    break;
            }
        }
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
        private Vector2Int _keyPosition;
        private Vector2Int _exitPosition;
        private Vector2Int _bonusPosition;
        private Vector2Int _playerStartPosition;

        #endregion
    }
}