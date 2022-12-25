using UnityEngine;
using UnityEngine.Tilemaps;

public class GamefieldView : MonoBehaviour
{
    public TileStorage tileStorage;
    public Tilemap tilemap;
    private Gamefield gamefield;
    private bool good;

    public Gamefield Gamefield {
        set
        {
            if (gamefield != null)
            {
                gamefield.OnGenerate -= Generate;
            }
            gamefield = value;
            gamefield.OnGenerate += Generate;
        }
    }

    private void Awake()
    {
        good = true;
        if (tilemap == null)
        {
            good = false;
            Debug.LogError("Tilemap component is not set", this);
        }
        if (tileStorage == null)
        {
            good = false;
            Debug.LogError("Tile Storage component is not set", this);
        }
    }

    private void OnDestroy()
    {
        if (gamefield != null)
        {
            gamefield.OnGenerate -= Generate;
        }
    }

    private void Generate()
    {
        if (!good) return;
        tilemap.ClearAllTiles();
        int height = gamefield.Cells.Length;
        int width  = gamefield.Cells[0].Length;
        int h_offset = -height / 2;
        int w_offset = -width  / 2;
        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                var tile = tileStorage.GetTile(gamefield.Cells[i][j].type);
                var pos = new Vector3Int(j + w_offset, i + h_offset, 0);
                tilemap.SetTile(pos, tile);
            }
        }
    }
}