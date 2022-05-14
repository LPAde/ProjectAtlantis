using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Rhythm
{
    [CreateAssetMenu(fileName = "ComplexSong", menuName = "Song/Complex Song", order = 1)]
    public class ComplexSong : Song
    { 
        public List<int> beatCounts;
        public List<float> followingBpms;
    }
}