using UnityEngine;

public class Scr_PlayerJumpState : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player){
        player.anim.Play("Anim_player_jump");
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump") && player.jumpCounter > 0) { player.SwitchState(player.DoubleJumpState); }
        if (player.rig.velocity.y < 0) { player.SwitchState(player.FallState); }

        if (Input.GetKey(KeyCode.X) || Input.GetButtonDown("Attack")) { player.SwitchState(player.AttackingState); }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetAxisRaw("Dash") > 0 && !player.isDashing) { player.SwitchState(player.DashState); }

        if (player.IsOnWall && !player.IsOnFloor && player.rig.velocity.y < 0) { player.SwitchState(player.WallSlideState); }
        if (player.IsOnSlope && player.rig.velocity.y <= 0) { player.SwitchState(player.IdleState); }
    }

    public override void OnCollisionEnter(Scr_PlayerStateManager player){

    }
}
