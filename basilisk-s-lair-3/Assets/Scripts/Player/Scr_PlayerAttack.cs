using Unity.VisualScripting;
using UnityEngine;

public class Scr_PlayerAttack : MonoBehaviour
{
    public PlayerVariables pVariables;

    private float swordDamage = 1;
    private bool _isDamaging = false;

    private float _attackRadius;
    [SerializeField] float _swordAttack1Radius;
    [SerializeField] Vector2 walkAttackArea = new Vector2(.35f, .45f);
    [SerializeField] LayerMask enemyLayer;

    [SerializeField] Transform _attackPos;

    private int _swordComboCounter;
    private bool _swordAttackForceTrigger;

    private void Start()
    {
        pVariables._isAttacking = false;

        _swordComboCounter = 0;

        _attackRadius = _swordAttack1Radius;
    }

    private void Update()
    {
        AttackInput();
    }

    void AttackInput()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Attack"))
        {
            pVariables._isAttacking = true;

            if (pVariables.Manager.IsOnFloor)
            {
                pVariables._canWalk = false;
            }

            pVariables._attackInputCounter++;

            _attackRadius = _swordAttack1Radius;

                if (pVariables._attackInputCounter > 3)
                {
                    pVariables._attackInputCounter = 3;
                }
        }

        if (pVariables._isAttacking && _swordComboCounter >= 3)
        {
            _swordAttackForceTrigger = false;
            pVariables._isAttacking = false;
            pVariables._canWalk = true;

            pVariables.attackEnd = true;
            _isDamaging = false;

            pVariables._attackInputCounter = 0;
            _swordComboCounter = 0;
        }
    }

    void Damage()
    {
        _isDamaging = true;
    }

    void AttackDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPos.position, _attackRadius, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Molho")) { enemy.GetComponent<Scr_MolhoStateManager>().Damage(swordDamage); }
            _isDamaging = false;
        }
    }

    void AttackEnd()
    {
        pVariables._attackInputCounter--;

        if (pVariables._attackInputCounter == 0)
        {
            pVariables._isAttacking = false;
            pVariables.attackEnd = true;
            pVariables._canWalk = true;

            _swordAttackForceTrigger = false;
            _isDamaging = false;

            _swordComboCounter = 0;
        }
        else
        {
            _swordComboCounter++;
        }
    }

    void SwordAddForce()
    {
        if (pVariables.Manager.IsOnFloor)
        {
            _swordAttackForceTrigger = true;
        }
    }

    void SwordEndForce()
    {
        _swordAttackForceTrigger = false;
    }

    private void FixedUpdate()
    {
        if (_swordAttackForceTrigger)
        {
            if (_swordComboCounter == 2)
            {
                pVariables.rig.AddForce(new Vector2(pVariables._playerDirection * 75, 0), ForceMode2D.Force);
            }
            else
            {
                pVariables.rig.AddForce(new Vector2(pVariables._playerDirection * 33, 0), ForceMode2D.Force);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (_isDamaging)
        {
            Gizmos.DrawWireSphere(_attackPos.position, _attackRadius);
        }
    }
}
