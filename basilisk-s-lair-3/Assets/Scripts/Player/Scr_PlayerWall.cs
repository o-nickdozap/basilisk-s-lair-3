using UnityEngine;

public class Scr_PlayerWall : MonoBehaviour
{
    public PlayerVariables pVariables;

    private float moveY;

    private float wallGravity = -0.2f;
    private float wallGravityMultiplier;
    private float walljumpForceX = 256f;
    private float walljumpForceY = 128f;

    private void Start()
    {
        wallGravityMultiplier = wallGravity;
    }

    void WallSlide()
    {
        if (moveY < 0) { wallGravity = wallGravityMultiplier * (-moveY * 3f); }
        else { wallGravity = wallGravityMultiplier; }

        if (pVariables.Manager.IsOnWall)
        {
            Physics2D.gravity = new Vector2(0, 0);
            pVariables.spr.flipX = true;

            pVariables.rig.linearVelocity = new Vector2(0, wallGravity);
        }
        else { pVariables.spr.flipX = false; }
    }

    void WallJump()
    {
        if (pVariables.Manager.IsOnWall)
        {
            pVariables.Manager._playerJump._jumpCounter = pVariables.jumpCounterMax;
            pVariables.Manager._playerDash._dashCounter = pVariables._dashCounterMax;

            if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump"))
            {
                pVariables.rig.AddForce(new Vector2(-pVariables._playerDirection * walljumpForceX, walljumpForceY), ForceMode2D.Force);
                pVariables._playerDirection = -pVariables._playerDirection;
            }
        }
    }

    private void Update()
    {
        moveY = Input.GetAxisRaw("Vertical");

        WallSlide();
        WallJump();
    }
}
