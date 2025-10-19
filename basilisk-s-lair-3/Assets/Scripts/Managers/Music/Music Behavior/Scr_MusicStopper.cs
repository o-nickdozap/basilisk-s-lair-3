using UnityEngine;

public class Scr_MusicStopper : MonoBehaviour
{
    void Start()
    {
        Scr_MusicManager._instance.PlayMusic(null, 1.2f);
    }
}
