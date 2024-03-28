using UnityEngine;

public class Scr_PlayerStateManager : MonoBehaviour
{
    Scr_PlayerBaseState currentState;

    public Scr_PlayerIdleState IdleState = new Scr_PlayerIdleState();
    public Scr_PlayerWalkState WalkState = new Scr_PlayerWalkState();
    public Scr_PlayerJumpState JumpState = new Scr_PlayerJumpState();
    public Scr_PlayerFallState FallState = new Scr_PlayerFallState();
    public Scr_PlayerDoubleJumpState DoubleJumpState = new Scr_PlayerDoubleJumpState();
    public Scr_PlayerAttacking AttackingState = new Scr_PlayerAttacking();
    public Scr_PlayerDashState DashState = new Scr_PlayerDashState();
    public Scr_PlayerWallSlideState WallSlideState = new Scr_PlayerWallSlideState();

    public Animator anim;
    public Rigidbody2D rig;
    public SpriteRenderer spr;

    #region Move

        public float move;
        private float moveY;
        public float speed = 1.5f;

        private bool IsOnSlope;

    #endregion

    #region Jump

        public bool IsOnFloor;

        public Transform footPos;
        public Vector2 footArea;
        public LayerMask floorLayer;

        public float jumpForce;
        public float jumpTime;
        public float jumpTimecounter;

        public float gravity;
        public float fallGravity;
        public int jumpCounter;
        public int jumpCounterMax;

    #endregion

    #region Animation Manager

    public int playerDirection = 1;

    #endregion

    #region Attack

    public bool attackEnd;

    public int attackDirection;

    public float swordDamage = 1;

    bool isDamaging=false;

    bool isAttacking=false;

    public Transform attackPos;
    public Vector2 attackArea;
    public Vector2 walkAttackArea;
    public Vector2 idleAttackArea;
    public LayerMask enemyLayer;

    #endregion

    #region Dash

        public float dashForce;
        public float dashTime;
        public float dashTimeCounter;

        public float dashCounter=1;

        public bool isDashing;
        public bool _canDash;

    #endregion

    #region WallJump

        public bool IsOnWall;
        public float wallGravity;
        private float wallGravityMultiplier;
        public Vector2 wallCheckDistance;
        public Transform wallCheckPos;
        public bool resetYVelocityTrigger;
        public float walljumpForceX;
        public float walljumpForceY;
        public bool wallDirection;

    #endregion

    void Start()
    {
        currentState = IdleState;

        attackArea = idleAttackArea;
        wallGravityMultiplier = wallGravity;

        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);

        if (!IsOnWall) { PlayerDirection(); }
        Attack();
        Move();

        if (!IsOnWall) { Jump(); }

        Dash();
        WallSlide();
        WallJump();
        Slope();
    }

    void PlayerDirection()
    {
        if (move != 0) { playerDirection = (int)Mathf.Sign(move); }

        gameObject.transform.localScale = new Vector3(playerDirection, 1, 1);
    }

    void Move()
    {
        move = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        rig.velocity = new Vector2(move * speed, rig.velocity.y);
    }

    void Jump()
    {

        IsOnFloor = Physics2D.OverlapBox(footPos.position, footArea, 0, floorLayer);

        if (rig.velocity.y <= 0 && !IsOnWall) {
            if (!IsOnFloor) { Physics2D.gravity = new Vector2(0, fallGravity); }
            else { jumpCounter = jumpCounterMax; }
        } else { Physics2D.gravity = new Vector2(0, gravity); }


        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump")) {
            if (IsOnFloor) {
                rig.velocity = new Vector2(rig.velocity.x, jumpForce);
                jumpTimecounter = jumpTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump")) {
            if (!IsOnFloor && jumpCounter > 0) {
                rig.velocity = new Vector2(rig.velocity.x, jumpForce);
                jumpTimecounter = jumpTime;
                jumpCounter -= 1;
            }

        }

        if (Input.GetKey(KeyCode.Z) || Input.GetButton("Jump")) {
            if (jumpTimecounter > 0) {
                rig.velocity = new Vector2(rig.velocity.x, jumpForce);
                jumpTimecounter -= Time.deltaTime;
            }
        }
        if (Input.GetKeyUp(KeyCode.Z) || Input.GetButtonUp("Jump")) { jumpTimecounter = 0; }
    }

    void Slope()
    {
        Collider2D[] hitfloor = Physics2D.OverlapBoxAll(footPos.position, footArea , 0, floorLayer);
        foreach (Collider2D col in hitfloor) {
            if (col.CompareTag("Slope")) { IsOnSlope = true; }
            else { IsOnSlope = false; }
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Attack"))
        {
            if (!isAttacking)
            {
                isAttacking = true;
                attackDirection = playerDirection;

                if (move == 0) { attackArea = idleAttackArea; }

                else { attackArea = walkAttackArea; }

                isDamaging = true;

            }
        }

        if (isDamaging)
        {
            if (move != 0) { Invoke(nameof(AttackDamage), 0.2f); isDamaging = false; }
            else { AttackDamage(); isDamaging = false; }
        }

        //if (attackDirection != playerDirection) { AttackEnd(); }
    }

    void AttackDamage()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPos.position, attackArea, 0, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Molho")) { enemy.GetComponent<Scr_MolhoStateManager>().Damage(swordDamage); }
            isDamaging = false;
        }

    }

    void AttackEnd()
    {

        isAttacking = false;
        isDamaging = false;
        attackEnd = true;

    }

    void Dash()
    {
        if (IsOnFloor) { dashCounter = 1; }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetAxis("Dash") > 0 && !isDashing)
        {
            if (dashCounter > 0)
            {
                isAttacking = false;
                dashTimeCounter = dashTime;
                isDashing = true;
                dashCounter--;
            }

        }

        if (dashTimeCounter <= 0 && Input.GetAxis("Dash") == 0)
        {
            dashTimeCounter = 0;
            isDashing = false;
        }

        dashTimeCounter -= Time.deltaTime;

        if (dashTimeCounter > 0)
        {
            rig.AddForce(new Vector2(((dashForce * 10000) * playerDirection) * Time.deltaTime, 0), ForceMode2D.Force);
            rig.velocity = new Vector2(rig.velocity.x, 0);
        }
    }
    
    void WallSlide()
    {
        IsOnWall = Physics2D.OverlapBox(wallCheckPos.position, wallCheckDistance, 0, floorLayer)
        && !IsOnFloor && rig.velocity.y <= 0;

        if (moveY < 0) { wallGravity = wallGravityMultiplier * ((-moveY * 4) + 1); }
        else { wallGravity = wallGravityMultiplier; }

        if (IsOnWall) {
            rig.velocity = new Vector2(rig.velocity.x, 0);
            Physics2D.gravity = new Vector2(0, wallGravity);
            spr.flipX = true;
        } else { spr.flipX = false; }
    }

    void WallJump()
    {
        if (IsOnWall) {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump")) {
                jumpCounter = jumpCounterMax;
                dashCounter = 1;
                rig.AddForce(new Vector2(-playerDirection * walljumpForceX, walljumpForceY), ForceMode2D.Force);
            }
        }
    }

    public void SwitchState(Scr_PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(footPos.position, footArea);

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(attackPos.position, attackArea);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckDistance);
    }
}
