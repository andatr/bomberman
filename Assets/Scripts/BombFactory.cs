using UnityEngine;

public class BombFactory : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameRules _rules;

    [SerializeField]
    private Gamefield _gamefield;

    [SerializeField]
    private GameObject _bombPrefab;

    private GameObject[] _bombs;

    #endregion

    #region Public / Messages

    private void Awake()
    {
        CheckComponents();
        Init();
    }

    public void PlaceBomb(Vector3 position)
    {
        Vector2Int tile = _gamefield.WorldToTile(position);
    }

    #endregion

    #region Private

    void Init()
    {
        int columnsHorizontal = (_gamefield.Width - 3) / 2;
        int columnsVertical = (_gamefield.Height - 3) / 2;
        int maxBombs = _gamefield.Width * _gamefield.Height - columnsHorizontal * columnsVertical;
        //_bombs = new GameObject[]
        //for (int i = 0; i < maxBombs; ++i) {
        //    _bombs = Object.Instantiate(bombPrefab,
        //}
    }

    private void CheckComponents()
    {
        CheckGameRules();
        CheckGamefield();
        CheckBombPrefab();
    }

    private void CheckGameRules()
    {
        if (_rules == null) {
            enabled = false;
            Debug.LogError("Game Rules component not set", this);
        }
    }

    private void CheckGamefield()
    {
        if (_gamefield == null) {
            enabled = false;
            Debug.LogError("Gamefield component not set", this);
        }
    }

    private void CheckBombPrefab()
    {
        if (_bombPrefab == null) {
            enabled = false;
            Debug.LogError("Bomb Prefab not set", this);
        }
    }

    #endregion
}
