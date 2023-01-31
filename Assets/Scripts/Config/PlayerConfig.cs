using UnityEngine;

namespace Bomberman
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "ScriptableObjects/PlayerConfig", order = 2)]
    public class PlayerConfig : ScriptableObject
    {
        public float speed;
        public int maxBombs;
        public float bombLiveTime;
        public int bombRange;
    }
}
