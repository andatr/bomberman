using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bomberman
{
    public class PlayerController : MonoBehaviour
    {
        #region Public / Messages

        private void FixedUpdate()
        {
            Vector2 delta = _player.Speed * Time.fixedDeltaTime * _player.Direction;
            delta.x = CheckHorizontalCollision(_transform.position, delta.x);
            delta.y = CheckVerticalCollision  (_transform.position, delta.y);
            _player.Position += delta;
        }

        private void OnMovement(InputValue value)
        {
            Vector2 direction = value.Get<Vector2>();
            direction.Normalize();
            _player.Direction = direction;
        }

        private void OnAttack(InputValue value)
        {
            if (!_player.PlaceBomb(_grid.WorldToCell(_player.Position))) {
                Debug.LogWarning("Couldn't place a Bomb");
            }
        }

        public void Init(IPlayerModel_Controller player, IGrid grid)
        {
            CheckComponents();
            _player = player;
            _grid = grid;
        }

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
            SetTransform();
            SetCollisionFilter();
            SetExtents();
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

        #region Fields

        [SerializeField] private float _skinWidth = 0.01f;
        [SerializeField] private int _obstacleCount = 1;
        private IPlayerModel_Controller _player;
        private IGrid _grid;
        private Vector2 _extents;
        private Transform _transform;
        private ContactFilter2D _collisionFilter;
        private RaycastHit2D[] _obstacles;

        #endregion
    }
}