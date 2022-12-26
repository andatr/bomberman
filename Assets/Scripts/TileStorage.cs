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

    private Dictionary<Gamefield.CellType, Tile[]> _tiles;

    private void SetTiles()
    {
        _tiles = new Dictionary<Gamefield.CellType, Tile[]>();
        _tiles[Gamefield.CellType.Path]     = paths;
        _tiles[Gamefield.CellType.Border]   = borders;
        _tiles[Gamefield.CellType.Pillar]   = pillars;
        _tiles[Gamefield.CellType.Obstacle] = obstacles;
        foreach (var kvp in _tiles)
        {
            if (kvp.Value.Length == 0)
            {
                Debug.LogError($"Tiles for CellType {kvp.Key} are not set", this);
            }
        }
    }

    public Tile GetTile(Gamefield.CellType type)
    {
        if (_tiles == null) SetTiles();
        Tile[] variants = null;
        _tiles.TryGetValue(type, out variants);
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