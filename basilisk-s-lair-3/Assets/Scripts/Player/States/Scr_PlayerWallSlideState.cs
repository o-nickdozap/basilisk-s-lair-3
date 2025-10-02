
using UnityEngine;

public class Scr_PlayerWallSlideState : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player)
    {
        if (player._canWall) { player.pVariables.anim.Play("Anim_player_wallslide"); }
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (player.IsOnFloor) { player.SwitchState(player.IdleState); }

        if (!player.IsOnWall)
        {
            if (Input.GetKey(KeyCode.Z) || Input.GetButton("Jump")) { player.SwitchState(player.JumpState); }
            else { player.SwitchState(player.FallState); }
        }
    }

    public override void CollisionEnter(Scr_PlayerStateManager player)
    {

    }
}
