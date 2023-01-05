using UnityEngine;

public class Game : MonoBehaviour
{
    #region Unity Messages

    [SerializeField]
    private GameRules rules;

    [SerializeField]
    private Gamefield gamefield;

    [SerializeField]
    private PlayerMovementController player;

    private void Awake()
    {
        if (gamefield == null)
        {
            enabled = false;
            Debug.LogError("Gamefield component is not set", this);
        }
        if (rules == null)
        {
            enabled = false;
            Debug.LogError("Game Rules component is not set", this);
        }
        if (player == null)
        {
            enabled = false;
            Debug.LogError("Player Movement Controller component is not set", this);
        }
    }

    private void Start()
    {
        NextLevel(0);
    }

    #endregion

    #region Private

    private void NextLevel(int level)
    {
        gamefield.Generate(
            rules.gamefield.width,
            rules.gamefield.height,
            rules.gamefield.GetDensity(level)
        );
        player.Position = gamefield.TileToWorld(new Vector3Int(1, 1, 0));
        player.Speed = rules.player.speed;
    }

    #endregion
}
