
using UnityEngine;

public class Scr_PlayerWallSlideState : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player)
    {
        player.anim.Play("Anim_player_wallslide");
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (player.IsOnFloor()) { player.SwitchState(player.IdleState); }
        if (!player.IsOnWall()) {
            if (Input.GetKey(KeyCode.Z) || Input.GetButton("Jump")) { player.SwitchState(player.JumpState); }
            else { player.SwitchState(player.FallState); }
        }
    }

    public override void OnCollisionEnter(Scr_PlayerStateManager player)
    {

    }
}
