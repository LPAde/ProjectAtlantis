using System;
using Gameplay.Rhythm;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private Song currentSong;

    private void Awake()
    {
        GameManager.Instance.RhythmManager.OnTrackChange += UpdateBackgroundTrack;
    }

    private void Update()
    {
        print(backgroundMusic.isPlaying);
        // Starts a new song when the current one ends.
        if(!backgroundMusic.isPlaying)
            GameManager.Instance.RhythmManager.OnTrackChange.Invoke(currentSong);
    }

    /// <summary>
    /// Updates the background track and plays it.
    /// </summary>
    /// <param name="newSong"></param>
    private void UpdateBackgroundTrack(Song newSong)
    {
        currentSong = newSong;
        backgroundMusic.clip = newSong.song;
        backgroundMusic.Play();
    }
}
