using UnityEngine;

public class Game : MonoBehaviour
{
    public GameRules rules;
    public Gamefield gamefield;
    public PlayerController player;

    private bool _good;

    private void Awake()
    {
        _good = true;
        if (gamefield == null)
        {
            _good = false;
            Debug.LogError("Gamefield component is not set", this);
        }
        if (rules == null)
        {
            _good = false;
            Debug.LogError("Game Rules component is not set", this);
        }
        if (player == null)
        {
            _good = false;
            Debug.LogError("Player Controller component is not set", this);
        }
    }

    private void Start()
    {
        if (!_good) return;
        NextLevel(0);
    }

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
}
