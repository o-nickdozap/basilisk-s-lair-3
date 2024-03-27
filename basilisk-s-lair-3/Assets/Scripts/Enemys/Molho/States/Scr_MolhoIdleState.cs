using UnityEngine;

public class Scr_MolhoIdleState : Scr_MolhoBaseState
{
    public override void EnterState(Scr_MolhoStateManager molho)
    {
        molho.anim.Play("Anim_molho_idle");
    }

    public override void UpdateState(Scr_MolhoStateManager molho)
    {
        if (molho.isChasing) { molho.SwitchState(molho.ChaseState); }
    }

    public override void OnCollisionEnter(Scr_MolhoStateManager molho)
    {

    }
}
