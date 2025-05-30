using UnityEngine;

public class Scr_PlayerFallState : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player)
    {
        player.pVariables.anim.Play("Anim_player_fall");
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (player.IsOnFloor()) { player.SwitchState(player.IdleState); }

        if (player.pVariables._isJumping)
        {
            player.SwitchState(player.DoubleJumpState);
        }

        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Attack"))
        {
            if (player._canAttack) { player.SwitchState(player.AttackingState); }
        }

        if (player.pVariables._isDashing) { player.SwitchState(player.DashState); }

        if (player.IsOnWall() && !player.IsOnFloor() && player.pVariables.rig.linearVelocity.y < 0) { player.SwitchState(player.WallSlideState); }
    }

    public override void CollisionEnter(Scr_PlayerStateManager player)
    {

    }
}
