using UnityEngine;

public class Scr_GuardaChaseState : Scr_GuardaBaseState
{
    public override void EnterState(Scr_GuardaStateManager guarda)
    {
        guarda._anim.Play("Walk_Guarda");
    }

    public override void UpdateState(Scr_GuardaStateManager guarda)
    {
        guarda._guardaDirection = (int)Mathf.Sign(guarda.playerObject.transform.position.x - guarda.transform.position.x);
        guarda._rig.linearVelocity = new Vector2(guarda._guardaDirection * guarda._speed * 1.5f, guarda._rig.linearVelocity.y);

        if (!guarda.IsOnChasing())
        {
            guarda.SwitchState(guarda._patrollingState);
        }
    }

    public override void OnCollisionEnter(Scr_GuardaStateManager guarda)
    {

    }
}
