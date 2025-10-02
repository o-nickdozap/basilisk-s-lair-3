using UnityEngine;

public class Scr_SceneTransition : MonoBehaviour
{
    public static Scr_SceneTransition _instance;

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
}
