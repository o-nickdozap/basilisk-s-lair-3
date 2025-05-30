using UnityEngine;

public abstract class Scr_PlayerBaseState
{
    public abstract void EnterState(Scr_PlayerStateManager player);

    public abstract void UpdateState(Scr_PlayerStateManager player);

    public abstract void CollisionEnter(Scr_PlayerStateManager player);
}