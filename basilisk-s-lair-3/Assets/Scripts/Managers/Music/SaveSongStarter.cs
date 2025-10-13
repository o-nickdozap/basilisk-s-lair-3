using UnityEngine;

public class SaveSongStarter : MonoBehaviour
{
    void Start()
    {
        Scr_MusicManager._instance.PlayMusic("Save", 1.2f);
    }
}
