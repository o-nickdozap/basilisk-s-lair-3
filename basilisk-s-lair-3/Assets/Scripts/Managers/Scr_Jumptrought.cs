using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Jumptrought : MonoBehaviour
{
    [SerializeField] Transform _playerTransform;
    private Rigidbody2D _playerRig;

    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        _playerTransform = GameObject.Find("Foot").GetComponent<Transform>();
        _playerRig = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        _boxCollider = this.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (_playerRig != null)
        {
            if (isPlayerAbove())
            {
                _boxCollider.enabled = true;
            }
            else
            {
                _boxCollider.enabled = false;
            }
        }
    }

    bool isPlayerAbove()
    {
        if (_playerTransform != null)
        {
            return ((this.transform.position.y - _playerTransform.position.y) < 0) ? true : false;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player" && _playerRig.velocity.y > 0)
        {
            _boxCollider.enabled = false;
        }
    }
}
