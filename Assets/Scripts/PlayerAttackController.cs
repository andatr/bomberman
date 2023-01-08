using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameRules _rules;

    [SerializeField]
    private Gamefield _gamefield;

    private int _bombsPlaced;

    #endregion

    #region Public / Messages

    private void Awake()
    {
        CheckComponents();
        Init();
    }

    private void OnAttack(InputValue value)
    {
        Vector2Int tile = _gamefield.WorldToTile(transform.position);

        Debug.LogWarning(tile);
    }

    #endregion

    #region Private

    private void CheckComponents()
    {
        CheckGameRules();
        CheckGamefield();
    }

    private void Init()
    {
        _bombsPlaced = 0;
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

    #endregion
}
