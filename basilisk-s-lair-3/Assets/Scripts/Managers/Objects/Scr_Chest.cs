using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Scr_Chest : MonoBehaviour
{
    Animator _anim;

    bool _wasOpended = false;
    bool _playerIsColliding;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void WasHit()
    {
        _anim.Play("Anim_OpenedChest");
        _wasOpended = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerIsColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!_wasOpended) { _anim.Play("Anim_ClosedChest"); }

            _playerIsColliding = false;
        }
    }

    private void Update()
    {
        if (_playerIsColliding && !_wasOpended)
        {
            _anim.Play("Anim_HighlightChest");

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _anim.Play("Anim_OpenedChest");
                _wasOpended = true;
            }
        }
    }
}
