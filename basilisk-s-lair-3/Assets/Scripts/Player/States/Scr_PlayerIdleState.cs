using UnityEngine;

public class Scr_PlayerIdleState : Scr_PlayerBaseState
{
    float _secondIdleTimer = 3.5f;
    float _secondIdleTimerCounter;

    public override void EnterState(Scr_PlayerStateManager player)
    {
        player.pVariables.anim.Play("Anim_player_idle");

        _secondIdleTimerCounter = _secondIdleTimer;
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {
        if (player.pVariables._move != 0) { player.SwitchState(player.WalkState); }

        if (player.pVariables._isJumping) { player.SwitchState(player.JumpState); }

        if (player.pVariables._isAttacking) { player.SwitchState(player.AttackingState); }

        if (player.pVariables._isDashing) { player.SwitchState(player.DashState); }


        if (_secondIdleTimerCounter <= 0)
        {
            int _ribbitChance;

            _ribbitChance = Random.Range(0, 100);

            if (_ribbitChance >= 50)
            {
                player.SwitchState(player.Idle2State);
            }

            _secondIdleTimerCounter = _secondIdleTimer;
        }
        else
        {
            _secondIdleTimerCounter -= Time.deltaTime;
        }
    }

    public override void CollisionEnter(Scr_PlayerStateManager player)
    {

    }
}
