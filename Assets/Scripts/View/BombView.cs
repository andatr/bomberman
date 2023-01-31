using UnityEngine;

namespace Bomberman
{
    public class BombView : MonoBehaviour
    {
        #region Public / Messages

        private void OnDestroy()
        {
            Unsubscribe();
        }

        public void Init(IBombModel_View bomb, IGrid grid)
        {
            _transform = this.GetComponentEx<Transform>();
            _sprite = this.GetComponentEx<SpriteRenderer>();
            _defaultColor = _sprite.color;
            gameObject.SetActive(false);
            Unsubscribe();
            _bomb = bomb;
            _grid = grid;
            if (_bomb != null) {
                _bomb.Placed += OnPlaced;
                _bomb.Exploded += OnExploded;
                _bomb.Triggered += OnTriggered;
            }
        }

        #endregion

        #region Private

        private void Unsubscribe()
        {
            if (_bomb != null) {
                _bomb.Placed -= OnPlaced;
                _bomb.Exploded -= OnExploded;
                _bomb.Triggered -= OnTriggered;
            }
        }

        private void OnPlaced(IBombModel_View bomb)
        {
            Vector2 worldPosition = _grid.CellToWorld(bomb.GridPosition);
            _transform.position = new Vector3(worldPosition.x, worldPosition.y, 0.0f);
            gameObject.SetActive(true);
        }

        private void OnExploded(IBombModel_View bomb)
        {
            _sprite.color = _defaultColor;
            gameObject.SetActive(false);
        }

        private void OnTriggered(IBombModel_View bomb)
        {
            _sprite.color = _triggeredColor;
        }

        #endregion

        #region Fields

        [SerializeField] private Color _triggeredColor;
        IBombModel_View _bomb;
        IGrid _grid;
        private Transform _transform;
        private SpriteRenderer _sprite;
        private Color _defaultColor;

        #endregion
    }
}