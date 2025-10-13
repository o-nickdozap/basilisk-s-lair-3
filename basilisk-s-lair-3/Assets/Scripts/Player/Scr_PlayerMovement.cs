using UnityEngine;

public class Scr_PlayerMovement : MonoBehaviour
{
    public PlayerVariables pVariables;

    private readonly float _playerSpeed = 1.5f;

    private void Start()
    {
        pVariables._playerDirection = 1;
    }

    void MoveInput()
    {
        pVariables._move = Input.GetAxisRaw("Horizontal");
    }

    void Movement()
    {
        pVariables.rig.linearVelocity = new Vector2(pVariables._move * _playerSpeed, pVariables.rig.linearVelocity.y);
    }

    void PlayerDirection()
    {
        pVariables._playerDirection = (pVariables._move != 0) ? (int)Mathf.Sign(pVariables._move) : pVariables._playerDirection;

        gameObject.transform.localScale = new Vector3(pVariables._playerDirection, 1, 1);
    }

    private void Update()
    {
        if (pVariables._canWalk)
        {
            MoveInput();
        }
        else
        {
            pVariables._move = 0;
        }

        if (pVariables._canChangeDirection)
        {
            PlayerDirection();
        }  
    }

    private void FixedUpdate()
    {
        if (!pVariables._isDashing)
        {
            Movement();
        }
    }
}
