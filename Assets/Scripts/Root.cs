using UnityEngine;

namespace Bomberman
{
    public class Root : MonoBehaviour
    {
        #region Public / Messages

        private void Awake()
        {
            if (!CheckComponents()) return;
            (_gamefieldModel, _grid) = SetupGamefield(_gamefield, _config.gamefield);
            _explosionPoolModel = SetupExplosions(_explosions, _gamefieldModel, _config.bombs);
            _bombPoolModel = SetupBombs(_bombs, _gamefieldModel, _explosionPoolModel, _grid, _config.bombs);
            _playerModel = SetupPlayer(_player, _bombPoolModel, _grid, _config.player);            
            NewLevel(0);
        }

        #endregion

        #region Private

        public void NewLevel(int level)
        {
            _gamefieldModel.Generate(level);
            _playerModel.Position = _grid.CellToWorld(new Vector2Int(1, 1));
        }

        private bool CheckComponents()
        {
            if (_config == null) {
                enabled = false;
                Debug.LogError("Config not set", this);
            }
            if (_gamefield == null) {
                enabled = false;
                Debug.LogError("Gamefield not set", this);
            }
            if (_player == null) {
                enabled = false;
                Debug.LogError("Player not set", this);
            }
            if (_bombs == null) {
                enabled = false;
                Debug.LogError("Bombs not set", this);
            }
            if (_explosions == null) {
                enabled = false;
                Debug.LogError("Explosions not set", this);
            }
            return enabled;
        }

        private PlayerModel SetupPlayer(GameObject player, BombPoolModel bombs, IGrid grid, PlayerConfig config)
        {
            var model = new PlayerModel(config, bombs);
            PlayerController controller = player.GetComponent<PlayerController>();
            if (controller == null) {
                enabled = false;
                Debug.LogError("PlayerController component not found", this);
            }
            else {
                controller.Init(model, grid);
            }
            CharacterView view = player.GetComponent<CharacterView>();
            if (view == null) {
                enabled = false;
                Debug.LogError("CharacterView component not found", this);
            }
            else {
                view.Init(model);
            }
            return model;
        }

        private (GamefieldModel, IGrid) SetupGamefield(GameObject gamefield, GamefieldConfig config)
        {
            var model = new GamefieldModel(config);
            GamefieldView view = gamefield.GetComponent<GamefieldView>();
            if (view == null) {
                enabled = false;
                Debug.LogError("GamefieldView component not found", this);
            }
            else {
                view.Init(model);
            }
            return (model, view);
        }

        private ExplosionPoolModel SetupExplosions(GameObject explosions, GamefieldModel gamefield, BombConfig config)
        {
            var model = new ExplosionPoolModel(config, gamefield);
            ExplosionController controller = explosions.GetComponent<ExplosionController>();
            if (controller == null) {
                enabled = false;
                Debug.LogError("Explosion Controller component not found", this);
            }
            else {
                controller.Init(model, gamefield);
            }
            return model;
        }

        private BombPoolModel SetupBombs(GameObject bombs, GamefieldModel gamefield, ExplosionPoolModel explosions, IGrid grid, BombConfig config)
        {
            var model = new BombPoolModel(config, gamefield, explosions);
            BombController controller = bombs.GetComponent<BombController>();
            if (controller == null) {
                enabled = false;
                Debug.LogError("BombController component not found", this);
            }
            else {
                controller.Init(model, grid);
            }
            return model;
        }

        #endregion

        #region Fields

        [SerializeField] private Config _config;
        [SerializeField] private GameObject _gamefield;
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _explosions;
        [SerializeField] private GameObject _bombs;
        private IGrid _grid;
        private GamefieldModel _gamefieldModel;
        private PlayerModel _playerModel;
        private ExplosionPoolModel _explosionPoolModel;
        private BombPoolModel _bombPoolModel;

        #endregion
    }
}
