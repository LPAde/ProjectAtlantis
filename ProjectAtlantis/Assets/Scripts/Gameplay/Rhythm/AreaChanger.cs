using UnityEngine;

namespace Gameplay.Rhythm
{
   public class AreaChanger : MonoBehaviour
   {
      [SerializeField] private Song areaSong;
      [SerializeField] private AudioClip ambience;

      private void OnTriggerEnter(Collider other)
      {
         if (!other.CompareTag("Player")) 
            return;
         
         if(GameManager.Instance.RhythmManager.CurrentSong != areaSong)
         {
            GameManager.Instance.RhythmManager.OnTrackChange.Invoke(areaSong);
         }
         
         if(GameManager.Instance.AmbienceSound.clip == ambience)
            return;
         
         GameManager.Instance.AmbienceSound.clip = ambience;
         GameManager.Instance.AmbienceSound.Play();
      }
   }
}