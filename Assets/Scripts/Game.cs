using UnityEngine;

public class Game : MonoBehaviour
{
    public GameRules rules;
    public GamefieldView gamefieldView;

    private Gamefield gamefield;
    private bool good;

    private void Awake()
    {
        good = true;
        if (gamefieldView == null)
        {
            good = false;
            Debug.LogError("Gamefield View component is not set", this);
        }
        if (rules == null)
        {
            good = false;
            Debug.LogError("Game Rules component is not set", this);
        }
    }

    void Start()
    {
        if (!good) return;
        gamefield = new Gamefield();
        gamefieldView.Gamefield = gamefield;
        NextLevel(0);
    }

    void NextLevel(int level)
    {
        gamefield.Generate(
            rules.gamefield.width,
            rules.gamefield.height,
            rules.gamefield.GetDensity(level)
        );
    }
}
