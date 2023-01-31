using System;
using UnityEngine;

namespace Bomberman
{
    public class BombModel : IBombModel_View
    {
        #region Public

        public ICharacterModel_View Player { get { return _player; } }

        public Vector2Int GridPosition { get { return _gridPosition; } }

        public float Time { get { return _time; } }

        public float LiveTime { get { return _player.BombLiveTime; } }

        public int Range { get { return _player.BombRange; } }

        public bool Place(Vector2Int gridPosition, PlayerModel player)
        {
            _player = player;
            _gridPosition = gridPosition;
            _time = 0.0f;
            Placed?.Invoke(this);
            return true;
        }

        public bool Update(float deltaTime)
        {
            _time += deltaTime;
            if (_time >= _player.BombLiveTime) {
                _time = _player.BombLiveTime;
                Exploded?.Invoke(this);
                return true;
            }
            return false;
        }

        public event Action<IBombModel_View> Placed;
        public event Action<IBombModel_View> Exploded;

        #endregion

        #region Fields

        private PlayerModel _player;
        private Vector2Int _gridPosition;
        private float _time;

        #endregion
    }
}