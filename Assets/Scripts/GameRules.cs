using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameRules", menuName = "ScriptableObjects/GameRules", order = 2)]
public class GameRules : ScriptableObject
{
    [Serializable]
    public class Gamefield
    {
        public int width;
        public int height;
        public float density;

        public float GetDensity(float level)
        {
            return density + 0.1f * level;
        }
    }

    [Serializable]
    public class Player
    {
        public float speed;
    }

    public Gamefield gamefield;
    public Player player;
}
