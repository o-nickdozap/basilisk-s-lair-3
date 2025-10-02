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

    void OnCollisionEnter2D(Collision2D _col)
    {
        if (_col.collider.CompareTag("Player")) {
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        _SceneManager.GetComponent<Scr_SceneManager>()._targetRoomNumber = _targetRoomNumber;
        _SceneManager.GetComponent<Scr_SceneManager>()._targetRoomName = _targetRoomName;

        _SceneManager.GetComponent<Scr_SceneManager>().StartCoroutine("LoadLevel");
    }
}