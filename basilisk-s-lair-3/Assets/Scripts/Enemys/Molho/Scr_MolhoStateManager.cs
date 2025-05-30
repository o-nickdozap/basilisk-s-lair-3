using UnityEngine;

public class Scr_MolhoStateManager : MonoBehaviour
{
    private Scr_MolhoBaseState currentState;

    public EnemyData _molhoData;

    public Scr_MolhoIdleState IdleState = new Scr_MolhoIdleState();
    public Scr_MolhoChaseState ChaseState = new Scr_MolhoChaseState();
    public Scr_MolhoDamageState DamageState = new Scr_MolhoDamageState();

    public Animator anim;
    public Rigidbody2D rig;

    public Vector2 chaseArea;
    public LayerMask playerLayer;

    public GameObject playerObject;
    public Vector2 playerPos;

    public float molhoLife;
    [SerializeField] float _knockForceX, _knockForceY;

    public bool isChasing;

    void Start()
    {
        playerObject = GameObject.Find("Player");

        currentState = IdleState;

        currentState.EnterState(this);
    }

    void Update()
    {
        playerPos = gameObject.transform.position - playerObject.transform.position;

        isChasing = Physics2D.OverlapBox(gameObject.transform.position, chaseArea, 0, playerLayer);

        currentState.UpdateState(this);

        if (molhoLife <= 0) { Die(); }
    }

    public void Damage(float Damage)
    {
        molhoLife -= Damage;
        KnockBack();
    }

    public void KnockBack()
    {
        rig.linearVelocity = new Vector2(Mathf.Sign(playerPos.x) * _knockForceX, Mathf.Sign(playerPos.y) * _knockForceY);
    }

    void Die()
    {
        anim.Play("Anim_molho_death");
    }

    void Bye()
    {
        Destroy(this.gameObject);
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

            KnockBack();

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
