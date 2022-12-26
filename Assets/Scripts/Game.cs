using UnityEngine;

public class Game : MonoBehaviour
{
    public GameRules rules;
    public GamefieldView gamefieldView;
    public PlayerController player;

    private Gamefield _gamefield;
    private bool _good;

    private void Awake()
    {
        _good = true;
        if (gamefieldView == null)
        {
            _good = false;
            Debug.LogError("Gamefield View component is not set", this);
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
        _gamefield = new Gamefield();
        gamefieldView.Gamefield = _gamefield;
        NextLevel(0);
    }

    private void NextLevel(int level)
    {
        _gamefield.Generate(
            rules.gamefield.width,
            rules.gamefield.height,
            rules.gamefield.GetDensity(level)
        );
        player.Position = gamefieldView.TileToWorld(new Vector3Int(1, 1, 0));
        player.Speed = rules.player.speed;
    }
}
