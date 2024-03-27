using System;
using UnityEngine;

public class Scr_MolhoDamageState : Scr_MolhoBaseState
{
    public override void EnterState(Scr_MolhoStateManager molho)
    {
        molho.anim.Play("Anim_player_damage");
    }

    public override void UpdateState(Scr_MolhoStateManager molho)
    {
        
    }

    public override void OnCollisionEnter(Scr_MolhoStateManager molho)
    {

    }
}
