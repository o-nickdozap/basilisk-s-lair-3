using UnityEngine;

public class Scr_PlayerDoubleJumpState : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player)
    {
        if (!player._knockbackStun && player.jumpCounterMax == 2) { player.anim.Play("Anim_player_doublejump"); }
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (player.rig.linearVelocity.y <= 0) { player.SwitchState(player.FallState); }

        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Attack"))
        {
            if (player._canAttack) { player.SwitchState(player.AttackingState); }
        }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetAxis("Dash") > 0 && !player._isDashing) { player.SwitchState(player.DashState); }

        if (player.IsOnWall() && !player.IsOnFloor() && player.rig.linearVelocity.y < 0) { player.SwitchState(player.WallSlideState); }
    }

    public override void OnCollisionEnter(Scr_PlayerStateManager player)
    {

    }
}