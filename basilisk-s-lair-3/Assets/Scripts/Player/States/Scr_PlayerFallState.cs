using UnityEngine;

public class Scr_PlayerFallState : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player)
    {
        player.pVariables.anim.Play("Anim_player_fall");
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (player.IsOnFloor) { player.SwitchState(player.IdleState); }

        if (player.pVariables._isJumping) { player.SwitchState(player.DoubleJumpState); }

        if (player.pVariables.isAttacking) { player.SwitchState(player.AttackingState); }

        if (player.pVariables._isDashing) { player.SwitchState(player.DashState); }

        if (player.IsOnWall) { player.SwitchState(player.WallSlideState); }
    }

    public override void CollisionEnter(Scr_PlayerStateManager player)
    {

    }
}
