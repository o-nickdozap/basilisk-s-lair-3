using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_Exit : MonoBehaviour
{
    [SerializeField] Vector2 _starScenePosition;
    [SerializeField] string _targetRoomName;

    void OnCollisionEnter2D(Collision2D _col)
    {
        if (_col.collider.CompareTag("Player")) {
            _col.gameObject.GetComponent<Scr_PlayerStateManager>()._startScenePosition = _starScenePosition;
            SceneManager.LoadScene(_targetRoomName, LoadSceneMode.Single);
        }
    }
}