using System.Collections;
using UnityEngine;

public class Scr_MusicManager : MonoBehaviour
{
    public static Scr_MusicManager _instance;

    [SerializeField] public MusicLibrary _library;
    [SerializeField] public AudioSource _musicSource;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayMusic(string trackName, float fadeDuration = .5f)
    {
        StartCoroutine(AnimateMusicCrossfade(_library.GetClipFromName(trackName), fadeDuration));
    }

    IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration = .5f)
    {
        float percent = 0;

        while(percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            _musicSource.volume = Mathf.Lerp(1f, 0f, percent);
            yield return null;
        }

        _musicSource.clip = nextTrack;
        _musicSource.Play();

        percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            _musicSource.volume = Mathf.Lerp(0f, 1f, percent);
            yield return null;
        }
    }
}
