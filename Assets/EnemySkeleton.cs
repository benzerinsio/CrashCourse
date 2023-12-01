using UnityEngine;

public class EnemySkeleton : Entity
{

    bool isAttacking;//private by default

    [Header("Move Info")]
    [SerializeField] private float moveSpeed;

    [Header("Player Detection")]
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask whatIsPlayer;

    private RaycastHit2D isPlayerDetected;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (isPlayerDetected)
        {
            if (isPlayerDetected.distance > 2)
            {
                rb.velocity = new Vector2(moveSpeed * 1.5f * facingDirection, rb.velocity.y);
                isAttacking = false;
            }
            else
            {
                isAttacking = true;
            }
        } else
        {
            Movement();
        }

        if (!isGrounded || isWallDetected)
        {
            Flip();
        }
    }

    private void Movement()
    {
        if (!isAttacking)
        {
            rb.velocity = new Vector2(moveSpeed * facingDirection, rb.velocity.y);
        }
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right, playerCheckDistance * facingDirection, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + playerCheckDistance * facingDirection, transform.position.y));
    }
}
