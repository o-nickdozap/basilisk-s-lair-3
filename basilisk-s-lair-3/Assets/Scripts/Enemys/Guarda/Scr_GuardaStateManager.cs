using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scr_GuardaStateManager : MonoBehaviour
{
    private Scr_GuardaBaseState _currentState;

    public Scr_GuardaPatrollingState _patrollingState = new Scr_GuardaPatrollingState();

    public Animator _anim;
    public Rigidbody2D _rig;

    public GameObject playerObject;
    public Vector2 playerPos;

    [SerializeField] float _knockForceX, _knockForceY;

    [SerializeField] float _footCheckDistance;
    [SerializeField] float _floorCheckDistance;
    [SerializeField] LayerMask _floorLayerMask;
    [SerializeField] Transform _footPos;

    public int _guardaDirection = 1;
    public float _speed = 0.8f;

    void Start()
    {
        _currentState = _patrollingState;

        playerObject = GameObject.FindGameObjectWithTag("Player");

        _currentState.EnterState(this);
    }

    void Update()
    {
        playerPos = gameObject.transform.position - playerObject.transform.position;

        transform.localScale = new Vector3(_guardaDirection, 1, 1);

        _currentState.UpdateState(this);
    }

    public void KnockBack()
    {
        _rig.velocity = new Vector2(Mathf.Sign(playerPos.x) * _knockForceX, Mathf.Sign(playerPos.y) * _knockForceY);
    }

    public bool IsOnFloor()
    {
        return Physics2D.Raycast(_footPos.position, Vector2.down, _footCheckDistance, _floorLayerMask);
    }

    public bool IsOnWall()
    {
        return Physics2D.Raycast(_footPos.position, new Vector2(_guardaDirection, 0), _floorCheckDistance, _floorLayerMask);
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Scr_PlayerStateManager _player = col.gameObject.GetComponent<Scr_PlayerStateManager>();

            _player._playerLife--;
            _player._enemyHit = GetComponent<Collider2D>();

            _player._knockbackTrigger = true;
            _player._canBeDamage = false;

            _player._knockbackTimeCounter = _player._knobackTime;
            _player.Knockback();

            _player.rig.velocity = new Vector2(0, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(_footPos.position, Vector2.down * _footCheckDistance);

        Gizmos.DrawRay(_footPos.position, new Vector2(_guardaDirection * _floorCheckDistance, 0));
    }
}
