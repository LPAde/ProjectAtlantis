using Gameplay.Rhythm;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private Song currentSong;
    [SerializeField] private AudioMixer mixer;

    private void Awake()
    {
        GameManager.Instance.RhythmManager.OnTrackChange += UpdateBackgroundTrack;
    }

    private void Update()
    {
        // Starts a new song when the current one ends.
        if(!backgroundMusic.isPlaying && !GameManager.Instance.IsPaused)
            GameManager.Instance.RhythmManager.OnTrackChange.Invoke(currentSong);
    }

    public void OnSliderChange(Slider slider)
    {
        mixer.SetFloat(slider.name, slider.value);
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

    private void Load()
    {
        string loadString = SaveSystem.GetString("Volumes");
    }

    private void Save()
    {
        
    }
}