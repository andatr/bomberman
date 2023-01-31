using System;
using UnityEngine;

namespace Bomberman
{ 
    public class ExplosionModel : IExplosionlModel_View
    {
        #region Public

        public Vector2Int GridPosition { get { return _gridPosition; } }

        public int Strength { get { return _strength; } }

        public ExplosionDirection Direction { get { return _direction; } }

        public ExplosionModel(ExplosionPoolModel pool)
        {
            _pool = pool;
        }

        public void Place(Vector2Int position, ExplosionDirection direction, int range, float liveTime)
        {
            _gridPosition = position;
            _direction = direction;
            _strength = range;
            _time = 0.0f;
            _liveTime = liveTime;
            _spread = false;
        }

        public void ForceSpread()
        {
            if (_spread) return;
            _spread = true;
            _pool.SpreadExplosion(this, _gridPosition, _direction, _strength - 1, _liveTime - _time);
        }

        public bool Update(float deltaTime)
        {
            _time += deltaTime;
            if (_time > _pool.SpreadTime && !_spread) {
                ForceSpread();
            }
            return _time >= _liveTime;
        }

        #endregion

        #region Fields

        ExplosionPoolModel _pool;
        private Vector2Int _gridPosition;
        private ExplosionDirection _direction;
        private int _strength;
        private float _time;
        private float _liveTime;
        private bool _spread;

        #endregion
    }
}