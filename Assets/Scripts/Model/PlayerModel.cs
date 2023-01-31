using System;
using UnityEngine;

namespace Bomberman
{
    public class PlayerModel :
        IPlayerModel_Controller,
        ICharacterModel_View
    {
        #region Public

        public PlayerModel(PlayerConfig config, BombPoolModel bombs)
        {
            _bombs = bombs;
            _speed = config.speed;
            _maxBombs = config.maxBombs;
            _bombLiveTime = config.bombLiveTime;
            _bombRange = config.bombRange;
            foreach (var bomb in _bombs.Bombs) {
                bomb.Exploded += BombExploded;
            }
        }

        public Vector2 Position { 
            get { return _position; }
            set {
                _position = value;
                Moved?.Invoke(_position);
            }
        }

        public float Speed { get { return _speed; } }

        public Vector2 Direction { 
            get { return _direction; }
            set { 
                _direction = value;
                DirectionChanged?.Invoke(_direction);
            }
        }

        public float BombLiveTime { get { return _bombLiveTime; } }

        public int BombRange { get { return _bombRange; } }

        public bool PlaceBomb(Vector2Int gridPosition)
        {
            if (_bombsPlaced >= _maxBombs) return false;
            if (_bombs.PlaceBomb(gridPosition, this)) {
                ++_bombsPlaced;
                return true;
            }
            return false;
        }

        public event Action<Vector2> Moved;

        public event Action<Vector2> DirectionChanged;

        public event Action Died;

        #endregion

        #region Private

        private void BombExploded(IBombModel_View bomb)
        {
            if (bomb.Player == this) {
                --_bombsPlaced;
            }
        }

        #endregion

        #region Fields

        BombPoolModel _bombs;
        private Vector2 _position;
        private float _speed;
        private Vector2 _direction;
        private int _bombsPlaced;
        private int _maxBombs;
        private float _bombLiveTime;
        private int _bombRange;

        #endregion
    }
}