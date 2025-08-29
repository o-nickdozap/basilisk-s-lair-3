using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_Exit : MonoBehaviour
{
    private GameObject _SceneManager;

    public Transform _startScenePosition;
    [SerializeField] string _targetRoomName;
    public int _currentRoomNumber;
    public int _targetRoomNumber;

    [SerializeField] Animator _transitionAnim;
    [SerializeField] float _transitionTime;

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
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        _transitionAnim.SetTrigger("Start");

        yield return new WaitForSeconds(_transitionTime);

        SceneManager.LoadScene(_targetRoomName, LoadSceneMode.Single);
        _SceneManager.GetComponent<Scr_SceneManager>()._targetRoomNumber = _targetRoomNumber;
    }

}