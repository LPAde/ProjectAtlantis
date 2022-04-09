using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerScripts
{
    public class RhythmManager : MonoBehaviour
    {
        [SerializeField] private Song currentSong;
        [SerializeField] private List<float> leeway;
        [SerializeField] private List<Slider> sliders;
        [SerializeField] private List<float> currentTimers;
        [SerializeField] private float currentBeat;

        private void Start()
        {
            currentBeat = 60 / currentSong.BPM;
            currentTimers[0] = currentBeat;
            currentTimers[1] = currentBeat*2;
            currentTimers[2] = currentBeat*3;
            
            currentTimers[3] = currentBeat;
            currentTimers[4] = currentBeat*2;
            currentTimers[5] = currentBeat*3;
            
            foreach (var slider in sliders)
            {
                slider.maxValue = currentBeat * 4; 
                slider.minValue = 0;
            }
        }

        private void Update()
        {
            if (currentTimers[0] < 0)
                HandleTimers();

            for (int i = 0; i < currentTimers.Count; i++)
            {
                currentTimers[i] -= Time.deltaTime;
                
                sliders[i].value = currentTimers[i];
            }
        }

        /// <summary>
        /// Checks the current timing.
        /// </summary>
        /// <returns> How close to the timing we currently are. </returns>
        public Timing CheckTiming()
        {
            if (currentTimers[0] < leeway[0])
            {
                HandleTimers();
                return Timing.Perfect;
            }
            if (currentTimers[0] < leeway[1])
            {
                HandleTimers();
                return Timing.Amazing;
            }

            if (currentTimers[0] < leeway[2])
            {
                HandleTimers();
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
            currentBeat = 60 / currentSong.BPM;
            
            foreach (var slider in sliders)
            {
                slider.maxValue = currentBeat * 4; 
                slider.minValue = 0;
            }
            
            currentTimers[0] = currentBeat;
            currentTimers[1] = currentBeat*2;
            currentTimers[2] = currentBeat*3;
        }

        private void HandleTimers()
        {
            currentTimers[0] = currentTimers[1];
            currentTimers[1] = currentTimers[2];
            currentTimers[2] += currentBeat;
            
            currentTimers[3] = currentTimers[4];
            currentTimers[4] = currentTimers[5];
            currentTimers[5] += currentBeat;
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