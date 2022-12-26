using UnityEngine;
using UnityEngine.Tilemaps;

public class GamefieldView : MonoBehaviour
{
    public TileStorage tileStorage;
    public Tilemap pathTilemap;
    public Tilemap obstacleTilemap;

    private Gamefield _gamefield;
    private bool _good;

    public Gamefield Gamefield
    {
        set
        {
            Unsubscribe();
            _gamefield = value;
            _gamefield.OnGenerate += Generate;
        }
    }

    public Vector3 TileToWorld(Vector3Int tile)
    {
        tile.x -= _gamefield.Cells[0].Length / 2;
        tile.y -= _gamefield.Cells.Length / 2;
        var world = pathTilemap.CellToWorld(tile);
        world.x += pathTilemap.cellSize.x / 2.0f;
        world.y = -world.y + pathTilemap.cellSize.y / 2.0f;
        return world;
    }

    private void Awake()
    {
        _good = true;
        if (pathTilemap == null)
        {
            _good = false;
            Debug.LogError("Path Tilemap component is not set", this);
        }
        if (obstacleTilemap == null)
        {
            _good = false;
            Debug.LogError("Obstacle Tilemap component is not set", this);
        }
        if (tileStorage == null)
        {
            _good = false;
            Debug.LogError("Tile Storage component is not set", this);
        }
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Unsubscribe()
    {
        if (_gamefield == null) return;
        _gamefield.OnGenerate -= Generate;
    }

    private void Generate()
    {
        if (!_good) return;
        pathTilemap.ClearAllTiles();
        obstacleTilemap.ClearAllTiles();
        int height = _gamefield.Cells.Length;
        int width  = _gamefield.Cells[0].Length;
        int h_offset = -height / 2;
        int w_offset = -width  / 2;
        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                var cellType = _gamefield.Cells[i][j].type;
                var tile = tileStorage.GetTile(cellType);
                var pos = new Vector3Int(j + w_offset, i + h_offset, 0);
                if (cellType == Gamefield.CellType.Path)
                {
                    pathTilemap.SetTile(pos, tile);
                }
                else
                {
                    obstacleTilemap.SetTile(pos, tile);
                }
            }
        }
    }
}