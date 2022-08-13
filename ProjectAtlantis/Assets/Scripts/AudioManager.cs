using System.Collections.Generic;
using Gameplay.Rhythm;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[DefaultExecutionOrder(1)]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private Song currentSong;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private float timeSongHasPlayed;
    
    [SerializeField] private List<Slider> mixerSliders;

    public float SongTime => backgroundMusic.time;
    
    private void Awake()
    {
        Load();
    }

    private void Update()
    {
        if(backgroundMusic == null)
            return;

        if(GameManager.Instance.IsPaused)
            return;
        
        timeSongHasPlayed -= Time.deltaTime;
        
        // Starts a new song when the current one ends.
        if (timeSongHasPlayed <= 0)
        {
            GameManager.Instance.RhythmManager.OnTrackChange.Invoke(currentSong);
        }
    }

    public void RestartSong()
    {
        backgroundMusic.Play();
        backgroundMusic.time = backgroundMusic.clip.length - timeSongHasPlayed;
    }

    public void OnSliderChange(Slider slider)
    {
        mixer.SetFloat(slider.name, slider.value);
        Save();
    }
    
    /// <summary>
    /// Updates the background track and plays it.
    /// </summary>
    /// <param name="newSong"></param>
    public void UpdateBackgroundTrack(Song newSong)
    {
        currentSong = newSong;
        backgroundMusic.clip = newSong.song;
        timeSongHasPlayed = backgroundMusic.clip.length;
        
        backgroundMusic.Play();
    }

    private void Load()
    {
        string loadString = SaveSystem.GetString("Volumes");
        
        if(string.IsNullOrEmpty(loadString))
            return;
            
        var volumes = loadString.Split("*");
        
        // Convert string into mixer and slider values.
        mixer.SetFloat("Master", float.Parse(volumes[0]));
        mixer.SetFloat("Music", float.Parse(volumes[1]));
        mixer.SetFloat("SFX", float.Parse(volumes[2]));

        mixerSliders[0].value = float.Parse(volumes[0]);
        mixerSliders[1].value = float.Parse(volumes[1]);
        mixerSliders[2].value = float.Parse(volumes[2]);
    }

    private void Save()
    {
        string saveString = "";

        // Use slider values for save system.
        for (int i = 0; i < mixerSliders.Count; i++)
        {
            saveString += mixerSliders[i].value + "*";
        }
        
        SaveSystem.SetString("Volumes", saveString);
    }
}