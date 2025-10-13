using AutoLetterbox;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class Scr_SceneManager : MonoBehaviour
{
    public static Scr_SceneManager _instance;

    [SerializeField] GameObject _player;

    public bool _isOnSceneTransition = false;

    public int _targetRoomNumber;
    public string _targetRoomName;

    [SerializeField] Animator _transitionAnim;
    [SerializeField] float _transitionTime;

    AsyncOperation asyncOperation;

    [SerializeField] GameObject _cameraRatio;

    private string _currentScene;

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

        _player = GameObject.FindWithTag("Player");
        _transitionAnim = GameObject.FindWithTag("CrossFade").GetComponent<Animator>();
    }

    private void Update()
    {
        if (_cameraRatio == null)
        {
            _cameraRatio = GameObject.FindGameObjectWithTag("CameraRatio");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGameBuild();
        }
    }

    IEnumerator LoadLevel()
    {
        _transitionAnim.SetTrigger("Start");

        string _previousScene = SceneManager.GetActiveScene().name;

        yield return new WaitForSeconds(_transitionTime);

        asyncOperation = SceneManager.LoadSceneAsync(_targetRoomName, LoadSceneMode.Additive);
        _currentScene = _targetRoomName;
        asyncOperation.allowSceneActivation = true;

        yield return asyncOperation;

        SceneManager.UnloadSceneAsync(_previousScene);

        if (_cameraRatio != null)
        {
            _cameraRatio.GetComponent<ForceCameraRatio>().FindAllCamerasInScene();
        }

        _transitionAnim.SetTrigger("End");
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject[] Exits = GameObject.FindGameObjectsWithTag("Exit");

        _player.GetComponent<Scr_PlayerStateManager>().pVariables._afterFirstFrame = false;
        _player.GetComponent<Scr_PlayerStateManager>().pVariables._canWalk = true;
        _player.GetComponent<Scr_PlayerStateManager>().pVariables._canChangeDirection = true;
        _isOnSceneTransition = false;

        foreach (GameObject ex in Exits)
        {
            if (ex.GetComponent<Scr_Exit>()._currentRoomNumber == _targetRoomNumber)
            {
                _player.transform.position = ex.GetComponent<Scr_Exit>()._startScenePosition.position;
            }
        }

        if (_currentScene == "Sword")
        {
            Scr_MusicManager._instance._musicSource.clip = null;
        }
        else
        {
            if (Scr_MusicManager._instance._musicSource.clip != Scr_MusicManager._instance._library.GetClipFromName("Area 1"))
            {
                Scr_MusicManager._instance.PlayMusic("Area 1");
                Scr_MusicManager._instance._musicSource.loop = true;
            }
        }
    }

    void RestartGameBuild()
    {
        Process.Start(Application.dataPath.Replace("_Data", ".exe"));
        Application.Quit();
    }
}
