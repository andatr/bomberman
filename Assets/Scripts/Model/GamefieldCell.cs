namespace Bomberman
{
    public class GamefieldCell
    {
        #region Public

        public CellType CellType {
            get { return _cellType;  }
            set { _cellType = value; }
        }

        public ExplosionModel Explosion
        {
            get { return _explosion; }
            set { _explosion = value; }
        }

        public BombModel Bomb
        {
            get { return _bomb; }
            set { _bomb = value; }
        }

        #endregion

        #region Fields

        private CellType _cellType;

        private ExplosionModel _explosion;

        private BombModel _bomb;

        #endregion
    }
}