using UnityEngine;

public class Scr_PlayerDoubleJumpState : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player)
    {
        if (!player._knockbackStun && player.pVariables.jumpCounterMax == 2) { player.pVariables.anim.Play("Anim_player_doublejump"); }
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (player.pVariables.rig.linearVelocity.y <= 0) { player.SwitchState(player.FallState); }

        if (player.pVariables._isAttacking) { player.SwitchState(player.AttackingState); }

        if (player.pVariables._isDashing) { player.SwitchState(player.DashState); }

        if (player.IsOnWall) { player.SwitchState(player.WallSlideState); }
    }

    public override void CollisionEnter(Scr_PlayerStateManager player)
    {

    }
}