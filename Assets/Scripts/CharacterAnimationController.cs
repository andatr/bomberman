using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    #region Unity Messages

    private void Awake()
    {
        SetAnimator();
        SetController();       
        SetAnimationParams();
    }

    private void OnDestroy()
    {
        if (movementController != null)
        {
            movementController.DirectionChanged -= OnMovementDirectionChanged;
        }
    }

    #endregion

    #region Private

    private void SetAnimator()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            enabled = false;
            Debug.LogError("Animator component not found", this);
        }
    }

    private void SetController()
    {
        movementController = GetComponent<IMovementController>();
        if (movementController == null)
        {
            enabled = false;
            Debug.LogError("Movement Controller component not set", this);
        }
        else
        {
            movementController.DirectionChanged += OnMovementDirectionChanged;
        }
    }

    private void SetAnimationParams()
    {
        deltaXId = Animator.StringToHash("deltaX");
        deltaYId = Animator.StringToHash("deltaY");
        isMovingId = Animator.StringToHash("isMoving");
    }

    private void OnMovementDirectionChanged(Vector2 direction)
    {
        bool movingX = direction.x > float.Epsilon || direction.x < -float.Epsilon;
        bool movingY = direction.y > float.Epsilon || direction.y < -float.Epsilon;
        if (movingX || movingY)
        {
            animator.SetBool(isMovingId, true);
            animator.SetFloat(deltaXId, movingX ? (direction.x > 0.0f ? 1.0f : -1.0f) : 0.0f);
            animator.SetFloat(deltaYId, movingY ? (direction.y > 0.0f ? 1.0f : -1.0f) : 0.0f);
        }
        else
        {
            animator.SetBool(isMovingId, false);
        }
    }

    private Animator animator;
    private IMovementController movementController;
    private int deltaXId;
    private int deltaYId;
    private int isMovingId;

    #endregion
}
