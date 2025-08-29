using UnityEngine;

public class Scr_PlayerMovement : MonoBehaviour
{
    public PlayerVariables pVariables;

    private readonly float speed = 1.5f;

    private void Start()
    {
        pVariables._playerDirection = 1;
    }

    private void Update()
    {
        MoveInput();
        PlayerDirection();
    }

    private void FixedUpdate()
    {
        if (!pVariables._isDashing) { Movement(); }
    }

    void MoveInput()
    {
        pVariables.move = Input.GetAxisRaw("Horizontal");
    }

    void Movement()
    {
        pVariables.rig.linearVelocity = new Vector2(pVariables.move * speed, pVariables.rig.linearVelocity.y);
    }

    void PlayerDirection()
    {
        pVariables._playerDirection = (pVariables.move != 0) ? (int)Mathf.Sign(pVariables.move) : pVariables._playerDirection;

        gameObject.transform.localScale = new Vector3(pVariables._playerDirection, 1, 1);
    }
}
