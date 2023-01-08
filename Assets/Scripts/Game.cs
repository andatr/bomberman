using UnityEngine;

public class Game : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameRules _rules;

    [SerializeField]
    private Gamefield _gamefield;

    [SerializeField]
    private PlayerMovementController _player;

    private int _level = -1;

    #endregion

    #region Public / Messages

    private void Awake()
    {
        CheckComponents();
    }

    private void Start()
    {
        NewGame();
    }

    #endregion

    #region Private

    private void CheckComponents()
    {
        if (_gamefield == null) {
            enabled = false;
            Debug.LogError("Gamefield component not set", this);
        }
        if (_rules == null) {
            enabled = false;
            Debug.LogError("Game Rules component not set", this);
        }
        if (_player == null) {
            enabled = false;
            Debug.LogError("Player Movement Controller component not set", this);
        }
    }

    private void NewGame()
    {
        _level = -1;
        NextLevel();
    }

    private void NextLevel()
    {
        ++_level;
        _gamefield.Generate(_level);
        _player.Position = _gamefield.TileToWorld(new Vector3Int(1, 1, 0));
    }

    #endregion
}
