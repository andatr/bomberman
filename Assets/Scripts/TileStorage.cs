using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TileStorage", menuName = "ScriptableObjects/TileStorage", order = 1)]
public class TileStorage : ScriptableObject
{
    public Tile[] paths;
    public Tile[] borders;
    public Tile[] pillars;
    public Tile[] obstacles;

    private Dictionary<Gamefield.CellType, Tile[]> tiles;

    private void SetTiles()
    {
        tiles = new Dictionary<Gamefield.CellType, Tile[]>();
        tiles[Gamefield.CellType.Path]     = paths;
        tiles[Gamefield.CellType.Border]   = borders;
        tiles[Gamefield.CellType.Pillar]   = pillars;
        tiles[Gamefield.CellType.Obstacle] = obstacles;
        foreach (var kvp in tiles)
        {
            if (kvp.Value.Length == 0)
            {
                Debug.LogError($"Tiles for CellType {kvp.Key} are not set", this);
            }
        }
    }

    public Tile GetTile(Gamefield.CellType type)
    {
        if (tiles == null) SetTiles();
        Tile[] variants = null;
        tiles.TryGetValue(type, out variants);
        if (variants == null)
        {
            Debug.LogError($"No tiles of type {type} found", this);
            return null;
        }
        if (variants.Length == 1) return variants[0];
        int index = Random.Range(0, variants.Length);
        return variants[index];
    }
}