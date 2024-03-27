using UnityEngine;      
public class Scr_MolhoChaseState : Scr_MolhoBaseState
{

    float molhoSpd = 0.6f;

    public override void EnterState(Scr_MolhoStateManager molho)
    {
        
    }

    public override void UpdateState(Scr_MolhoStateManager molho)
    {
        molho.transform.localScale = new Vector3(Mathf.Sign(-molho.playerPos.x), 1, 1);
        molho.transform.position = Vector2.MoveTowards(molho.transform.position, molho.playerObject.transform.position, molhoSpd * Time.deltaTime);

        if (!molho.isChasing) { molho.SwitchState(molho.IdleState); }
    }

    public override void OnCollisionEnter(Scr_MolhoStateManager molho)
    {

    }
}
