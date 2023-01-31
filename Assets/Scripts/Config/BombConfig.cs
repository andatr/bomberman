using UnityEngine;

namespace Bomberman
{
    [CreateAssetMenu(fileName = "BombConfig", menuName = "ScriptableObjects/BombConfig", order = 4)]
    public class BombConfig : ScriptableObject
    {
        public float explosionLiveTime;
        public float explosionSpreadTime;
        public float bombTriggerTime;
        public int bombTotalLimit;
    }
}
