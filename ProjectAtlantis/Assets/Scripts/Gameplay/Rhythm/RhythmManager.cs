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
            OnTrackChange += ChangeSong;
            OnTrackChange += GameManager.Instance.AudioManager.UpdateBackgroundTrack;
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
            
            if (currentTimers[0] < leeway[0])
            {
                print("perfect");
                return Timing.Perfect;
            }
            else if (currentTimers[0] < leeway[1])
            {
                print("amazing");
                return Timing.Amazing;
            }
            else if (currentTimers[0] < leeway[2])
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
                slider.maxValue = currentBeat * 4; 
                slider.minValue = 0;
            }
            
            currentTimers[0] = currentBeat;
            currentTimers[1] = currentBeat*2;
            currentTimers[2] = currentBeat*3;
            
            currentTimers[3] = currentBeat;
            currentTimers[4] = currentBeat*2;
            currentTimers[5] = currentBeat*3;

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
            if (currentTimers[0] < 0)
            {
                print("hit");
                HitPerfect.Invoke();
                HandleSimpleTimers();
            }

            for (int i = 0; i < currentTimers.Count; i++)
            {
                currentTimers[i] -= Time.deltaTime;
                
                sliders[i].value = currentTimers[i];
            }
        }
        
        /// <summary>
        /// Places the timers at the correct spot.
        /// </summary>
        private void HandleSimpleTimers()
        {
            currentTimers[0] = currentTimers[1];
            currentTimers[1] = currentTimers[2];
            currentTimers[2] += currentBeat;
            
            currentTimers[3] = currentTimers[4];
            currentTimers[4] = currentTimers[5];
            currentTimers[5] += currentBeat;
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