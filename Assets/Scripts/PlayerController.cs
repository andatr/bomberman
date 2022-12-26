using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private BoxCollider2D _collider;
    private Transform _transform;
    private bool _good;
    private Vector3 _direction;
    private float _speed;
    private int _stateParamId;

    public Vector3 Position
    {
        set { _transform.position = value; }
    }

    public float Speed
    {
        set { _speed = value; }
    }

    private void Awake()
    {
        _good = true;
        _stateParamId = Animator.StringToHash("state");
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            _good = false;
            Debug.LogError("Animator component not found", this);
        }
        _collider = GetComponent<BoxCollider2D>();
        if (_collider == null)
        {
            _good = false;
            Debug.LogError("Collider component not found", this);
        }
        _transform = GetComponent<Transform>();
        if (_transform == null)
        {
            _good = false;
            Debug.LogError("Transform component not found", this);
        }
    }

    private void Update()
    {
        if (!_good) return;
        SetAnimation();
    }

    private void FixedUpdate()
    {
        if (!_good) return;
        Move();
    }

    private void Move()
    {
        Vector3 delta = _transform.position + _speed * Time.deltaTime * _direction;
        Vector2 delta2d = new Vector2(delta.x, delta.y);

        Vector2 c1 = new Vector2(_collider.bounds.min.x, _collider.bounds.min.y);
        Vector2 c2 = new Vector2(_collider.bounds.min.x, _collider.bounds.max.y);
        Vector2 c3 = new Vector2(_collider.bounds.max.x, _collider.bounds.min.y);
        Vector2 c4 = new Vector2(_collider.bounds.max.x, _collider.bounds.max.y);

        var r1 = new Ray2D(c1, delta2d);
        var r2 = new Ray2D(c1, delta2d);
        var r3 = new Ray2D(c1, delta2d);
        var r4 = new Ray2D(c1, delta2d);

        //new Ray2D()

        //collider.bounds.min;
        //collider.bounds.max;

        //collider.Raycast(new Ray2D())

        _transform.position += delta;
    }

    private void SetAnimation()
    {
        _direction = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"),
            0.0f
        );
        _direction.Normalize();
        if (_direction.x > float.Epsilon)
            _animator.SetInteger(_stateParamId, 1);
        else if (_direction.x < -float.Epsilon)
            _animator.SetInteger(_stateParamId, 2);
        else if (_direction.y > float.Epsilon)
            _animator.SetInteger(_stateParamId, 4);
        else if (_direction.y < -float.Epsilon)
            _animator.SetInteger(_stateParamId, 3);
        else
            _animator.SetInteger(_stateParamId, 0);
    }
}
