using UnityEngine;

public class Scr_Bars : MonoBehaviour
{
    public static Scr_Bars _instance;

    void Awake()
    {
        /*if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }*/
    }
}
