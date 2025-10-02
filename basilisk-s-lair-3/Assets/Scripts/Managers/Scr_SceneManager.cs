using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Scr_SceneManager : MonoBehaviour
{
    public static Scr_SceneManager _instance;

    [SerializeField] GameObject _player;

    public int _targetRoomNumber;
    public string _targetRoomName;

    [SerializeField] Animator _transitionAnim;
    [SerializeField] float _transitionTime;

    AsyncOperation asyncOperation;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    IEnumerator LoadLevel()
    {
        _transitionAnim.SetTrigger("Start");

        string _currentScene = SceneManager.GetActiveScene().name;

        yield return new WaitForSeconds(_transitionTime);

        asyncOperation = SceneManager.LoadSceneAsync(_targetRoomName, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = true;

        yield return asyncOperation;

        SceneManager.UnloadSceneAsync(_currentScene);

        _transitionAnim.SetTrigger("End");
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject[] Exits = GameObject.FindGameObjectsWithTag("Exit");

        _player.GetComponent<Scr_PlayerStateManager>().pVariables._afterFirstFrame = false;

        foreach (GameObject ex in Exits)
        {
            if (ex.GetComponent<Scr_Exit>()._currentRoomNumber == _targetRoomNumber)
            {
                _player.transform.position = ex.GetComponent<Scr_Exit>()._startScenePosition.position;
            }
        }
    }
}
