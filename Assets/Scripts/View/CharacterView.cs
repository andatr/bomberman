using UnityEngine;

namespace Bomberman
{
    public class CharacterView : MonoBehaviour
    {
        #region Public / Messages

        private void OnDestroy()
        {
            Unsubscribe();
        }

        public void Init(ICharacterModel_View character)
        {
            _transform = this.GetComponentEx<Transform>();
            _animator  = this.GetComponentEx<Animator>();
            SetAnimationParams();
            Unsubscribe();
            _character = character;
            if (_character != null) {
                _character.Moved += OnMove;
                _character.DirectionChanged += OnDirectionChanged;
                _character.Died += OnDeath;
            }
        }

        #endregion

        #region Private

        private void SetAnimationParams()
        {
            deltaXId   = Animator.StringToHash("deltaX"  );
            deltaYId   = Animator.StringToHash("deltaY"  );
            isMovingId = Animator.StringToHash("isMoving");
            isDeadId   = Animator.StringToHash("isDead"  );
        }

        private void OnDirectionChanged(Vector2 direction)
        {
            bool movingX = direction.x > float.Epsilon || direction.x < -float.Epsilon;
            bool movingY = direction.y > float.Epsilon || direction.y < -float.Epsilon;
            if (movingX || movingY) {
                _animator.SetBool(isMovingId, true);
                _animator.SetFloat(deltaXId, movingX ? (direction.x > 0.0f ? 1.0f : -1.0f) : 0.0f);
                _animator.SetFloat(deltaYId, movingY ? (direction.y > 0.0f ? 1.0f : -1.0f) : 0.0f);
            }
            else {
                _animator.SetBool(isMovingId, false);
            }
        }

        private void OnMove(Vector2 position)
        {
            _transform.position = new Vector3(position.x, position.y, 0.0f);
        }

        private void OnDeath()
        {
            _animator.SetBool(isDeadId, true);
        }

        private void Unsubscribe()
        {
            if (_character != null) {
                _character.Moved -= OnMove;
                _character.DirectionChanged -= OnDirectionChanged;
                _character.Died -= OnDeath;
            }
        }

        #endregion

        #region Fields

        private Transform _transform;
        private Animator _animator;
        private ICharacterModel_View _character;
        private int deltaXId;
        private int deltaYId;
        private int isMovingId;
        private int isDeadId;

        #endregion
    }
}