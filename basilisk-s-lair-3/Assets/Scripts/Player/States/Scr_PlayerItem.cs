using UnityEngine;

public class Scr_PlayerItem : Scr_PlayerBaseState
{
    public override void EnterState(Scr_PlayerStateManager player)
    {
        player.pVariables.anim.Play("Anim_player_item");
    }

    public override void UpdateState(Scr_PlayerStateManager player)
    {

    }

    public override void CollisionEnter(Scr_PlayerStateManager player)
    {

    }
}
