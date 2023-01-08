using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour, IMovementController
{
    #region Fields

    [SerializeField]
    private float _skinWidth = 0.01f;

    [SerializeField]
    private int _obstacleCount = 1;

    [SerializeField]
    private GameRules _rules;

    private Vector2 _extents;
    private Transform _transform;
    private ContactFilter2D _collisionFilter;
    private RaycastHit2D[] _obstacles;
    private Vector2 _movementDirection;
    private float _speed;

    #endregion

    #region Public / Messages

    public Vector3 Position
    {
        set { _transform.position = value; }
    }

    private void Awake()
    {
        CheckComponents();
    }

    private void FixedUpdate()
    {
        Vector2 delta = _speed * Time.fixedDeltaTime * _movementDirection;
        float deltaX = CheckHorizontalCollision(_transform.position, delta.x);
        float deltaY = CheckVerticalCollision(_transform.position, delta.y);
        _transform.position += new Vector3(deltaX, deltaY, 0.0f);
    }

    private void OnMovement(InputValue value)
    {
        _movementDirection = value.Get<Vector2>();
        _movementDirection.Normalize();
        DirectionChanged?.Invoke(_movementDirection);
    }

    #endregion

    #region IMovementController

    public event Action<Vector2> DirectionChanged;

    #endregion

    #region Private

    private float CheckHorizontalCollision(Vector3 position, float delta)
    {
        if (delta < float.Epsilon && delta > -float.Epsilon) return delta;
        bool movingLeft = delta < 0.0f;
        Vector2 originMin = new();
        Vector2 originMax = new();
        Vector2 rayDirection = new();
        if (movingLeft) {
            originMin = new Vector2(position.x - _extents.x, position.y - _extents.y + _skinWidth);
            originMax = new Vector2(position.x - _extents.x, position.y + _extents.y - _skinWidth);
            rayDirection = new Vector2(-1.0f, 0.0f);
        }
        else {
            originMin = new Vector2(position.x + _extents.x, position.y - _extents.y + _skinWidth);
            originMax = new Vector2(position.x + _extents.x, position.y + _extents.y - _skinWidth);
            rayDirection = new Vector2(1.0f, 0.0f);
        }
        float distance1 = CastRay(originMin, rayDirection, delta);
        float distance2 = CastRay(originMax, rayDirection, delta);
        return movingLeft
            ? -Mathf.Min(distance1, distance2)
            : Mathf.Min(distance1, distance2);
    }

    private float CheckVerticalCollision(Vector3 position, float delta)
    {
        if (delta < float.Epsilon && delta > -float.Epsilon) return delta;
        bool movingDown = delta < 0.0f;
        Vector2 originMin = new();
        Vector2 originMax = new();
        Vector2 rayDirection = new();
        if (movingDown) {
            originMin = new Vector2(position.x - _extents.x + _skinWidth, position.y - _extents.y);
            originMax = new Vector2(position.x + _extents.x - _skinWidth, position.y - _extents.y);
            rayDirection = new Vector2(0.0f, -1.0f);
        }
        else {
            originMin = new Vector2(position.x - _extents.x + _skinWidth, position.y + _extents.y);
            originMax = new Vector2(position.x + _extents.x - _skinWidth, position.y + _extents.y);
            rayDirection = new Vector2(0.0f, 1.0f);
        }
        float distance1 = CastRay(originMin, rayDirection, delta);
        float distance2 = CastRay(originMax, rayDirection, delta);
        return movingDown
            ? -Mathf.Min(distance1, distance2)
            : Mathf.Min(distance1, distance2);
    }

    private float CastRay(Vector2 origin, Vector2 direction, float delta)
    {
        delta = Mathf.Abs(delta);
        float distance = delta;
        int count = Physics2D.Raycast(origin, direction, _collisionFilter, _obstacles, delta);
        count = Math.Min(count, _obstacles.Length);
        for (int i = 0; i < count; ++i) {
            distance = Mathf.Min(distance, _obstacles[i].distance);
        }
        return distance;
    }

    private void CheckComponents()
    {
        CheckGameRules();
        SetTransform();
        SetCollisionFilter();
        SetExtents();
    }

    private void CheckGameRules()
    {
        if (_rules == null) {
            enabled = false;
            Debug.LogError("Game Rules component not set", this);
        }
        else {
            _speed = _rules.player.speed;
        }
    }

    private void SetCollisionFilter()
    {
        _collisionFilter = new ContactFilter2D();
        _collisionFilter.useLayerMask = true;
        _collisionFilter.layerMask = LayerMask.GetMask("Obstacles");
        _obstacles = new RaycastHit2D[_obstacleCount];
    }

    private void SetExtents()
    {
        var collider = GetComponent<BoxCollider2D>();
        if (collider == null) {
            enabled = false;
            Debug.LogError("BoxCollider2D component not found", this);
        }
        else {
            _extents = collider.bounds.extents;
        }
    }

    private void SetTransform()
    {
        _transform = GetComponent<Transform>();
        if (_transform == null) {
            enabled = false;
            Debug.LogError("Transform component not found", this);
        }
    }

    #endregion
}
