using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_PlayerStateManager : MonoBehaviour
{
    private Scr_PlayerBaseState currentState;
    public PlayerData _playerData;

    public static Scr_PlayerStateManager _instance;

    public Scr_PlayerIdleState IdleState = new Scr_PlayerIdleState();
    public Scr_PlayerWalkState WalkState = new Scr_PlayerWalkState();
    public Scr_PlayerJumpState JumpState = new Scr_PlayerJumpState();
    public Scr_PlayerFallState FallState = new Scr_PlayerFallState();
    public Scr_PlayerDoubleJumpState DoubleJumpState = new Scr_PlayerDoubleJumpState();
    public Scr_PlayerAttacking AttackingState = new Scr_PlayerAttacking();
    public Scr_PlayerDashState DashState = new Scr_PlayerDashState();
    public Scr_PlayerWallSlideState WallSlideState = new Scr_PlayerWallSlideState();

    public Vector2 _startScenePosition;

    [Header("Components")]
    public Animator anim;
    public Rigidbody2D rig;
    public SpriteRenderer spr;

    [Header("Triggers")]
    public bool _canAttack;
    public bool _canDash;
    public bool _canWall;

    [Header("Counters Max")]
    public int jumpCounterMax;
    public float dashCounterMax;

    [Header("Pos")]
    [SerializeField] Transform _footPos;
    [SerializeField] Transform _attackPos;
    [SerializeField] Transform _wallCheckPos;
    [SerializeField] Transform _lastFootPos;
    [SerializeField] Transform _hitBoxPivot;

    #region Move

    public float move;
    private float moveY;
    public float speed = 1.5f; 
    private Vector3 _lastFloorPos;
    [SerializeField] float _lastFootCheckDistance;

    #endregion

    #region Jump

    public bool _isJumping;

    public Vector2 footArea;
    public LayerMask _floorLayer;

    public float jumpForce;
    public float jumpTime;
    public float jumpTimecounter;

    public float gravity;
    public float fallGravity;
    public int jumpCounter;

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

    
    public Vector2 attackArea;
    public Vector2 walkAttackArea;
    public Vector2 idleAttackArea;
    public LayerMask enemyLayer;

    #endregion

    #region Dash

    public float dashForce;
    public float dashTime;
    public float dashTimeCounter;

    public float dashCounter;

    public bool _isDashing;

    #endregion

    #region WallJump

    public float wallGravity;
    private float wallGravityMultiplier;
    public Vector2 wallCheckDistance;
    public bool resetYVelocityTrigger;
    public float walljumpForceX;
    public float walljumpForceY;
    public bool wallDirection;

    #endregion

    #region Life

    [SerializeField] int _playerMaxLife;
    [SerializeField] public int _playerLife;
    public GameObject _hud;

    [SerializeField] Vector2 _playerHitBox;
    [SerializeField] LayerMask _enemyLayer;

    public bool _canBeDamage = true;

    [SerializeField] public float _knobackTime;
    public float _knockbackTimeCounter;
    public bool _knockbackTrigger;
    public bool _knockbackStun;
    public Collider2D _enemyHit;

    [SerializeField] float _knockbackForceX;
    [SerializeField] float _knockbackForceY;

    [SerializeField] RectTransform _hpTransform;
    [SerializeField] Transform _hpMinTransform;

    #endregion

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;

        currentState = IdleState;

        attackArea = idleAttackArea;
        wallGravityMultiplier = wallGravity;

        _hud = GameObject.Find("Hud");
        _hpTransform = GameObject.Find("HealthBar").GetComponent<RectTransform>();
        _hpMinTransform = GameObject.Find("Anchor").transform;

        currentState.EnterState(this);
    }

    private void FixedUpdate()
    {
        if (!_knockbackStun)
        {
            Move();

            if (!IsOnWall())
            {
                Jump();

                if (_canDash) { Dash(); }
            }
        }
    }

    void Update()
    {
        currentState.UpdateState(this);

        _playerData.gameTime += Time.deltaTime;

        move = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        LastFloor();

        PlayerDirection();

        DashInput();

        if (_knockbackTrigger) { Knockback(); }

        if (!_knockbackStun)
        {
            if (_canWall)
            {
                WallSlide();
                WallJump();
            }
            
            if (_canAttack) { Attack(); }

            if (!IsOnWall()) { JumpInput(); }
        }

        else if (IsOnFloor()) {
            _knockbackStun = false;
        }

        Life();

        //Debug.Log(_isJumping + " " + jumpTimecounter);

        if (Input.GetKeyDown(KeyCode.N))
        {
            Time.timeScale = 0.33f;
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Application.targetFrameRate = 10;
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            Application.targetFrameRate = 60;
        }
    }

    void PlayerDirection()
    {
        playerDirection = (move != 0) ? (int)Mathf.Sign(move) : playerDirection;

        gameObject.transform.localScale = new Vector3(playerDirection, 1, 1);
    }

    void Move()
    {
        rig.velocity = new Vector2(move * speed, rig.velocity.y);
    }

    void JumpInput()
    {
        if (rig.velocity.y <= 0 && !IsOnWall())
        {
            if (!IsOnFloor())
            {
                Physics2D.gravity = new Vector2(0, fallGravity);
            }
            else {
                jumpCounter = jumpCounterMax;
                _isJumping = false;
            }
        }
        else
        {
            Physics2D.gravity = new Vector2(0, gravity);
        }


        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump"))
        {
            if (IsOnFloor() && jumpCounter > 0)
            {
                _isJumping = true;

                jumpTimecounter = jumpTime;
                jumpCounter -= 1;
            }
        }

        if (jumpCounterMax >= 2)
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump"))
            {
                if (!IsOnFloor() && jumpCounter > 0)
                {
                    _isJumping = true;

                    jumpTimecounter = jumpTime;
                    jumpCounter -= 2;
                }
            }
        }

        if (Input.GetKey(KeyCode.Z) || Input.GetButton("Jump"))
        {
            if (jumpTimecounter > 0)
            {
                _isJumping = true;

                jumpTimecounter -= Time.deltaTime;
            }
        }

        if (Input.GetKeyUp(KeyCode.Z) || Input.GetButtonUp("Jump"))
        { 
            jumpTimecounter = 0;
            _isJumping = false;
        }

        if (jumpTimecounter <= 0)
        {
            jumpTimecounter = 0;
            _isJumping = false;
        }
    }

    void Jump()
    {
        if (_isJumping)
        {
            rig.velocity = new Vector2(rig.velocity.x, jumpForce);
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
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(_attackPos.position, attackArea, 0, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Molho")) { enemy.GetComponent<Scr_MolhoStateManager>().Damage(swordDamage); }
            isDamaging = false;
        }
    }

    public void Knockback()
    {
        float enemyPos = gameObject.transform.position.x - _enemyHit.transform.position.x;

        if (_knockbackTimeCounter > 0)
        {
            _knockbackTimeCounter -= Time.deltaTime;
            _knockbackStun = true;

            rig.velocity = new Vector2(Mathf.Sign(enemyPos) * _knockbackForceX, _knockbackForceY);
        } else { 
            _knockbackTrigger = false;
            _canBeDamage = true;
        }
    }

    void AttackEnd()
    {
        isAttacking = false;
        isDamaging = false;
        attackEnd = true;
    }

    void DashInput()
    {
        if (IsOnFloor())
        {
            dashCounter = dashCounterMax;
        }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetAxisRaw("Dash") > 0)
        {
            if (dashCounter > 0 && !_isDashing)
            {
                dashTimeCounter = dashTime;
                isAttacking = false;

                _isDashing = true;
                dashCounter--;
            }
        }

        if (_isDashing)
        {
            dashTimeCounter -= Time.deltaTime;
        }

        if (dashTimeCounter <= 0 && Input.GetAxisRaw("Dash") == 0)
        {
            _isDashing = false;
            dashTimeCounter = 0;
        }
    }
    
    void Dash()
    {
        if (dashTimeCounter > 0)
        {
            Physics2D.gravity = new Vector2(0, 0);
            rig.velocity = new Vector2((dashForce * playerDirection), 0);
        }
    }

    void WallSlide()
    {
        if (moveY < 0) { wallGravity = wallGravityMultiplier * (-moveY * 4 + 1); }
        else { wallGravity = wallGravityMultiplier; }

        if (IsOnWall()) {
            Physics2D.gravity = new Vector2(0, wallGravity);
            rig.velocity = new Vector2(0, 0);
            spr.flipX = true;

            dashCounter = dashCounterMax;
        } else { spr.flipX = false; }
    }

    void WallJump()
    {
        if (IsOnWall())
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump"))
            {
                rig.AddForce(new Vector2(-playerDirection * walljumpForceX, walljumpForceY), ForceMode2D.Force);

                jumpCounter = jumpCounterMax - 1;
                dashCounter = dashCounterMax;
            }
        }
    }

    void Life()
    {
        _hpTransform.pivot = new Vector2(0 , 0.5f);
        _hpTransform.position = _hpMinTransform.position;
        _hpTransform.sizeDelta = new Vector2(-1920 + (370 * _playerLife/_playerMaxLife), _hpTransform.sizeDelta.y);

        if (_playerLife <= 0) {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public bool IsOnFloor()
    {
        return Physics2D.OverlapBox(_footPos.position, footArea, 0, _floorLayer);
    }

    public bool IsOnWall()
    {
        return Physics2D.OverlapBox(_wallCheckPos.position, wallCheckDistance, 0, _floorLayer)
        && !IsOnFloor() && rig.velocity.y <= 0;
    }

    public void SwitchState(Scr_PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode){
        this.transform.position = _startScenePosition;

        rig.velocity = new Vector2(move * speed, 0);
        _isJumping = false;
    }

    private void OnBecameInvisible()
    {
        transform.position = _lastFloorPos;
    }

    void LastFloor()
    {
        if (Physics2D.Raycast(_footPos.position, Vector2.down, _lastFootCheckDistance, _floorLayer))
        {
            _lastFloorPos = this.transform.position;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireCube(_footPos.position, footArea);

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(_hitBoxPivot.position, _playerHitBox);

        Gizmos.DrawWireCube(_attackPos.position, attackArea);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(_wallCheckPos.position, wallCheckDistance);

        Gizmos.DrawRay(_lastFootPos.position, Vector2.down * _lastFootCheckDistance);
    }
}