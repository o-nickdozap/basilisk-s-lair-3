using AutoLetterbox;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public string _currentScene;
    string _currentArea;
    public float _currentAreaFadeDuration;

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

        UnityEngine.Debug.Log(CurrentArea() + " " + _currentAreaFadeDuration);
    }

    public string CurrentArea()
    {
        if (AreaSongList._instance._tutorialRooms.Contains(_currentScene))
        {
            _currentArea = "Tutorial";
            _currentAreaFadeDuration = 0.5f;
            return _currentArea;
        }

        else if (AreaSongList._instance._saveRooms.Contains(_currentScene))
        {
            _currentArea = "Save";
            _currentAreaFadeDuration = 1.2f;
            return _currentArea;
        }

        return _currentArea;
    }

    void RestartGameBuild()
    {
        Process.Start(Application.dataPath.Replace("_Data", ".exe"));
        Application.Quit();
    }
}
