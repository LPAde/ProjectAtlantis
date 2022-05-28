using UnityEngine;

namespace Gameplay.Rhythm
{
   public class AreaChanger : MonoBehaviour
   {
      [SerializeField] private Song areaSong;

      private void OnTriggerEnter(Collider other)
      {
         if (!other.CompareTag("Player")) 
            return;
         
         if(GameManager.Instance.RhythmManager.CurrentSong == areaSong)
            return;
         
         GameManager.Instance.RhythmManager.OnTrackChange.Invoke(areaSong);
      }
   }
}