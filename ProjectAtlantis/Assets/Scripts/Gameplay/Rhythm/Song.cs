using UnityEngine;

namespace Gameplay.Rhythm
{
    [CreateAssetMenu(fileName = "BaseSong", menuName = "Song/Base Song", order = 0)]
    public class Song : ScriptableObject
    {
        public AudioClip song;
        public float initialBpm;
    }
}
