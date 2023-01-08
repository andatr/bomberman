using System;
using UnityEngine;


public class Bomb : MonoBehaviour
{
    #region Fields

    private float _maxTime;
    private float _time;
    private float _range;


    #endregion

    #region Public / Messages

    public event Action<Bomb> Exploded;

    private void Awake()
    {
        CheckComponents();
    }

    private void FixedUpdate()
    {
        _time += Time.fixedDeltaTime;
        if (_time >= _maxTime) {
            Explode();
        }
    }

    public void Place(Vector3 position, float time, float range)
    {
        _maxTime = time;
        _range = range;
        _time = 0.0f;
    }

    #endregion

    #region Private

    private void Explode()
    {
        Exploded?.Invoke(this);
    }

    private void CheckComponents()
    {
    }

    #endregion
}
