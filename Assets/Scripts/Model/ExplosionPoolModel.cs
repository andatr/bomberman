using System;
using UnityEngine;

namespace Bomberman
{ 
    public class ExplosionPoolModel : IExplosionlPoolModel_Controller
    {
        #region Public

        public float LiveTime { get { return _liveTime; } }

        public float SpreadTime { get { return _spreadTime; } }

        public IExplosionlModel_View[] Explosions { get { return _explosions.Items; } }

        public ExplosionPoolModel(BombConfig config, GamefieldModel gamefield)
        {
            _liveTime = config.explosionLiveTime;
            _spreadTime = config.explosionSpreadTime;
            _gamefield = gamefield;
            Func<int, ExplosionModel> factory = (int index) => new ExplosionModel(this);
            _explosions = new Pool<ExplosionModel>(CalcCapacity(), factory);
        }

        public void PlaceExplosion(Vector2Int gridPosition, int strength)
        {
            var cell = _gamefield.Cell(gridPosition);
            if (cell.CellType != CellType.Path) return;
            ExplosionModel explosion = null;
            if (cell.Explosion != null) {
                explosion = cell.Explosion;
                explosion.ForceSpread();
                Removed?.Invoke(explosion);
            }
            else {
                explosion = _explosions.GetItem();
                if (explosion == null) return;
                cell.Explosion = explosion;
            }
            explosion.Place(gridPosition, ExplosionDirection.All, strength - 1, _liveTime);
            Placed?.Invoke(explosion);
        }

        public void SpreadExplosion(ExplosionModel origin, Vector2Int from, ExplosionDirection direction, int strength, float liveTime)
        {
            if (strength < 0) return;
            if (direction == ExplosionDirection.All) {
                SpreadExplosion(origin, from, ExplosionDirection.Top,    strength, liveTime);
                SpreadExplosion(origin, from, ExplosionDirection.Right,  strength, liveTime);
                SpreadExplosion(origin, from, ExplosionDirection.Bottom, strength, liveTime);
                SpreadExplosion(origin, from, ExplosionDirection.Left,   strength, liveTime);
                return;
            }
            var nextPosition = GetNextPosition(from, direction);
            if (nextPosition.x < 0 || nextPosition.y < 0) return;
            var cell = _gamefield.Cell(nextPosition);
            if (cell.CellType != CellType.Obstacle && cell.CellType != CellType.Path) return;
            if (cell.CellType == CellType.Obstacle) {
                strength = 0;
            }
            ExplosionModel explosion = null;
            if (cell.Explosion != null) {
                explosion = cell.Explosion;
                explosion.ForceSpread();
                Removed?.Invoke(explosion);
            }
            else {
                explosion = _explosions.GetItem();
                if (explosion == null) return;
                cell.Explosion = explosion;
            }
            if (cell.Bomb != null) {
                cell.Bomb.Trigger();
            }
            Spread?.Invoke(origin);
            explosion.Place(nextPosition, direction, strength, liveTime);
            Placed?.Invoke(explosion);
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < _explosions.Size; ++i) {
                var explosion = _explosions.Items[i];
                if (explosion.Update(deltaTime)) {
                    var cell = _gamefield.Cell(explosion.GridPosition);
                    cell.Explosion = null;
                    if (cell.CellType == CellType.Obstacle) {
                        _gamefield.ClearCell(explosion.GridPosition);
                    }
                    Removed?.Invoke(explosion);
                    i = _explosions.Release(i);
                }
            }
        }

        public event Action<IExplosionlModel_View> Placed;

        public event Action<IExplosionlModel_View> Spread;

        public event Action<IExplosionlModel_View> Removed;

        #endregion

        #region Private 

        private Vector2Int GetNextPosition(Vector2Int from, ExplosionDirection direction)
        {
            switch (direction) {
                case ExplosionDirection.Left:   return new Vector2Int(from.x - 1, from.y);
                case ExplosionDirection.Right:  return new Vector2Int(from.x + 1, from.y);
                case ExplosionDirection.Top:    return new Vector2Int(from.x, from.y - 1);
                case ExplosionDirection.Bottom: return new Vector2Int(from.x, from.y + 1);
            }
            return new Vector2Int(-1, -1);
        }

        private int CalcCapacity()
        {
            int total = (_gamefield.Width - 2) * (_gamefield.Height - 2);
            int columnVertical = (_gamefield.Height - 3) / 2;
            int columnHorizontal = (_gamefield.Width - 3) / 2;
            return total - columnVertical * columnHorizontal;
        }

        #endregion

        #region Fields

        private float _liveTime;
        private float _spreadTime;
        private GamefieldModel _gamefield;
        private Pool<ExplosionModel> _explosions;

        #endregion
    }
}