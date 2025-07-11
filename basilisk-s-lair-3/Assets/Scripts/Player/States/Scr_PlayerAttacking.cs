using UnityEngine;

public class Scr_PlayerAttacking : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player)
    {
        if (player.pVariables.rig.linearVelocity != Vector2.zero) { player.pVariables.anim.Play("Anim_player_walk_attack"); }
        else { player.pVariables.anim.Play("Anim_player_idle_attack"); }
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (player.attackEnd) { 
            player.SwitchState(player.IdleState);
            player.attackEnd = false;
        }
    }

    public override void CollisionEnter(Scr_PlayerStateManager player)
    {

    }
}
