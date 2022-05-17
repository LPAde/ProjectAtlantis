using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Rhythm
{
    public class RhythmManager : MonoBehaviour
    {
        [Header("Player Related Stuff")]
        [SerializeField] private List<Slider> sliders;
        [SerializeField] private List<float> leeway;
        [SerializeField] private List<float> currentTimers;
        [SerializeField] private bool playerActed;
        
        [Header("Song Related Stuff")]
        [SerializeField] private Song currentSong;
        [SerializeField] private float currentBeat;
        
        [Header("Complex Song Related Stuff")]
        [SerializeField] private List<int> beatCounts;
        [SerializeField] private List<float> followingBeats;
        [SerializeField] private bool isComplex;
        [SerializeField] private int currentIndex;

        public Action HitPerfect;
        
        private void Start()
        {
            HitPerfect += ResetPlayerAction;
            
            UpdateSong(currentSong);
        }

        private void Update()
        {
            if (isComplex)
            {
                ComplexUpdate();
            }
            else
            {
                SimpleUpdate();   
            }
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
                return Timing.Perfect;
            }
            if (currentTimers[0] < leeway[1])
            {
                return Timing.Amazing;
            }

            if (currentTimers[0] < leeway[2])
            {
                return Timing.Good;
            }
            
            return Timing.Bad;
        }

        /// <summary>
        /// Updates the current song to a new one.
        /// </summary>
        /// <param name="newSong"> The new one that shall be played and played with. </param>
        public void UpdateSong(Song newSong)
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

            if (currentSong is ComplexSong cSong)
            {
                beatCounts = cSong.beatCounts;

                for (int i = 0; i < followingBeats.Count; i++)
                {
                    followingBeats[i] = 60 / cSong.followingBpms[i];
                }
                
                isComplex = true;
            }
            else
            {
                isComplex = false;
            }
        }

        #endregion

        /// <summary>
        /// The update used for a simple song.
        /// </summary>
        private void SimpleUpdate()
        {
            if (currentTimers[0] < 0)
            {
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
        /// The update used for a complex song.
        /// </summary>
        private void ComplexUpdate()
        {
            if (currentTimers[0] < 0)
            {
                HitPerfect.Invoke();
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

        /// <summary>
        /// Does wild shit yknow.
        /// </summary>
        private void HandleComplexTimers()
        {
            
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