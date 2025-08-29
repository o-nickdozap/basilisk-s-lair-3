using UnityEngine;

public class Scr_PlayerAttack : MonoBehaviour
{
    public PlayerVariables pVariables;

    
    private int attackDirection;
    private float swordDamage = 1;
    bool isDamaging = false;
    
    private Vector2 attackArea;
    private Vector2 idleAttackArea = new Vector2(.5f, .3f);
    private Vector2 walkAttackArea = new Vector2(.35f, .45f);
    [SerializeField] LayerMask enemyLayer;

    [SerializeField] Transform _attackPos;

    private void Start()
    {
        attackArea = idleAttackArea;
    }

    private void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Attack"))
        {
            if (!pVariables.isAttacking)
            {
                pVariables.isAttacking = true;
                attackDirection = pVariables._playerDirection;

                if (pVariables.rig.linearVelocity == Vector2.zero) { attackArea = idleAttackArea; }

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

    void AttackEnd()
    {
        pVariables.isAttacking = false;
        isDamaging = false;
        pVariables.attackEnd = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(_attackPos.position, attackArea);
    }
}
