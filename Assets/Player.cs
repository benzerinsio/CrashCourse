using UnityEngine;

public class Player : Entity
{
    [Header("Move Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Dash Info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private float dashTime;

    [SerializeReference] private float dashCooldown;
    private float dashCooldownTimer;

    [Header("Attack Info")]
    [SerializeField] private float comboTime = 1f;
    private bool isAttacking;
    private int comboCounter;
    private float comboTimeWindow;

    private float xInput;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        Movement();
        CheckInput();
        CollisionChecks();

        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        comboTimeWindow -= Time.deltaTime;

        FlipController();
        AnimatorController();
    }

    public void AttackOver()
    {
        isAttacking = false;
        comboCounter++;
        if (comboCounter > 2)
        {
            comboCounter = 0;
        }
    }
    private void Movement()
    {
        if (isAttacking)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else if (dashTime > 0)
        {
            rb.velocity = new Vector2(facingDirection * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if (isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void CheckInput() //only handle the input (don't do a lot of check here)
    {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashAbility();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            startAttackEvent();
        }
    }

    private void dashAbility()
    {
        if (dashCooldownTimer < 0 && !isAttacking)//the second part of input check (the first part is only input as mentioned) 
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }
    private void AnimatorController()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);
    }

    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        else if (rb.velocity.x < 0 && facingRight)
            Flip();
    }

    private void startAttackEvent()
    {
        if (!isGrounded)
        {
            return;
        }
        if (comboTimeWindow < 0)
        {
            comboCounter = 0;
        }
        isAttacking = true;
        comboTimeWindow = comboTime;
    }
}
