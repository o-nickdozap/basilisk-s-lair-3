using UnityEngine;

public class Scr_PlayerIdleState : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player)
    {
        player.pVariables.anim.Play("Anim_player_idle");
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (player.pVariables.move != 0) { player.SwitchState(player.WalkState); }

        if (player.pVariables._isJumping) { player.SwitchState(player.JumpState); }

        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Attack"))
        {
            if (player._canAttack) { player.SwitchState(player.AttackingState); }
        }

        if (player.pVariables._isDashing) { player.SwitchState(player.DashState); }
    }

    public override void CollisionEnter(Scr_PlayerStateManager player)
    {

    }
}
