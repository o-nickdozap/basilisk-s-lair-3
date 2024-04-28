using UnityEngine;

public class Scr_PlayerAttacking : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player)
    {
        if (player.rig.velocity != Vector2.zero) { player.anim.Play("Anim_player_walk_attack"); }
        else { player.anim.Play("Anim_player_idle_attack"); }
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (player.attackEnd) { 
            player.SwitchState(player.IdleState);
            player.attackEnd = false;
        }
    }

    public override void OnCollisionEnter(Scr_PlayerStateManager player)
    {

    }
}
