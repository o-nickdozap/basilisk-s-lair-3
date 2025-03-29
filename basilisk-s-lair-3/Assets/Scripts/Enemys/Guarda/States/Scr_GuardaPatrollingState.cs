using System;
using UnityEngine;

public class Scr_GuardaPatrollingState : Scr_GuardaBaseState
{
    public override void EnterState(Scr_GuardaStateManager guarda)
    {
        guarda._anim.Play("Walk_Guarda");
    }

    public override void UpdateState(Scr_GuardaStateManager guarda)
    {
        guarda._rig.linearVelocity = new Vector2(guarda._guardaDirection * guarda._speed, guarda._rig.linearVelocity.y);

        if (!guarda.IsOnFloor() || guarda.IsOnWall() || guarda.IsHitEnemy())
        { 
            guarda._guardaDirection = -guarda._guardaDirection;
        }

        if (guarda.IsOnChasing())
        {
            guarda.SwitchState(guarda._chaseState);
        }
    }

    public override void OnCollisionEnter(Scr_GuardaStateManager guarda)
    {

    }
}
