using System;
using UnityEngine;

namespace Bomberman
{ 
    public class BombPoolModel : IBombPoolModel_Controller
    {
        #region Public

        public IBombModel_View[] Bombs { get { return _bombs.Items; } }

        public BombPoolModel(BombConfig config, GamefieldModel gamefield, ExplosionPoolModel explosions)
        {
            _gamefield = gamefield;
            _explosions = explosions;
            Func<int, BombModel> bombFactory = (int index) => new BombModel();
            _bombs = new Pool<BombModel>(config.bombTotalLimit, bombFactory);
        }

        public bool PlaceBomb(Vector2Int gridPosition, PlayerModel player)
        {
            var cell = _gamefield.Cell(gridPosition);
            if (cell.Bomb != null || cell.CellType != CellType.Path) return false;
            BombModel bomb = _bombs.GetItem();
            if (bomb == null) return false;
            if (!bomb.Place(gridPosition, player)) return false;
            cell.Bomb = bomb;
            return true;
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < _bombs.Size; ++i) {
                var bomb = _bombs.Items[i];
                if (bomb.Update(deltaTime)) {
                    _gamefield.Cell(bomb.GridPosition).Bomb = null;
                    _explosions.PlaceExplosion(bomb.GridPosition, bomb.Range);
                    i = _bombs.Release(i);
                }
            }
        }

        #endregion

        #region Fields

        private GamefieldModel _gamefield;
        private Pool<BombModel> _bombs;
        private ExplosionPoolModel _explosions;

        #endregion
    }
}