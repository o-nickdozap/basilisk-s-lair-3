using UnityEngine;

public class Scr_PlayerJumpState : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player)
    {
        player.pVariables.anim.Play("Anim_player_jump");
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (player.IsOnFloor && player.pVariables.rig.linearVelocity.y <= 0) { player.SwitchState(player.IdleState); }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump")) { player.SwitchState(player.DoubleJumpState); }

        if (player.pVariables.rig.linearVelocityY < 0 && !player.pVariables._isJumping) { player.SwitchState(player.FallState); }

        if (player.pVariables._isAttacking) { player.SwitchState(player.AttackingState); }

        if (player.pVariables._isDashing) { player.SwitchState(player.DashState); }

        if (player.IsOnWall) { player.SwitchState(player.WallSlideState); }
    }

    public override void CollisionEnter(Scr_PlayerStateManager player){

    }
}
