using UnityEngine;

namespace Bomberman
{
    [CreateAssetMenu(fileName = "GamefieldConfig", menuName = "ScriptableObjects/GamefieldConfig", order = 3)]
    public class GamefieldConfig : ScriptableObject
    {
        public int width;
        public int height;
        public float density;
    }
}