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
        [SerializeField] private bool playerActed;
        
        [Header("Visual Stuff")]
        [SerializeField] private List<Slider> sliders;
        [SerializeField] private List<float> currentTimers;
        
        [Header("Song Related Stuff")]
        [SerializeField] private Song currentSong;
        [SerializeField] private float currentBeat;

        public Action<Song> OnTrackChange;
        public Action HitPerfect;

        public Song CurrentSong => currentSong;
        
        private void Awake()
        {
            HitPerfect += ResetPlayerAction;
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
            if (playerActed)
                return Timing.Bad;
            
            playerActed = true;
            float currentTime = currentTimers[0] - GameManager.Instance.AudioManager.SongTime - currentBeat*.5f;
            
            if (currentTime < leeway[0])
            {
                print("perfect");
                return Timing.Perfect;
            }
            else if (currentTime < leeway[1])
            {
                print("amazing");
                return Timing.Amazing;
            }
            else if (currentTime < leeway[2])
            {
                print("good");
                return Timing.Good;
            }
            else
            {
                print("bad");
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
            if (currentTimers[0] - GameManager.Instance.AudioManager.SongTime - currentBeat*.5f < 0)
            {
                HitPerfect.Invoke();
                HandleSimpleTimers();
            }

            for (int i = 0; i < currentTimers.Count; i++)
            {
                sliders[i].value = currentTimers[i] - GameManager.Instance.AudioManager.SongTime - currentBeat*.5f;
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
        
        private void ResetPlayerAction()
        {
            playerActed = false;
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