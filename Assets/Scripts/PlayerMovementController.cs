using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerMovementController : MonoBehaviour, IMovementController
{
    #region Unity Messages

    [SerializeField]
    private float skinWidth = 0.01f;

    [SerializeField]
    private int obstacleCount = 1;

    private void Awake()
    {
        obstacles = new RaycastHit2D[obstacleCount];
        SetTransform();
        SetCollisionFilter();
        SetExtents();
    }

    private void FixedUpdate()
    {
        Vector2 delta = speed * Time.fixedDeltaTime * movementDirection;
        float deltaX = CheckHorizontalCollision(transform.position, delta.x);
        float deltaY = CheckVerticalCollision  (transform.position, delta.y);
        transform.position += new Vector3(deltaX, deltaY, 0.0f);
    }

    private void OnMovement(InputValue value)
    {
        movementDirection = value.Get<Vector2>();
        movementDirection.Normalize();
        DirectionChanged?.Invoke(movementDirection);
    }

    #endregion

    #region Public

    public Vector3 Position
    {
        set { transform.position = value; }
    }

    public float Speed
    {
        set { speed = value; }
    }

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
        if (movingLeft)
        {
            originMin = new Vector2(position.x - extents.x, position.y - extents.y + skinWidth);
            originMax = new Vector2(position.x - extents.x, position.y + extents.y - skinWidth);
            rayDirection = new Vector2(-1.0f, 0.0f);
        }
        else
        {
            originMin = new Vector2(position.x + extents.x, position.y - extents.y + skinWidth);
            originMax = new Vector2(position.x + extents.x, position.y + extents.y - skinWidth);
            rayDirection = new Vector2(1.0f, 0.0f);
        }
        float distance1 = CastRay(originMin, rayDirection, delta);
        float distance2 = CastRay(originMax, rayDirection, delta);
        return movingLeft
            ? -Mathf.Min(distance1, distance2)
            :  Mathf.Min(distance1, distance2);
    }

    private float CheckVerticalCollision(Vector3 position, float delta)
    {       
        if (delta < float.Epsilon && delta > -float.Epsilon) return delta;
        bool movingDown = delta < 0.0f;
        Vector2 originMin = new();
        Vector2 originMax = new();
        Vector2 rayDirection = new();
        if (movingDown)
        {
            originMin = new Vector2(position.x - extents.x + skinWidth, position.y - extents.y);
            originMax = new Vector2(position.x + extents.x - skinWidth, position.y - extents.y);
            rayDirection = new Vector2(0.0f, -1.0f);
        }
        else
        {
            originMin = new Vector2(position.x - extents.x + skinWidth, position.y + extents.y);
            originMax = new Vector2(position.x + extents.x - skinWidth, position.y + extents.y);
            rayDirection = new Vector2(0.0f, 1.0f);
        }
        float distance1 = CastRay(originMin, rayDirection, delta);
        float distance2 = CastRay(originMax, rayDirection, delta);
        return movingDown
            ? -Mathf.Min(distance1, distance2)
            :  Mathf.Min(distance1, distance2);
    }

    private float CastRay(Vector2 origin, Vector2 direction, float delta)
    {
        delta = Mathf.Abs(delta);
        float distance = delta;
        int count = Physics2D.Raycast(origin, direction, collisionFilter, obstacles, delta);
        count = Math.Min(count, obstacles.Length);
        for (int i = 0; i < count; ++i)
        {
            distance = Mathf.Min(distance, obstacles[i].distance);
        }
        return distance;
    }

    private void SetCollisionFilter()
    {
        collisionFilter = new ContactFilter2D();
        collisionFilter.useLayerMask = true;
        collisionFilter.layerMask = LayerMask.GetMask("Obstacles");
    }

    private void SetExtents()
    {
        var collider = GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            enabled = false;
            Debug.LogError("BoxCollider2D component not found", this);
        }
        else
        {
            extents = collider.bounds.extents;
        }
    }

    private void SetTransform()
    {
        transform = GetComponent<Transform>();
        if (transform == null)
        {
            enabled = false;
            Debug.LogError("Transform component not found", this);
        }
    }

    private Vector2 extents;
    private new Transform transform;
    private ContactFilter2D collisionFilter;
    private RaycastHit2D[] obstacles;
    private Vector2 movementDirection;
    private float speed;

    #endregion
}
