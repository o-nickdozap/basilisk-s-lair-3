using UnityEngine;

public class Scr_PlayerDashState : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player)
    {
        player.anim.Play("Anim_player_dash");
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (player.dashTimeCounter <= 0 && player.IsOnFloor) { player.SwitchState(player.IdleState); }
        if (player.dashTimeCounter <= 0 && !player.IsOnFloor) { player.SwitchState(player.IdleState); }
        //if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump")) { player.SwitchState(player.JumpState); }
    }

    public override void OnCollisionEnter(Scr_PlayerStateManager player)
    {

    }
}
