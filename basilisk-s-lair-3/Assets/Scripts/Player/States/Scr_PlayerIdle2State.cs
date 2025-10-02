using Unity.VisualScripting;
using UnityEngine;

public class Scr_PlayerIdle2State : Scr_PlayerBaseState
{
    float _firstdIdleTimer = 2f;
    float _firstIdleTimerCounter;

    public override void EnterState(Scr_PlayerStateManager player)
    {
        player.pVariables.anim.Play("Anim_player_idle2");

        _firstIdleTimerCounter = _firstdIdleTimer;
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (player.pVariables._move != 0) { player.SwitchState(player.WalkState); }

        if (player.pVariables._isJumping) { player.SwitchState(player.JumpState); }

        if (player.pVariables._isAttacking) { player.SwitchState(player.AttackingState); }

        if (player.pVariables._isDashing) { player.SwitchState(player.DashState); }


        if (_firstIdleTimerCounter <= 0)
        {
            player.SwitchState(player.IdleState);

            _firstIdleTimerCounter = _firstdIdleTimer;
        }
        else
        {
            _firstIdleTimerCounter -= Time.deltaTime;
        }
    }

    public override void CollisionEnter(Scr_PlayerStateManager player)
    {

    }
}