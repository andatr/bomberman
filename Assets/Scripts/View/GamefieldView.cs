using UnityEngine;
using UnityEngine.Tilemaps;

namespace Bomberman
{
    public class GamefieldView : MonoBehaviour, IGrid
    {
        #region Public / Messages

        private void OnDestroy()
        {
            Unsubscribe();
        }

        public void Init(IGamefieldModel_View gamefield)
        {
            CheckComponents();
            Unsubscribe();
            _gamefield = gamefield;
            if (_gamefield != null) {
                _gamefield.Generated += OnGenerated;
                _gamefield.CellCleared += OnCellCleared;
                _tilemapHelper = new TilemapHelper(_tilemap, gamefield.Width, gamefield.Height);
            }
        }

        public Vector2 CellToWorld(Vector2Int gridPosition)
        {
            Vector3Int gridViewPosition = _tilemapHelper.CellLogicToView(gridPosition);
            Vector3 worldPosition = _tilemap.CellToWorld(gridViewPosition);
            worldPosition.x += _tilemap.cellSize.x / 2.0f;
            worldPosition.y += _tilemap.cellSize.y / 2.0f;
            return new Vector2(worldPosition.x, worldPosition.y);
        }

        public Vector2Int WorldToCell(Vector2 worldPosition)
        {
            Vector3Int gridViewPosition = _tilemap.WorldToCell(new Vector3(worldPosition.x, worldPosition.y, 0.0f));
            return _tilemapHelper.CellViewToLogic(gridViewPosition);
        }

        #endregion

        #region Private

        private void CheckComponents()
        {
            this.CheckMember(_tilemap, "Tilemap");
            this.CheckMember(_pathTiles, "Path");
            this.CheckMember(_pillarTiles, "Pillart");
            this.CheckMember(_obstacleMidTiles, "Obstacle");
            this.CheckMember(_obstacleRightTiles, "Obstacle");
            this.CheckMember(_obstacleLeftTiles, "Obstacle");
            this.CheckMember(_obstacleSingleTiles, "Obstacle");
            this.CheckMember(_borderTopTiles, "Top border");
            this.CheckMember(_borderBottomTiles, "Bottom border");
            this.CheckMember(_borderLeftTiles, "Left border");
            this.CheckMember(_borderRightTiles, "Right border");
            this.CheckMember(_cornerTopLeftTile, "Top-Left corner");
            this.CheckMember(_cornerTopRightTile, "Top-Right corner");
            this.CheckMember(_cornerBottomLeftTile, "Bottom-Left corner");
            this.CheckMember(_cornerBottomRightTile, "Bottom-right corner");
            this.CheckMember(_entranceTile, "Entrance");
            this.CheckMember(_exitLockedTile, "Locked Exit");
            this.CheckMember(_exitTile, "Exit");
        }

        private void OnCellCleared(Vector2Int cell)
        {
            _tilemapHelper.SetTiles(cell.x, cell.y, _pathTiles);
        }

        private void OnGenerated()
        {
            int h = _gamefield.Height - 1;
            int w = _gamefield.Width - 1;
            // corners
            _tilemapHelper.SetTile(0, 0, _cornerTopLeftTile);
            _tilemapHelper.SetTile(w, 0, _cornerTopRightTile);
            _tilemapHelper.SetTile(0, h, _cornerBottomLeftTile);
            _tilemapHelper.SetTile(w, h, _cornerBottomRightTile);
            // top and bottom borders
            for (int i = 1; i < w; ++i) {
                _tilemapHelper.SetTiles(i, 0, _borderTopTiles);
                _tilemapHelper.SetTiles(i, h, _borderBottomTiles);
            }
            // left and right borders
            for (int i = 1; i < h; ++i) {
                _tilemapHelper.SetTiles(0, i, _borderLeftTiles);
                _tilemapHelper.SetTiles(w, i, _borderRightTiles);
            }
            // pillars, paths and obstacles
            for (int i = 1; i < h; ++i) {
                for (int j = 1; j < w; ++j) {
                    if (_gamefield.Cells[i][j].CellType == CellType.Border) {
                        _tilemapHelper.SetTiles(j, i, _pillarTiles);
                    }
                    else if (_gamefield.Cells[i][j].CellType == CellType.Obstacle) {
                        if (_gamefield.Cells[i][j - 1].CellType == CellType.Obstacle) {
                            if (_gamefield.Cells[i][j + 1].CellType == CellType.Obstacle) {
                                _tilemapHelper.SetTiles(j, i, _obstacleMidTiles);
                            }
                            else {
                                _tilemapHelper.SetTiles(j, i, _obstacleRightTiles);
                            }
                        }
                        else {
                            if (_gamefield.Cells[i][j + 1].CellType == CellType.Obstacle) {
                                _tilemapHelper.SetTiles(j, i, _obstacleLeftTiles);
                            }
                            else {
                                _tilemapHelper.SetTiles(j, i, _obstacleSingleTiles);
                            }
                        }
                    }
                    else {
                        _tilemapHelper.SetTiles(j, i, _pathTiles);
                    }
                }
            }
            _tilemapHelper.SetTile(1, 0, _entranceTile);
        }

        private void Unsubscribe()
        {
            if (_gamefield != null) {
                _gamefield.Generated -= OnGenerated;
                _gamefield.CellCleared -= OnCellCleared;
            }
        }

        #endregion

        #region Fields

        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tile[] _pathTiles;
        [SerializeField] private Tile[] _pillarTiles;
        [SerializeField] private Tile[] _obstacleSingleTiles;
        [SerializeField] private Tile[] _obstacleLeftTiles;
        [SerializeField] private Tile[] _obstacleRightTiles;
        [SerializeField] private Tile[] _obstacleMidTiles;
        [SerializeField] private Tile[] _borderTopTiles;
        [SerializeField] private Tile[] _borderBottomTiles;
        [SerializeField] private Tile[] _borderLeftTiles;
        [SerializeField] private Tile[] _borderRightTiles;
        [SerializeField] private Tile _cornerTopLeftTile;
        [SerializeField] private Tile _cornerTopRightTile;
        [SerializeField] private Tile _cornerBottomLeftTile;
        [SerializeField] private Tile _cornerBottomRightTile;
        [SerializeField] private Tile _entranceTile;
        [SerializeField] private Tile _exitLockedTile;
        [SerializeField] private Tile _exitTile;
        IGamefieldModel_View _gamefield;
        TilemapHelper _tilemapHelper;

        #endregion
    }
}