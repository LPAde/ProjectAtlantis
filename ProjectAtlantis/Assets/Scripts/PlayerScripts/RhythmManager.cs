using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerScripts
{
    public class RhythmManager : MonoBehaviour
    {
        [SerializeField] private Song currentSong;
        [SerializeField] private float currentTimer;
        [SerializeField] private List<float> leeway;
        [SerializeField] private Slider slider;

        private void Start()
        {
            currentTimer = currentSong.beat;

            slider.maxValue = currentSong.beat;
            slider.minValue = 0;
        }

        private void Update()
        {
            if (currentTimer < 0)
                currentTimer = currentSong.beat;
            
            currentTimer -= Time.deltaTime;
            slider.value = currentTimer;
        }

        /// <summary>
        /// Checks the current timing.
        /// </summary>
        /// <returns> How close to the timing we currently are. </returns>
        public Timing CheckTiming()
        {
            if (currentTimer < leeway[0])
                return Timing.Perfect;
            if (currentTimer < leeway[1])
                return Timing.Amazing;
            return currentTimer < leeway[2] ? Timing.Good : Timing.Bad;
        }

        public void UpdateSong(Song newSong)
        {
            currentSong = newSong;
            
            slider.maxValue = currentSong.beat;
            slider.minValue = 0;
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