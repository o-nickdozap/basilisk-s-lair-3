using UnityEngine;

public class Scr_PlayerIdleState : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player){
        player.anim.Play("Anim_player_idle");
    }

    public override void UpdateState(Scr_PlayerStateManager player){
        if (player.move != 0) { player.SwitchState(player.WalkState); }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump"))
        {
            if (player.jumpCounter > 0) { player.SwitchState(player.JumpState); }
        }

        if (player.rig.velocity.y < 0 && !player.IsOnFloor) { player.SwitchState(player.FallState); }

        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Attack")) { player.SwitchState(player.AttackingState); }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetAxisRaw("Dash") > 0)
        {
            if (!player.isDashing && player.dashCounter > 0) { player.SwitchState(player.DashState); }
        }
    }

    public override void OnCollisionEnter(Scr_PlayerStateManager player){

    }
}
