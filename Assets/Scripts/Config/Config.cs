using UnityEngine;

namespace Bomberman
{
    [CreateAssetMenu(fileName = "Config", menuName = "ScriptableObjects/Config", order = 1)]
    public class Config : ScriptableObject
    {
        public GamefieldConfig gamefield;
        public PlayerConfig player;
        public BombConfig bombs;
    }
}
