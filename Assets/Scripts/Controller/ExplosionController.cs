using UnityEngine;
using UnityEngine.Tilemaps;

namespace Bomberman
{
    public class ExplosionController : MonoBehaviour
    {
        #region Public / Messages

        private void OnDestroy()
        {
            Unsubscribe();
        }

        public void Init(IExplosionlPoolModel_Controller explosionPool, IGamefieldModel_View gamefield)
        {
            CheckComponents();
            Unsubscribe();
            _gamefield = gamefield;
            _explosionPool = explosionPool;
            if (_explosionPool != null && _gamefield != null) {
                _tilemapHelper = new TilemapHelper(_tilemap, _gamefield.Width, _gamefield.Height);
                _explosionPool.Placed += ExplosionPlaced;
                _explosionPool.Removed += ExplosionRemoved;
                _explosionPool.Spread += ExplosionSpread;
            }
        }

        private void FixedUpdate()
        {
            _explosionPool.Update(Time.fixedDeltaTime);
        }

        #endregion

        #region Private

        private void ExplosionPlaced(IExplosionlModel_View explosion)
        {
            if (explosion.Direction == ExplosionDirection.All) {
                _tilemapHelper.SetTile(explosion.GridPosition, _explosionMiddle);
            }
            else if (_gamefield.Cell(explosion.GridPosition).CellType == CellType.Obstacle) {
                _tilemapHelper.SetTile(explosion.GridPosition, _explosionObstacle);
            }
            else {
                RotatedTile tile = null;
                switch (explosion.Direction) {
                    case ExplosionDirection.Left:
                        tile = _explosionLeftEnd;
                        break;
                    case ExplosionDirection.Right:
                        tile = _explosionRightEnd;
                        break;
                    case ExplosionDirection.Top:
                        tile = _explosionTopEnd;
                        break;
                    case ExplosionDirection.Bottom:
                        tile = _explosionBottomEnd;
                        break;
                }
                SetTile(explosion.GridPosition, tile);
            }
        }

        private void ExplosionSpread(IExplosionlModel_View explosion)
        {
            RotatedTile tile = null;
            switch (explosion.Direction) {
                case ExplosionDirection.Left:
                    tile = _explosionLeft;
                    break;
                case ExplosionDirection.Right:
                    tile = _explosionRight;
                    break;
                case ExplosionDirection.Top:
                    tile = _explosionTop;
                    break;
                case ExplosionDirection.Bottom:
                    tile = _explosionBottom;
                    break;
            }
            SetTile(explosion.GridPosition, tile);
        }

        private void SetTile(Vector2Int gridPosition, RotatedTile tile)
        {
            if (tile == null) return;
            TileChangeData changeData = new();
            changeData.position = _tilemapHelper.CellLogicToView(gridPosition);
            changeData.color = Color.white;
            changeData.tile = tile.tile;
            changeData.transform = Matrix4x4.Rotate(Quaternion.Euler(tile.rotation));
            _tilemap.SetTile(changeData, true);
        }

        private void ExplosionRemoved(IExplosionlModel_View explosion)
        {
            _tilemapHelper.SetTile(explosion.GridPosition, null);
        }

        private void CheckComponents()
        {
            CheckComponent(_tilemap, "Tilemap");
            CheckComponent(_explosionMiddle, "Explosion Middle");
            CheckComponent(_explosionLeft, "Explosion Left");
            CheckComponent(_explosionRight, "Explosion Right");
            CheckComponent(_explosionTop, "Explosion Top");
            CheckComponent(_explosionBottom, "Explosion Bottom");
            CheckComponent(_explosionLeftEnd, "Explosion Left End");
            CheckComponent(_explosionRightEnd, "Explosion Right End");
            CheckComponent(_explosionTopEnd, "Explosion Top End");
            CheckComponent(_explosionBottomEnd, "Explosion Bottom End");
            CheckComponent(_explosionObstacle, "Explosion Obstacle");
        }

        private void Unsubscribe()
        {
            if (_explosionPool != null) {
                _explosionPool.Placed -= ExplosionPlaced;
                _explosionPool.Removed -= ExplosionRemoved;
                _explosionPool.Spread -= ExplosionSpread;
            }
        }

        private void CheckComponent(Object component, string name)
        {
            if (component == null) {
                enabled = false;
                Debug.LogError($"{name} not set", this);
            }
        }

        #endregion

        #region Fields

        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private AnimatedTile _explosionMiddle;
        [SerializeField] private AnimatedTile _explosionObstacle;
        [SerializeField] private RotatedTile _explosionLeft;
        [SerializeField] private RotatedTile _explosionRight;
        [SerializeField] private RotatedTile _explosionTop;
        [SerializeField] private RotatedTile _explosionBottom;
        [SerializeField] private RotatedTile _explosionLeftEnd;
        [SerializeField] private RotatedTile _explosionRightEnd;
        [SerializeField] private RotatedTile _explosionTopEnd;
        [SerializeField] private RotatedTile _explosionBottomEnd;
        private TilemapHelper _tilemapHelper;
        private IExplosionlPoolModel_Controller _explosionPool;
        private IGamefieldModel_View _gamefield;

        #endregion
    }
}