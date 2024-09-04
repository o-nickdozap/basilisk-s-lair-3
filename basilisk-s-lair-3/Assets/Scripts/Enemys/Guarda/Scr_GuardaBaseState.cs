using UnityEngine;

public abstract class Scr_GuardaBaseState
{
    public abstract void EnterState(Scr_GuardaStateManager guarda);

    public abstract void UpdateState(Scr_GuardaStateManager guarda);

    public abstract void OnCollisionEnter(Scr_GuardaStateManager guarda);
}
