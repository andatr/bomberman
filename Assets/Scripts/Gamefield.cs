using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Gamefield : MonoBehaviour
{
    #region Unity Messages

    public Tilemap tilemap;
    public Tile[] pathTiles;
    public Tile[] pillarTiles;
    public Tile[] obstacleTiles;
    public Tile[] borderTopTiles;
    public Tile[] borderBottomTiles;
    public Tile[] borderLeftTiles;
    public Tile[] borderRightTiles;
    public Tile[] cornerTopLeftTiles;
    public Tile[] cornerTopRightTiles;
    public Tile[] cornerBottomLeftTiles;
    public Tile[] cornerBottomRightTiles;

    private void Awake()
    {
        _good = true;
        if (tilemap == null)
        {
            _good = false;
            Debug.LogError("Tilemap component is not set", this);
        }
        CheckTiles(pathTiles, "Path");
        CheckTiles(pillarTiles, "Pillart");
        //CheckTiles(obstacleTiles, "Obstacle");
        CheckTiles(borderTopTiles, "Top border");
        CheckTiles(borderBottomTiles, "Bottom border");
        CheckTiles(borderLeftTiles, "Left border");
        CheckTiles(borderRightTiles, "Right border");
        CheckTiles(cornerTopLeftTiles, "Top-Left corner");
        CheckTiles(cornerTopRightTiles, "Top-Right corner");
        CheckTiles(cornerBottomLeftTiles, "Bottom-Left corner");
        CheckTiles(cornerBottomRightTiles, "Bottom-right corner");
    }

    #endregion

    #region Public

    public Vector3 TileToWorld(Vector3Int tilePosition)
    {
        if (!_good) return new Vector3();
        tilePosition.x -= _cells[0].Length / 2;
        tilePosition.y -= _cells.Length / 2;
        var worldPosition = tilemap.CellToWorld(tilePosition);
        worldPosition.x += tilemap.cellSize.x / 2.0f;
        worldPosition.y = -worldPosition.y + tilemap.cellSize.y / 2.0f;
        return worldPosition;
    }

    public Vector3Int WorldToTile(Vector3 worldPosition)
    {
        if (!_good) return new Vector3Int();
        var tilePosition = tilemap.WorldToCell(worldPosition);
        tilePosition.x += _cells[0].Length / 2;
        tilePosition.y += _cells.Length / 2;
        return tilePosition;
    }

    public void Generate(int width, int height, float density)
    {
        if (!_good) return;
        ValidateSize(ref width);
        ValidateSize(ref height);
        AllocateCells(width, height);
        GenerateLogic(width, height, density);
        GenerateView(width, height);
    }

    #endregion

    #region Private

    private void CheckTiles(Tile[] tiles, string name)
    {
        if (tiles == null || tiles.Length == 0)
        {
            _good = false;
            Debug.LogError($"{name} tiles are not set", this);
        }
    }

    private void ValidateSize(ref int size)
    {
        if (size < 5)
        {
            Debug.LogWarning($"Gamefield: gamefield size {size} must be > 5");
            size = 5;
        }
        if (size % 2 == 0)
        {
            Debug.LogWarning($"Gamefield: gamefield size {size} must be an odd number");
            size += 1;
        }
    }

    private void AllocateCells(int width, int height)
    {
        if (_cells != null && height == _cells.Length && width == _cells[0].Length) return;
        _cells = new Cell[height][];
        for (int i = 0; i < height; ++i)
        {
            _cells[i] = new Cell[width];
            for (int j = 0; j < width; ++j)
            {
                _cells[i][j] = new Cell();
            }
        }
        tilemap.ClearAllTiles();
    }

    private void GenerateLogic(int width, int height, float density)
    {
        density = 1.0f - density;
        int h = height - 1;
        int w = width  - 1;
        // top and bottom borders
        for (int i = 0; i < width; ++i)
        {
            _cells[0][i].type = CellType.Border;
            _cells[h][i].type = CellType.Border;
        }
        // left and right borders
        for (int i = 0; i < height; ++i)
        {
            _cells[i][0].type = CellType.Border;
            _cells[i][w].type = CellType.Border;
        }
        // pillars, paths and obstacles
        for (int i = 1; i < h; ++i)
        {
            for (int j = 1; j < w; ++j)
            {
                if (i % 2 == 0 && j % 2 == 0)
                {
                    _cells[i][j].type = CellType.Border;
                }
                else if (UnityEngine.Random.Range(0.0f, 1.0f) > density)
                {
                    _cells[i][j].type = CellType.Obstacle;
                }
                else
                {
                    _cells[i][j].type = CellType.Path;
                }
            }
        }
    }

    private void GenerateView(int width, int height)
    {
        int h = height - 1;
        int w = width - 1;
        int h_offset = -height / 2;
        int w_offset = -width / 2;
        Action<int, int, Tile[]> setTile = (i, j, tiles) =>
        {
            var pos = new Vector3Int(j + w_offset, i + h_offset, 0);
            var tile = GetTile(tiles);
            tilemap.SetTile(pos, tile);
        };
        // corners
        setTile(0, 0, cornerTopRightTiles);
        setTile(0, w, cornerTopLeftTiles);
        setTile(h, 0, cornerBottomRightTiles);
        setTile(h, w, cornerBottomLeftTiles);
        // top and bottom borders
        for (int i = 1; i < w; ++i)
        {
            setTile(0, i, borderTopTiles);
            setTile(h, i, borderBottomTiles);
        }
        // left and right borders
        for (int i = 1; i < h; ++i)
        {
            setTile(i, 0, borderLeftTiles);
            setTile(i, w, borderRightTiles);
        }
        // pillars, paths and obstacles
        for (int i = 1; i < h; ++i)
        {
            for (int j = 1; j < w; ++j)
            {
                if (_cells[i][j].type == CellType.Border)
                {
                    setTile(i, j, pillarTiles);
                }
                else
                {
                    setTile(i, j, pathTiles);
                }
            }
        }
    }

    private Tile GetTile(Tile[] variants)
    {
        if (variants.Length == 1) return variants[0];
        int index = UnityEngine.Random.Range(0, variants.Length);
        return variants[index];
    }

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

    private Cell[][] _cells { get; set; }
    private bool _good;

    #endregion
}