using UnityEngine;

public abstract class Scr_MolhoBaseState
{
    public abstract void EnterState(Scr_MolhoStateManager molho);

    public abstract void UpdateState(Scr_MolhoStateManager molho);

    public abstract void OnCollisionEnter(Scr_MolhoStateManager molho);
}
