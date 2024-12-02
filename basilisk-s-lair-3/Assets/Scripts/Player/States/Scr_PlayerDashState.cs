using UnityEngine;

public class Scr_PlayerDashState : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player)
    {
        if (!player._knockbackStun && player._canDash) { player.anim.Play("Anim_player_dash"); }
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (player.dashTimeCounter <= 0 && player.IsOnFloor()) { player.SwitchState(player.IdleState); }
        if (player.dashTimeCounter <= 0 && !player.IsOnFloor()) { player.SwitchState(player.FallState); }
    }

    public override void OnCollisionEnter(Scr_PlayerStateManager player)
    {

    }
}
