using UnityEngine;
using UnityEngine.Tilemaps;

namespace Bomberman
{
    public class TilemapHelper
    {
        #region Public

        public TilemapHelper(Tilemap tilemap, int width, int height)
        {
            _tilemap = tilemap;
            _widthOffset = -width / 2;
            _heightOffset = height / 2;
        }

        public void SetTiles(Vector2Int position, TileBase[] tiles)
        {
            TileBase tile = GetTile(tiles);
            SetTile(position, tile);
        }

        public void SetTiles(int x, int y, TileBase[] tiles)
        {
            SetTiles(new Vector2Int(x, y), tiles);
        }

        public void SetTile(Vector2Int position, TileBase tile)
        {
            Vector3Int pos = CellLogicToView(position);
            _tilemap.SetTile(pos, tile);
        }

        public void SetTile(int x, int y, TileBase tile)
        {
            SetTile(new Vector2Int(x, y), tile);
        }

        public Vector3Int CellLogicToView(Vector2Int position)
        {
            return new Vector3Int(position.x + _widthOffset, -position.y + _heightOffset, 0);
        }

        public Vector2Int CellViewToLogic(Vector3Int position)
        {
            return new Vector2Int(position.x - _widthOffset, -position.y + _heightOffset);
        }

        #endregion

        #region Private

        private TileBase GetTile(TileBase[] variants)
        {
            if (variants.Length == 1) return variants[0];
            int index = UnityEngine.Random.Range(0, variants.Length);
            return variants[index];
        }

        #endregion

        #region Fields

        private Tilemap _tilemap;
        private int _widthOffset;
        private int _heightOffset;

        #endregion
    }
}
