using UnityEngine;

namespace PlayerScripts
{
    public class RhythmManager : MonoBehaviour
    {
        [SerializeField] private int currentTime;
        
        public Timing CheckTiming()
        {
            return Timing.Bad;
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