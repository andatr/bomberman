using UnityEngine;

namespace Bomberman
{
    public class BombController : MonoBehaviour
    {
        #region Public / Messages

        public void Init(IBombPoolModel_Controller bombPool, IGrid grid)
        {
            _bombPool = bombPool;
            var parent = transform;
            foreach (var bomb in bombPool.Bombs) {
                var bombView = Instantiate(_bombPrefab, parent).GetComponent<BombView>();
                bombView.Init(bomb, grid);
            }
        }

        private void FixedUpdate()
        {
            _bombPool.Update(Time.fixedDeltaTime);
        }

        #endregion

        #region Fields

        [SerializeField] private GameObject _bombPrefab;
        IBombPoolModel_Controller _bombPool;

        #endregion
    }
}