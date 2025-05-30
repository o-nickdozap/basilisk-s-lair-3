using UnityEngine;

public class Scr_PlayerDash : MonoBehaviour
{
    public PlayerVariables pVariables;

    [SerializeField] float dashForce;
    private float dashTime = .15f;
    public float dashTimeCounter;
    public float dashCounter;

    private void Update()
    {
        DashInput();
    }

    private void FixedUpdate()
    {
        Dash();
    }

    void DashInput()
    {
        if (pVariables.Manager.IsOnFloor())
        {
            dashCounter = pVariables._dashCounterMax;
        }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetAxisRaw("Dash") > 0)
        {
            if (dashCounter > 0 && !pVariables._isDashing)
            {
                dashTimeCounter = dashTime;

                pVariables._isDashing = true;
                dashCounter--;
            }
        }

        if (pVariables._isDashing)
        {
            dashTimeCounter -= Time.deltaTime;
        }

        if (dashTimeCounter <= 0 && Input.GetAxisRaw("Dash") == 0)
        {
            pVariables._isDashing = false;
            dashTimeCounter = 0;
        }
    }

    void Dash()
    {
        if (pVariables._isDashing)
        {
            Physics2D.gravity = new Vector2(0, 0);
            pVariables.rig.linearVelocity = new Vector2((dashForce * pVariables._playerDirection), 0);
        }
    }
}
