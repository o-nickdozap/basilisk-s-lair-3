using Unity.VisualScripting;
using UnityEngine;

public class Scr_PlayerDash : MonoBehaviour
{
    public PlayerVariables pVariables;

    [SerializeField] float dashForce;
    private float _dashTime = .15f;
    public float _dashTimeCounter;
    public float _dashCounter;

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
        if (pVariables.Manager.IsOnFloor && Input.GetAxisRaw("Dash") == 0)
        {
            _dashCounter = pVariables._dashCounterMax;
        }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetAxisRaw("Dash") > 0)
        {
            if (!pVariables._isDashing && _dashCounter > 0)
            {
                _dashTimeCounter = _dashTime;
                pVariables._isDashing = true;
                _dashCounter--;
            }
        }

        if (pVariables._isDashing)
        {
            _dashTimeCounter -= Time.deltaTime;
        }

        if (_dashTimeCounter <= 0)
        {
            pVariables._isDashing = false;
            _dashTimeCounter = 0;
        }
    }

    void Dash()
    {
        if (pVariables._isDashing)
        {
            Physics2D.gravity = new Vector2(0, 0);
            pVariables.rig.linearVelocity = new Vector2(dashForce * pVariables._playerDirection, 0);
        }
    }
}
