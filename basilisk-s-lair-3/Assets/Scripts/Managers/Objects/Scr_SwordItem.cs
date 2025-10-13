using System.Collections;
using UnityEngine;

public class Scr_SwordItem : MonoBehaviour
{
    bool _playerIsColliding;
    GameObject _player;

    private void Start()
    {
        GameObject _playerObject = GameObject.FindGameObjectWithTag("Player");

        if (_playerObject != null)
        {
            if (_playerObject.GetComponent<Scr_PlayerAttack>().isActiveAndEnabled)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        if (_playerIsColliding && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Scr_PlayerStateManager _playerStateManager = _player.GetComponent<Scr_PlayerStateManager>();

            _player.GetComponent<Scr_PlayerAttack>().enabled = true;
            _playerStateManager.pVariables._canChangeDirection = false;
            _playerStateManager.pVariables._canWalk = false;
            _playerStateManager.pVariables.Manager.SwitchState(_playerStateManager.ItemState);

            Scr_MusicManager._instance.PlayMusic("Item", 0f);
            Scr_MusicManager._instance._musicSource.loop = false;

            transform.position = new Vector3(_playerStateManager._ItemPos.position.x,
                                             _playerStateManager._ItemPos.position.y,
                                             -1f);

            StartCoroutine("DestroySword");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerIsColliding = true;
            _player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerIsColliding = false;
        }
    }

    IEnumerator DestroySword()
    {
        yield return new WaitForSecondsRealtime(6f);
        
        Destroy(gameObject);
    }
}
