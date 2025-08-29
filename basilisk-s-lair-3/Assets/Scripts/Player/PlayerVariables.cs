using UnityEngine;

public class PlayerVariables : MonoBehaviour
{
    public float gameTime;

    public Scr_PlayerStateManager Manager;

    public Rigidbody2D rig;
    public Animator anim;
    public SpriteRenderer spr;

    public float move;
    public int _playerDirection;

    public int jumpCounterMax;
    public bool _isJumping;

    public LayerMask floorLayer;

    public int _dashCounterMax;
    public bool _isDashing;

    public bool attackEnd;
    public bool isAttacking;
}
