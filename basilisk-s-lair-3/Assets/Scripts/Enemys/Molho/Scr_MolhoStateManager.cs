using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scr_MolhoStateManager : MonoBehaviour
{
    private Scr_MolhoBaseState currentState;

    public MolhoData _molhoData;

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

        foreach(string DM in _molhoData.DeadMolhos)
        {
            if (DM == this.name) { Destroy(this.gameObject); }
        }
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
        rig.velocity = new Vector2(Mathf.Sign(playerPos.x) * _knockForceX, Mathf.Sign(playerPos.y) * _knockForceY);
    }

    void Die()
    {
        anim.Play("Anim_molho_death");
    }

    void Bye()
    {
        _molhoData.DeadMolhos.Add(this.gameObject.name);

        Destroy(this.gameObject);
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
