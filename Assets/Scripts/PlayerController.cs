using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 20.0f;

    private Animator animator;
    private new Rigidbody2D rigidbody;
    private Vector3 direction;

    private void Start()
    {
        animator  = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        SetAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rigidbody.MovePosition(transform.position + speed * Time.deltaTime * direction);
    }

    private void SetAnimation()
    {
        direction = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"),
            0.0f
        );
        direction.Normalize();
        if (direction.x > float.Epsilon)
            animator.SetInteger("state", 1);
        else if (direction.x < -float.Epsilon)
            animator.SetInteger("state", 2);
        else if (direction.y > float.Epsilon)
            animator.SetInteger("state", 4);
        else if (direction.y < -float.Epsilon)
            animator.SetInteger("state", 3);
        else
            animator.SetInteger("state", 0);
    }
}
