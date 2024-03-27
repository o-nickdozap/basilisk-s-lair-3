using UnityEngine;

public class Scr_PlayerDoubleJumpState : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player)
    {
        player.anim.Play("Anim_player_doublejump");
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (player.rig.velocity.y < 0) { player.SwitchState(player.FallState); }
        if (Input.GetKey(KeyCode.X) || Input.GetButtonDown("Attack")) { player.SwitchState(player.AttackingState); }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetAxis("Dash") > 0 && !player.isDashing) { player.SwitchState(player.DashState); }

        if (player.IsOnWall && !player.IsOnFloor && player.rig.velocity.y < 0) { player.SwitchState(player.WallSlideState); }
    }

    public override void OnCollisionEnter(Scr_PlayerStateManager player)
    {

    }
}