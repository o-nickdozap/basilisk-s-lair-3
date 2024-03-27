using UnityEngine;

public class Scr_PlayerFallState : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player)
    {
        player.anim.Play("Anim_player_fall");
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (player.IsOnFloor) { player.SwitchState(player.IdleState); }
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump") && player.jumpCounter > 0) { player.SwitchState(player.DoubleJumpState); }
        if (Input.GetKey(KeyCode.X) || Input.GetButtonDown("Attack")) { player.SwitchState(player.AttackingState); }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetAxis("Dash") > 0 && !player.isDashing && player.dashCounter > 0) { player.SwitchState(player.DashState); }

        if (player.IsOnWall && !player.IsOnFloor && player.rig.velocity.y < 0) { player.SwitchState(player.WallSlideState); }
    }

    public override void OnCollisionEnter(Scr_PlayerStateManager player)
    {

    }
}
