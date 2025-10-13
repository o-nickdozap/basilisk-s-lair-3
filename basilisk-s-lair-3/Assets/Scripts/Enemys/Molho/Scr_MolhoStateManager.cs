using UnityEngine;

public class Scr_MolhoStateManager : MonoBehaviour
{
    private Scr_MolhoBaseState currentState;

    public EnemyData _molhoData;

    public Scr_TakingDamage _takingDamage;

    public Scr_MolhoIdleState IdleState = new Scr_MolhoIdleState();
    public Scr_MolhoChaseState ChaseState = new Scr_MolhoChaseState();
    public Scr_MolhoDamageState DamageState = new Scr_MolhoDamageState();

    public Animator anim;
    public Rigidbody2D rig;

    public Vector2 chaseArea;
    public LayerMask playerLayer;

    public bool isChasing;

    public GameObject _playerObject;
    public Vector2 _playerPos;

    void Start()
    {
        currentState = IdleState;

        currentState.EnterState(this);
    }

    void Update()
    {
        isChasing = Physics2D.OverlapBox(gameObject.transform.position, chaseArea, 0, playerLayer);

        currentState.UpdateState(this);

        if (_playerObject != null)
        {
            _playerPos = this.transform.position - _playerObject.transform.position;
        }
        else
        {
            _playerObject = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public void SwitchState(Scr_MolhoBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")) {
            Scr_PlayerStateManager _player = col.gameObject.GetComponent<Scr_PlayerStateManager>();

            _player._playerLife--;
            _player._enemyHit = GetComponent<Collider2D>();

            _player._knockbackTrigger = true;
            _player._canBeDamage = false;

            _player._knockbackTimeCounter = _player._knobackTime;
            _player.Knockback();

            _player.pVariables.rig.linearVelocity = new Vector2(0, 0);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(gameObject.transform.position, chaseArea);
    }
}
