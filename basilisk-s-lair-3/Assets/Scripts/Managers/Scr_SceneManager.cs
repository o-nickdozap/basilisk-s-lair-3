using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_SceneManager : MonoBehaviour
{
    public static Scr_SceneManager _instance;

    [SerializeField] GameObject _player;

    public int _targetRoomNumber;

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

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject[] Exits = GameObject.FindGameObjectsWithTag("Exit");

        foreach (GameObject ex in Exits)
        {
            if (ex.GetComponent<Scr_Exit>()._currentRoomNumber == _targetRoomNumber)
            {
                _player.transform.position = ex.GetComponent<Scr_Exit>()._startScenePosition.position;
            }
        }
    }
}
