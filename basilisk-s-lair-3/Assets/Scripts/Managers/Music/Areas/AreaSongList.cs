using System.Collections.Generic;
using UnityEngine;

public class AreaSongList : MonoBehaviour
{
    public static AreaSongList _instance;

    public List<string> _tutorialRooms = new List<string> { };
    public List<string> _saveRooms = new List<string> { };

    private void Awake()
    {
        _instance = this;
    }
}
