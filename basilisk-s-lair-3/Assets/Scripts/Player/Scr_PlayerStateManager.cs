using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Scr_PlayerStateManager : MonoBehaviour
{
    private Scr_PlayerBaseState currentState;
    public PlayerData _playerData;

    public Scr_PlayerIdleState IdleState = new Scr_PlayerIdleState();
    public Scr_PlayerWalkState WalkState = new Scr_PlayerWalkState();
    public Scr_PlayerJumpState JumpState = new Scr_PlayerJumpState();
    public Scr_PlayerFallState FallState = new Scr_PlayerFallState();
    public Scr_PlayerDoubleJumpState DoubleJumpState = new Scr_PlayerDoubleJumpState();
    public Scr_PlayerAttacking AttackingState = new Scr_PlayerAttacking();
    public Scr_PlayerDashState DashState = new Scr_PlayerDashState();
    public Scr_PlayerWallSlideState WallSlideState = new Scr_PlayerWallSlideState();

    public Vector2 _startScenePosition;

    public Animator anim;
    public Rigidbody2D rig;
    public SpriteRenderer spr;

    #region Move

    public float move;
    private float moveY;
    public float speed = 1.5f;

    public bool IsOnSlope;
    private RaycastHit2D slopeHit;
    [SerializeField] Vector2 slopeArea;

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
    private bool isJumping;

    #endregion

    #region Animation Manager

    public int playerDirection = 1;

    #endregion

    #region Attack

    public bool attackEnd;

    public int attackDirection;

    public float swordDamage = 1;

    bool isDamaging = false;

    bool isAttacking = false;

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

    public float dashCounter = 1;

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

    #region Life

    [SerializeField] int _playerMaxLife;
    [SerializeField] int _playerLife;

    [SerializeField] Vector2 _playerHitBox;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] Transform _hitBoxPivot;

    private Collider2D[] _wasHit;
    private bool _canBeDamage = true;

    [SerializeField] float _knobackTime;
    private float _knockbackTimeCounter;
    private bool _knockbackTrigger;
    public bool _knockbackStun;
    private Collider2D _enemyHit;

    [SerializeField] float _knockbackForceX;
    [SerializeField] float _knockbackForceY;

    [SerializeField] RectTransform _hpTransform;
    [SerializeField] Transform _hpMinTransform;

    #endregion

    void Start()
    {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoad;

        currentState = IdleState;

        attackArea = idleAttackArea;
        wallGravityMultiplier = wallGravity;

        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);

        _playerData.gameTime += Time.deltaTime;

        IsOnFloor = Physics2D.OverlapBox(footPos.position, footArea, 0, floorLayer);

        IsOnWall = Physics2D.OverlapBox(wallCheckPos.position, wallCheckDistance, 0, floorLayer)
        && !IsOnFloor && rig.velocity.y <= 0;

        if (!IsOnWall) { PlayerDirection(); }

        if (_knockbackTrigger) { Knockback(); }

        if (!_knockbackStun) {
            Dash();
            WallSlide();
            WallJump();
            Move();
            Attack();

            if (!IsOnWall) { Jump(); }
        } 
        else if (IsOnFloor) {
            _knockbackStun = false;
        }

        Slope();
        TDamage();
        Life();

        //Debug.Log(Physics2D.gravity);
    }

    void PlayerDirection()
    {
        if (move != 0) { playerDirection = (int)Mathf.Sign(move); }

        gameObject.transform.localScale = new Vector3(playerDirection, 1, 1);
    }

    void Move()
    {
        move = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        rig.velocity = new Vector2(move * speed, rig.velocity.y);
    }

    void Jump()
    {
        if (rig.velocity.y <= 0 && !IsOnWall) {
            if (!IsOnFloor) { Physics2D.gravity = new Vector2(0, fallGravity); }
            else { jumpCounter = jumpCounterMax; isJumping = false; }
        } else { Physics2D.gravity = new Vector2(0, gravity); }


        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump")) {
            if (IsOnFloor && jumpCounter > 0) {
                rig.velocity = new Vector2(rig.velocity.x, jumpForce);
                jumpTimecounter = jumpTime;
                jumpCounter -= 1;
                isJumping = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump")) {
            if (!IsOnFloor && jumpCounter > 0) {
                rig.velocity = new Vector2(rig.velocity.x, jumpForce);
                jumpTimecounter = jumpTime;
                jumpCounter -= 2;
                isJumping = true;
            }
        }

        if (Input.GetKey(KeyCode.Z) || Input.GetButton("Jump")) {
            if (jumpTimecounter > 0) {
                rig.velocity = new Vector2(rig.velocity.x, jumpForce);
                jumpTimecounter -= Time.deltaTime;
                isJumping = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.Z) || Input.GetButtonUp("Jump")) { jumpTimecounter = 0; isJumping = false; }
    }

    void Slope()
    {
        slopeHit = Physics2D.Raycast(transform.position, -Vector2.up, 100f, floorLayer);

        Collider2D[] hitfloor = Physics2D.OverlapBoxAll(footPos.position, slopeArea, 0);
        foreach (Collider2D col in hitfloor) {
            if (col.CompareTag("Slope")) { IsOnSlope = true; }
            else { IsOnSlope = false; }
        }

        if (IsOnSlope && !isJumping) {
            if (rig.velocity.y <= 0) { transform.position = new Vector2(transform.position.x, slopeHit.point.y + slopeHit.distance); }
            else { rig.velocity = new Vector2(rig.velocity.x, 0); }

            rig.gravityScale = 0;
        } if (!IsOnSlope) { rig.gravityScale = 1; }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Attack"))
        {
            if (!isAttacking)
            {
                isAttacking = true;
                attackDirection = playerDirection;

                if (rig.velocity == Vector2.zero) { attackArea = idleAttackArea; }

                else { attackArea = walkAttackArea; }

                isDamaging = true;
            }
        }

        if (isDamaging)
        {
            AttackDamage();
        }
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

    void TDamage()
    {
        _wasHit = Physics2D.OverlapBoxAll(_hitBoxPivot.position, _playerHitBox, 0, _enemyLayer);

        foreach (Collider2D hit in _wasHit) {
            if (hit.CompareTag("Molho") && _canBeDamage) {
                _playerLife--;

                _enemyHit = hit;
                _knockbackTrigger = true;
                _canBeDamage = false;
                _knockbackTimeCounter = _knobackTime;

                Knockback();
                hit.gameObject.GetComponent<Scr_MolhoStateManager>().KnockBack();

                rig.velocity = new Vector2(0, 0);
            }
        }
    }

    void Knockback()
    {
        float molhoPos = gameObject.transform.position.x - _enemyHit.transform.position.x;

        if (_knockbackTimeCounter > 0)
        {
            _knockbackTimeCounter -= Time.deltaTime;
            _knockbackStun = true;

            rig.velocity = new Vector2(Mathf.Sign(molhoPos) * _knockbackForceX, _knockbackForceY);
        } else { _knockbackTrigger = false; _canBeDamage = true; }
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
                dashTimeCounter = dashTime;
                isAttacking = false;
                isDashing = true;
                dashCounter--;
            }
        }

        if (dashTimeCounter <= 0 && Input.GetAxis("Dash") == 0)
        {
            dashTimeCounter = 0;
            isDashing = false;
        }

        if (dashTimeCounter > 0)
        {
            rig.AddForce(new Vector2(((dashForce * 10000) * playerDirection) * Time.deltaTime, 0), ForceMode2D.Force);
            rig.velocity = new Vector2(rig.velocity.x, 0);
            dashTimeCounter -= Time.deltaTime;
        }
    }
    
    void WallSlide()
    {
        if (moveY < 0) { wallGravity = wallGravityMultiplier * (-moveY * 4 + 1); }
        else { wallGravity = wallGravityMultiplier; }

        if (IsOnWall) {
            rig.velocity = new Vector2(0, 0);
            Physics2D.gravity = new Vector2(0, wallGravity);
            jumpCounter = jumpCounterMax;
            dashCounter = 1;
            spr.flipX = true;
        } else { spr.flipX = false; }
    }

    void WallJump()
    {
        if (IsOnWall) {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump")) {
                rig.AddForce(new Vector2(-playerDirection * walljumpForceX, walljumpForceY), ForceMode2D.Force);
            }
        }
    }

    void Life()
    {
        _hpTransform.pivot = new Vector2(0 , 0.5f);
        _hpTransform.position = _hpMinTransform.position;
        _hpTransform.sizeDelta = new Vector2(-1913 + (371 * _playerLife/_playerMaxLife), _hpTransform.sizeDelta.y);

        if (_playerLife <= 0) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
    }

    public void SwitchState(Scr_PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode){
        this.transform.position = _startScenePosition;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireCube(footPos.position, footArea);

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(_hitBoxPivot.position, _playerHitBox);

        Gizmos.DrawWireCube(attackPos.position, attackArea);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckDistance);

        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(footPos.position, slopeArea);
    }
}
