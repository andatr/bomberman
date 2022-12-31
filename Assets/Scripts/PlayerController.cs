using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidBody;
    private bool _good;
    private Vector2 _direction;
    private float _speed;
    private int _stateParamId;

    public Vector3 Position
    {
        set { _rigidBody.position = value; }
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
        _rigidBody = GetComponent<Rigidbody2D>();
        if (_rigidBody == null)
        {
            _good = false;
            Debug.LogError("Rigidbody2D component not found", this);
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
        PlaceBomb();
    }

    private void Move()
    {
        _rigidBody.MovePosition(_rigidBody.position + _speed * Time.fixedDeltaTime * _direction);
    }

    private void PlaceBomb()
    {
        if (Input.GetKeyDown("space"))
        {
            PlaceBomb();
        }
    }

    private void SetAnimation()
    {
        _direction = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
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
