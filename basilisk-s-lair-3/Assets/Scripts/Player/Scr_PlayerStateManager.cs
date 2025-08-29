using AutoLetterbox;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_PlayerStateManager : MonoBehaviour
{
    private Scr_PlayerBaseState currentState;

    public PlayerVariables pVariables;
    public Scr_PlayerJump _playerJump;
    public Scr_PlayerDash _playerDash;

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

    [Header("Triggers")]
    public bool _canAttack;
    public bool _canDash;
    public bool _canWall;

    [Header("Pos")]
    [SerializeField] Transform _footPos;
    [SerializeField] Transform _wallCheckPos;
    [SerializeField] Transform _lastFootPos;
    [SerializeField] Transform _hitBoxPivot;

    #region Move

    private float moveY;
    private Vector3 _lastFloorPos;
    [SerializeField] float _lastFootCheckDistance;

    public bool IsOnFloor;

    #endregion

    #region Wall

    public bool IsOnWall;
    private Vector2 wallCheckDistance = new Vector2(.12f, .2f);

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

        _hud = GameObject.Find("Hud");
        _hpTransform = GameObject.Find("HealthBar").GetComponent<RectTransform>();
        _hpMinTransform = GameObject.Find("Anchor").transform;

        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);

        pVariables.gameTime += Time.deltaTime;

        IsOnWall = Physics2D.OverlapBox(_wallCheckPos.position, wallCheckDistance, 0, pVariables.floorLayer)
        && !IsOnFloor && pVariables.rig.linearVelocity.y <= 0;

        IsOnFloor = Physics2D.OverlapBox(_footPos.position, _playerJump._footArea, 0, pVariables.floorLayer);

        LastFloor();

        if (_knockbackTrigger) { Knockback(); }

        else if (IsOnFloor)
        {
            _knockbackStun = false;
        }

        Life();

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

    public void Knockback()
    {
        float enemyPos = gameObject.transform.position.x - _enemyHit.transform.position.x;

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
        currentState = state;
        state.EnterState(this);
        //Debug.Log(currentState);
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        pVariables._isJumping = false;
        pVariables._isDashing = false;
    }

    private void OnBecameInvisible()
    {
        transform.position = _lastFloorPos;
    }

    void LastFloor()
    {
        if (Physics2D.Raycast(_footPos.position, Vector2.down, _lastFootCheckDistance, pVariables.floorLayer))
        {
            _lastFloorPos = this.transform.position;
        }
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