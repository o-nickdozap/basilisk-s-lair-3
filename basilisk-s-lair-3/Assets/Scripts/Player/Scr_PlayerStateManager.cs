using AutoLetterbox;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_PlayerStateManager : MonoBehaviour
{
    private Scr_PlayerBaseState _currentState;

    public PlayerVariables pVariables;
    public Scr_PlayerJump _playerJump;
    public Scr_PlayerDash _playerDash;
    public Scr_PlayerWall _playerWall;
    public Scr_PlayerMovement _playerMovement;

    public static Scr_PlayerStateManager _instance;

    public Scr_PlayerIdleState IdleState = new Scr_PlayerIdleState();
    public Scr_PlayerIdle2State Idle2State = new Scr_PlayerIdle2State();
    public Scr_PlayerWalkState WalkState = new Scr_PlayerWalkState();
    public Scr_PlayerJumpState JumpState = new Scr_PlayerJumpState();
    public Scr_PlayerFallState FallState = new Scr_PlayerFallState();
    public Scr_PlayerDoubleJumpState DoubleJumpState = new Scr_PlayerDoubleJumpState();
    public Scr_PlayerAttacking AttackingState = new Scr_PlayerAttacking();
    public Scr_PlayerDashState DashState = new Scr_PlayerDashState();
    public Scr_PlayerWallSlideState WallSlideState = new Scr_PlayerWallSlideState();
    public Scr_PlayerItem ItemState = new Scr_PlayerItem();

    public Vector2 _startScenePosition;

    [Header("Triggers")]
    public bool _canAttack;
    public bool _canDash;
    public bool _canWall;

    [Header("Pos")]
    [SerializeField] Transform _footPos;
    [SerializeField] Transform _wallCheckPos;
    [SerializeField] Transform _lastFootPos;
    [SerializeField] Transform _hitBoxPivot;
    [SerializeField] public Transform _ItemPos;

    #region Move

    private float moveY;
    private Vector3 _lastFloorPos;
    [SerializeField] float _lastFootCheckDistance;

    public bool IsOnFloor;

    #endregion

    #region Wall

    public bool IsOnWall;
    [SerializeField] Vector2 wallCheckDistance;

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
    private float enemyPos;

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

        _currentState = IdleState;

        _hud = GameObject.Find("Hud");
        _hpTransform = GameObject.Find("HealthBar").GetComponent<RectTransform>();
        _hpMinTransform = GameObject.Find("Anchor").transform;

        _currentState.EnterState(this);
    }

    void Update()
    {
        _currentState.UpdateState(this);

        pVariables.gameTime += Time.deltaTime;

        if (_playerWall.isActiveAndEnabled) {
            IsOnWall = Physics2D.OverlapBox(_wallCheckPos.position, wallCheckDistance, 0, pVariables.floorLayer)
            && !IsOnFloor && pVariables.rig.linearVelocity.y <= 0;
        }

        IsOnFloor = Physics2D.OverlapBox(_footPos.position, _playerJump._footArea, 0, pVariables.floorLayer);

        //LastFloor();

        if (_knockbackTrigger) { Knockback(); }

        else if (IsOnFloor)
        {
            _knockbackStun = false;
        }

        Life();

        #region Time Scale

            if (Input.GetKeyDown(KeyCode.N))
            {
                Time.timeScale = 0.33f;
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                Time.timeScale = 1f;
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                Time.timeScale = 2f;
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                Application.targetFrameRate = 10;
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                Application.targetFrameRate = 60;
            }

        #endregion
    }

    public void Knockback()
    {
        if (_enemyHit != null)
        {
            enemyPos = gameObject.transform.position.x - _enemyHit.transform.position.x;
        }

        if (_knockbackTimeCounter > 0)
        {
            _knockbackTimeCounter -= Time.deltaTime;
            _knockbackStun = true;

            pVariables.rig.linearVelocity = new Vector2(Mathf.Sign(enemyPos) * _knockbackForceX, _knockbackForceY);
        } else { 
            _knockbackTrigger = false;
            _canBeDamage = true;
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

    public void SwitchState(Scr_PlayerBaseState state)
    {
        _currentState = state;
        state.EnterState(this);

        //Debug.Log(_currentState);
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        pVariables._isJumping = false;
        pVariables._isDashing = false;
    }

    private void OnBecameInvisible()
    {
        //transform.position = _lastFloorPos;
    }

    void LastFloor()
    {
        if (Physics2D.Raycast(_footPos.position, Vector2.down, _lastFootCheckDistance, pVariables.floorLayer))
        {
            _lastFloorPos = this.transform.position;
        }
    }

    void ResetItemState()
    {
        pVariables._canWalk = true;
        pVariables._canChangeDirection = true;
        SwitchState(IdleState);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireCube(_footPos.position, _playerJump._footArea);

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(_hitBoxPivot.position, _playerHitBox);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(_wallCheckPos.position, wallCheckDistance);

        Gizmos.DrawRay(_lastFootPos.position, Vector2.down * _lastFootCheckDistance);
    }
}