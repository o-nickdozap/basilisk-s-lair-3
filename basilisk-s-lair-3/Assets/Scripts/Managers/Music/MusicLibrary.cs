using UnityEngine;

[System.Serializable]
public struct MusicTrack
{
    public string _trackName;
    public AudioClip clip;
}

public class MusicLibrary : MonoBehaviour
{
    public MusicTrack[] _tracks;
    
    public AudioClip GetClipFromName(string _trackName)
    {
        foreach (var track in _tracks)
        {
            if (track._trackName == _trackName)
            {
                return track.clip;
            }
        }

        return null;
    }
}
