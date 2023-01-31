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
            CheckComponents();
            Unsubscribe();
            _bomb = bomb;
            _grid = grid;
            if (_bomb != null) {
                _bomb.Placed += OnPlace;
                _bomb.Exploded += OnExplode;
            }
        }

        #endregion

        #region Private

        private void CheckComponents()
        {
            _transform = GetComponent<Transform>();
            if (_transform == null) {
                enabled = false;
                Debug.LogError("Transform component not found", this);
            }
            gameObject.SetActive(false);
        }

        private void Unsubscribe()
        {
            if (_bomb != null) {
                _bomb.Placed -= OnPlace;
                _bomb.Exploded -= OnExplode;
            }
        }

        private void OnPlace(IBombModel_View bomb)
        {
            Vector2 worldPosition = _grid.CellToWorld(bomb.GridPosition);
            _transform.position = new Vector3(worldPosition.x, worldPosition.y, 0.0f);
            gameObject.SetActive(true);
        }

        private void OnExplode(IBombModel_View bomb)
        {
            gameObject.SetActive(false);
        }

        #endregion

        #region Fields

        IBombModel_View _bomb;
        IGrid _grid;
        private Transform _transform;

        #endregion
    }
}