using UnityEngine;

public class Scr_PlayerDashState : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player)
    {
        if (!player._knockbackStun && player._canDash) { player.pVariables.anim.Play("Anim_player_dash"); }
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (!player.pVariables._isDashing) {
            if (player.IsOnFloor) player.SwitchState(player.IdleState); 
            if (!player.IsOnFloor) player.SwitchState(player.FallState);
        }
    }

    public override void CollisionEnter(Scr_PlayerStateManager player)
    {

    }
}
