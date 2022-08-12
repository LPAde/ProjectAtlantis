using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Rhythm
{
    public class RhythmManager : MonoBehaviour
    {
        [Header("Player Related Stuff")]
        [SerializeField] private List<float> leeway;
        [SerializeField] private List<float> leewayPercentages;
        
        [Header("Visual Stuff")]
        [SerializeField] private List<Slider> sliders;
        [SerializeField] private List<float> currentTimers;
        [SerializeField] private float sliderOffset;
        
        [Header("Song Related Stuff")]
        [SerializeField] private Song currentSong;
        [SerializeField] private float currentBeat;

        public Action<Song> OnTrackChange;

        public Song CurrentSong => currentSong;
        
        private void Awake()
        {
            OnTrackChange += GameManager.Instance.AudioManager.UpdateBackgroundTrack;
            OnTrackChange += ChangeSong;
        }

        private void Start()
        {
            OnTrackChange.Invoke(currentSong);
        }

        private void Update()
        {
            SimpleUpdate();
        }

        #region Public Methods
        
        /// <summary>
        /// Checks the current timing.
        /// </summary>
        /// <returns> How close to the timing we currently are. </returns>
        public Timing CheckTiming()
        {
            float currentTime = currentTimers[0] - GameManager.Instance.AudioManager.SongTime - sliderOffset;
            
            if (currentTime < leeway[0])
            {
                GameManager.Instance.HudManager.HitAnimation();
                return Timing.Perfect;
            }
            else if (currentTime < leeway[1])
            {
                GameManager.Instance.HudManager.HitAnimation();
                return Timing.Amazing;
            }
            else if (currentTime < leeway[2])
            {
                return Timing.Good;
            }
            else
            {
                return Timing.Bad;
            }
        }

        /// <summary>
        /// Updates the current song to a new one.
        /// </summary>
        /// <param name="newSong"> The new one that shall be played and played with. </param>
        private void ChangeSong(Song newSong)
        {
            currentSong = newSong;
            currentBeat = 60 / currentSong.initialBpm;
            
            foreach (var slider in sliders)
            {
                slider.maxValue = currentBeat * 3; 
                slider.minValue = 0;
            }

            float songTime = GameManager.Instance.AudioManager.SongTime;
            
            currentTimers[0] = currentBeat + songTime;
            currentTimers[1] = currentBeat*2 + songTime;
            currentTimers[2] = currentBeat*3 + songTime;
            
            currentTimers[3] = currentBeat + songTime;
            currentTimers[4] = currentBeat*2 + songTime;
            currentTimers[5] = currentBeat*3 + songTime;

            leeway[0] = currentBeat * leewayPercentages[0];
            leeway[1] = currentBeat * leewayPercentages[1];
            leeway[2] = currentBeat * leewayPercentages[2];
        }

        #endregion

        /// <summary>
        /// The update used for a simple song.
        /// </summary>
        private void SimpleUpdate()
        {
            // Check if current timing is the beat.
            if (currentTimers[0] - GameManager.Instance.AudioManager.SongTime - sliderOffset < 0)
            {
                HandleSimpleTimers();
            }

            for (int i = 0; i < currentTimers.Count; i++)
            {
                sliders[i].value = currentTimers[i] - GameManager.Instance.AudioManager.SongTime - sliderOffset;
            }
        }
        
        /// <summary>
        /// Places the timers at the correct spot.
        /// </summary>
        private void HandleSimpleTimers()
        {
            for (int i = 0; i < currentTimers.Count; i++)
            {
                currentTimers[i] += currentBeat;
            }
        }
    }
}

public enum Timing
{
    Bad,
    Good,
    Amazing,
    Perfect
}