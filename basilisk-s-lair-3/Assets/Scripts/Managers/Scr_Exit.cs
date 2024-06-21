using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_Exit : MonoBehaviour
{
    [SerializeField] Vector2 _starScenePosition;
    [SerializeField] bool _isReturning;
    private int _roomIndex;

    void OnCollisionEnter2D(Collision2D _col)
    {
        if (_col.collider.CompareTag("Player")) {
            if (_isReturning) { _col.gameObject.GetComponent<Scr_PlayerStateManager>().playerDirection *= -1; }

            _roomIndex = _isReturning ? -1 : 1;

            _col.gameObject.GetComponent<Scr_PlayerStateManager>()._startScenePosition = _starScenePosition;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + _roomIndex, LoadSceneMode.Single);
        }
    }
}