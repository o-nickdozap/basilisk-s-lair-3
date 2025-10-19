using UnityEngine;

public class Scr_PlayerAttack : MonoBehaviour
{
    public PlayerVariables pVariables;

    private int _swordDamage = 1;
    private bool _isDamaging = false;

    private float _attackRadius;
    [SerializeField] float _swordAttack1Radius;
    //[SerializeField] Vector2 walkAttackArea = new Vector2(.35f, .45f);
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask interactableLayer;

    [SerializeField] Transform _attackPos;

    private int _swordComboCounter;
    private bool _swordAttackForceTrigger;

    private int _airAttackDirection;

    [SerializeField] AudioSource _swordHitSound;

    private void Start()
    {
        pVariables._isAttacking = false;

        _swordComboCounter = 0;

        _attackRadius = _swordAttack1Radius;
    }

    private void Update()
    {
        AttackInput();

        if (_isDamaging)
        {
            AttackDamage();
            AttackInteract();
        }
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
            else
            {
                _airAttackDirection = pVariables._playerDirection;
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
            AttackEnd();
        }

        if (pVariables._isAttacking && !pVariables.Manager.IsOnFloor)
        {
            pVariables._playerDirection = _airAttackDirection;
            pVariables._canChangeDirection = false;
        }

        if (pVariables.Manager.IsOnFloor && !pVariables._isAttacking)
        {
            pVariables._canChangeDirection = true;
        }
    }

    void Damage()
    {
        _isDamaging = true;

        _swordHitSound.Play();
    }

    void AttackDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPos.position, _attackRadius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Scr_TakingDamage>().Damage(_swordDamage);

            _isDamaging = false;
        }
    }

    void AttackInteract()
    {
        Collider2D[] hitInteractable = Physics2D.OverlapCircleAll(_attackPos.position, _attackRadius, interactableLayer);

        foreach (Collider2D interactable in hitInteractable)
        {
            if (interactable.CompareTag("Chest"))
            {
                interactable.GetComponent<Scr_Chest>().WasHit();
            }
            if (interactable.CompareTag("Candlestick"))
            {
                Destroy(interactable.gameObject);
            }

            _isDamaging = false;
        }
    }

    void AttackComboCounter()
    {
        pVariables._attackInputCounter--;

        if (pVariables._attackInputCounter == 0)
        {
            AttackEnd();
        }
        else
        {
            _swordComboCounter++;
        }
    }

    void AttackEnd()
    {
        pVariables._isAttacking = false;
        pVariables.attackEnd = true;
        pVariables._canWalk = true;
        pVariables._canChangeDirection = true;

        _swordAttackForceTrigger = false;
        _isDamaging = false;

        pVariables._attackInputCounter = 0;
        _swordComboCounter = 0;
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
