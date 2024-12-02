using UnityEngine;

public class Scr_PlayerWalkState : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player){
        player.anim.Play("Anim_player_walk");
    }

    public override void UpdateState(Scr_PlayerStateManager player) {
        if (player.move == 0) { player.SwitchState(player.IdleState); }
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump")) { player.SwitchState(player.JumpState); }

        if (player.rig.velocity.y < 0 && !player.IsOnFloor()) { player.SwitchState(player.FallState); }

        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Attack"))
        {
            if (player._canAttack) { player.SwitchState(player.AttackingState); }
        }

        if (Input.GetKey(KeyCode.C) || Input.GetAxis("Dash") > 0 && !player._isDashing && player.dashCounter > 0) { player.SwitchState(player.DashState); }
    }

    public override void OnCollisionEnter(Scr_PlayerStateManager player){

    }
}
