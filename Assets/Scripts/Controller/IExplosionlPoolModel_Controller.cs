using System;
using UnityEngine;

namespace Bomberman
{
    public interface IExplosionlPoolModel_Controller
    {
        event Action<IExplosionlModel_View> Placed;

        event Action<IExplosionlModel_View> Spread;

        event Action<IExplosionlModel_View> Removed;

        void Update(float deltaTime);
    }
}