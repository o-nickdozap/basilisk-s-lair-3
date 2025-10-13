//using UnityEditor.PackageManager;
using UnityEngine;

public class Scr_Exit : MonoBehaviour
{
    private GameObject _SceneManager;

    public Transform _startScenePosition;
    [SerializeField] string _targetRoomName;
    public int _currentRoomNumber;
    public int _targetRoomNumber;

    private void Awake()
    {
        _SceneManager = GameObject.FindWithTag("SceneManager");
    }

    private void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.CompareTag("Player"))
        {
            if (!_SceneManager.GetComponent<Scr_SceneManager>()._isOnSceneTransition)
            {
                LoadNextLevel();

                _col.GetComponent<Scr_PlayerStateManager>().pVariables._canWalk = false;
                _col.GetComponent<Scr_PlayerStateManager>().pVariables._canChangeDirection = false;
            }
        }
    }

    void LoadNextLevel()
    {
        _SceneManager.GetComponent<Scr_SceneManager>()._targetRoomNumber = _targetRoomNumber;
        _SceneManager.GetComponent<Scr_SceneManager>()._targetRoomName = _targetRoomName;

        _SceneManager.GetComponent<Scr_SceneManager>()._isOnSceneTransition = true;
        _SceneManager.GetComponent<Scr_SceneManager>().StartCoroutine("LoadLevel");
    }
}