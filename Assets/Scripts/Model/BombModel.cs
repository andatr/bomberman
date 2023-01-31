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

        public BombModel(BombConfig config)
        {
            _triggerTime = config.bombTriggerTime;
        }

        public bool Place(Vector2Int gridPosition, PlayerModel player)
        {
            _player = player;
            _gridPosition = gridPosition;
            _time = 0.0f;
            _liveTime = _player.BombLiveTime;
            Placed?.Invoke(this);
            return true;
        }

        public void Trigger()
        {
            float newLiveTime = _time + _triggerTime;
            _liveTime = Mathf.Min(_liveTime, newLiveTime);
            Triggered?.Invoke(this);
        }

        public bool Update(float deltaTime)
        {
            _time += deltaTime;
            if (_time >= _liveTime) {
                Exploded?.Invoke(this);
                return true;
            }
            return false;
        }

        public event Action<IBombModel_View> Placed;
        public event Action<IBombModel_View> Exploded;
        public event Action<IBombModel_View> Triggered;

        #endregion

        #region Fields

        private PlayerModel _player;
        private Vector2Int _gridPosition;
        private float _time;
        private float _liveTime;
        private float _triggerTime;

        #endregion
    }
}