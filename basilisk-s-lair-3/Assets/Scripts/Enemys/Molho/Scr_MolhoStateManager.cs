using UnityEngine;

public class Scr_MolhoStateManager : MonoBehaviour
{
    Scr_MolhoBaseState currentState;

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
        rig.velocity = new Vector2(0, 0);
        rig.AddForce(new Vector2(Mathf.Sign(playerPos.x) * _knockForceX, playerPos.y * _knockForceY) , ForceMode2D.Impulse);
    }

    void Die()
    {
        anim.Play("Anim_molho_death");
    }

    void Bye()
    {
        Destroy(gameObject);
    }

    public void SwitchState(Scr_MolhoBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(gameObject.transform.position, chaseArea);

    }
}
