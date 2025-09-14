using UnityEngine;

public class Scr_PlayerJump : MonoBehaviour
{
    public PlayerVariables pVariables;

    private float jumpForce = 1.5f;
    private float jumpTime = .3f;
    private float jumpTimecounter;

    private float gravity = -8;
    private float fallGravity = -13;
    public int _jumpCounter;

    public Vector2 _footArea;

    void JumpInput()
    {
        if (pVariables.rig.linearVelocity.y <= 0)
        {
            if (!pVariables.Manager.IsOnWall)
            {
                if (!pVariables.Manager.IsOnFloor)
                {
                    Physics2D.gravity = new Vector2(0, fallGravity);
                }
            }
        }
        else
        {
            Physics2D.gravity = new Vector2(0, gravity);
        }

        if (pVariables.Manager.IsOnFloor && !pVariables._isJumping)
        {
            pVariables._isJumping = false;
            _jumpCounter = pVariables.jumpCounterMax;
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Jump"))
        {
            if (_jumpCounter > 0)
            {
                pVariables._isJumping = true;

                jumpTimecounter = jumpTime;
                _jumpCounter--;
            }
        }

        if (jumpTimecounter > 0)
        {
            pVariables._isJumping = true;

            jumpTimecounter -= Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Z) || Input.GetButtonUp("Jump"))
        {
            jumpTimecounter = 0;
            pVariables._isJumping = false;
        }

        if (jumpTimecounter <= 0)
        {
            jumpTimecounter = 0;
            pVariables._isJumping = false;
        }
    }

    void Jump()
    {
        if (pVariables._isJumping)
        {
            pVariables.rig.linearVelocity = new Vector2(pVariables.rig.linearVelocity.x, jumpForce);
        }
    }

    private void Update()
    {
        JumpInput();
    }

    private void FixedUpdate()
    {
        Jump();
    }
}
