using UnityEngine;
using UnityEngine.Tilemaps;

public class Gamefield : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameRules _rules;

    [SerializeField]
    private Tilemap _tilemap;

    [SerializeField]
    private Tile[] _pathTiles;

    [SerializeField]
    private Tile[] _pillarTiles;

    [SerializeField]
    private Tile[] _obstacleTiles;

    [SerializeField]
    private Tile[] _borderTopTiles;

    [SerializeField]
    private Tile[] _borderBottomTiles;

    [SerializeField]
    private Tile[] _borderLeftTiles;

    [SerializeField]
    private Tile[] _borderRightTiles;

    [SerializeField]
    private Tile[] _cornerTopLeftTiles;

    [SerializeField]
    private Tile[] _cornerTopRightTiles;

    [SerializeField]
    private Tile[] _cornerBottomLeftTiles;

    [SerializeField]
    private Tile[] _cornerBottomRightTiles;

    private enum CellType
    {
        Path,
        Border,
        Obstacle
    }

    private class Cell
    {
        public CellType type;

        public Cell()
        {
            type = CellType.Path;
        }
    }

    private Cell[][] _cells;
    private int _width;
    private int _height;
    private int _widthOffset;
    private int _heightOffset;

    #endregion

    #region Public / Messages

    public int Width { get { return _width; } }

    public int Height { get { return _height; } }

    private void Awake()
    {
        CheckComponents();
        Init();
    }

    public Vector3 TileToWorld(Vector3Int tilePosition)
    {
        tilePosition.x -= _cells[0].Length / 2;
        tilePosition.y -= _cells.Length / 2;
        var worldPosition = _tilemap.CellToWorld(tilePosition);
        worldPosition.x += _tilemap.cellSize.x / 2.0f;
        worldPosition.y = -worldPosition.y + _tilemap.cellSize.y / 2.0f;
        return worldPosition;
    }

    public Vector2Int WorldToTile(Vector3 worldPosition)
    {
        var tilePosition = _tilemap.WorldToCell(worldPosition);
        tilePosition.x -= _widthOffset;
        tilePosition.y = -tilePosition.y + _heightOffset;
        return new Vector2Int(tilePosition.x, tilePosition.y);
    }

    public void Generate(int level)
    {
        float density = _rules.gamefield.GetDensity(level);
        GenerateLogic(density);
        GenerateView();
    }

    #endregion

    #region Private

    private void Init()
    {
        _width = _rules.gamefield.width;
        _height = _rules.gamefield.height;
        ValidateSize(ref _width);
        ValidateSize(ref _height);
        _widthOffset = -_width / 2;
        _heightOffset = _height / 2;
        AllocateCells();
    }

    private void CheckComponents()
    {
        CheckTilemap();
        CheckGameRules();
        CheckTiles(_pathTiles, "Path");
        CheckTiles(_pillarTiles, "Pillart");
        CheckTiles(_obstacleTiles, "Obstacle");
        CheckTiles(_borderTopTiles, "Top border");
        CheckTiles(_borderBottomTiles, "Bottom border");
        CheckTiles(_borderLeftTiles, "Left border");
        CheckTiles(_borderRightTiles, "Right border");
        CheckTiles(_cornerTopLeftTiles, "Top-Left corner");
        CheckTiles(_cornerTopRightTiles, "Top-Right corner");
        CheckTiles(_cornerBottomLeftTiles, "Bottom-Left corner");
        CheckTiles(_cornerBottomRightTiles, "Bottom-right corner");
    }

    private void CheckTilemap()
    {
        if (_tilemap == null) {
            enabled = false;
            Debug.LogError("Tilemap component not set", this);
        }
    }

    private void CheckGameRules()
    {
        if (_rules == null) {
            enabled = false;
            Debug.LogError("Game Rules component not set", this);
        }
    }

    private void CheckTiles(Tile[] tiles, string name)
    {
        if (tiles == null || tiles.Length == 0) {
            enabled = false;
            Debug.LogError($"{name} tiles not set", this);
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
        _cells = new Cell[_height][];
        for (int i = 0; i < _height; ++i) {
            _cells[i] = new Cell[_width];
            for (int j = 0; j < _width; ++j) {
                _cells[i][j] = new Cell();
            }
        }
    }

    private void GenerateLogic(float density)
    {
        density = 1.0f - density;
        int h = _height - 1;
        int w = _width - 1;
        // top and bottom borders
        for (int i = 0; i < _width; ++i) {
            _cells[0][i].type = CellType.Border;
            _cells[h][i].type = CellType.Border;
        }
        // left and right borders
        for (int i = 0; i < _height; ++i) {
            _cells[i][0].type = CellType.Border;
            _cells[i][w].type = CellType.Border;
        }
        // pillars, paths and obstacles
        for (int i = 1; i < h; ++i) {
            for (int j = 1; j < w; ++j) {
                if (i % 2 == 0 && j % 2 == 0) {
                    _cells[i][j].type = CellType.Border;
                }
                else if (UnityEngine.Random.Range(0.0f, 1.0f) > density) {
                    _cells[i][j].type = CellType.Obstacle;
                }
                else {
                    _cells[i][j].type = CellType.Path;
                }
            }
        }
    }

    private void GenerateView()
    {
        int h = _height - 1;
        int w = _width - 1;
        // corners
        SetTile(0, 0, _cornerTopLeftTiles);
        SetTile(0, w, _cornerTopRightTiles);
        SetTile(h, 0, _cornerBottomLeftTiles);
        SetTile(h, w, _cornerBottomRightTiles);
        // top and bottom borders
        for (int i = 1; i < w; ++i) {
            SetTile(0, i, _borderTopTiles);
            SetTile(h, i, _borderBottomTiles);
        }
        // left and right borders
        for (int i = 1; i < h; ++i) {
            SetTile(i, 0, _borderLeftTiles);
            SetTile(i, w, _borderRightTiles);
        }
        // pillars, paths and obstacles
        for (int i = 1; i < h; ++i) {
            for (int j = 1; j < w; ++j) {
                if (_cells[i][j].type == CellType.Border) {
                    SetTile(i, j, _pillarTiles);
                }
                else if (_cells[i][j].type == CellType.Obstacle) {
                    SetTile(i, j, _obstacleTiles);
                }
                else {
                    SetTile(i, j, _pathTiles);
                }
            }
        }
    }

    private void SetTile(int y, int x, Tile[] tiles)
    {
        var pos = new Vector3Int(x + _widthOffset, -y + _heightOffset, 0);
        var tile = GetTile(tiles);
        _tilemap.SetTile(pos, tile);
    }

    private Tile GetTile(Tile[] variants)
    {
        if (variants.Length == 1) return variants[0];
        int index = UnityEngine.Random.Range(0, variants.Length);
        return variants[index];
    }

    #endregion
}