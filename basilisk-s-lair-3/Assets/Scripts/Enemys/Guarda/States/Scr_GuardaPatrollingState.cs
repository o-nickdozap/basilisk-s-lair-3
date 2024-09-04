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
        guarda._rig.velocity = new Vector2(guarda._guardaDirection * guarda._speed, guarda._rig.velocity.y);

        if (!guarda.IsOnFloor() || guarda.IsOnWall()) { guarda._guardaDirection = -guarda._guardaDirection; }
    }

    public override void OnCollisionEnter(Scr_GuardaStateManager guarda)
    {

    }
}
