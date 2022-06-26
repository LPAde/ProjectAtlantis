using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Collectibles
{
    public class DriftBottle : BaseItem
    {
        protected override void OnCollecting()
        {
            string currentlyUnlockedCutscenes = SaveSystem.GetString("UnlockedCutscenes");
            var cutscenes = currentlyUnlockedCutscenes.Split("-");
            
            // Stop when all cutscenes have been unlocked.
            if(cutscenes.Length >= 10)
                return;
            
            List<int> numbers = cutscenes.Select(int.Parse).ToList();

            bool foundPotentialClip = false;

            int index = 0;
            
            // Trying random numbers to add a clip.
            while (!foundPotentialClip)
            {
                var random = Random.Range(0, 10);
                index++;
                
                if(index > 100)
                    break;
                
                if(numbers.Contains(random))
                    continue;

                currentlyUnlockedCutscenes += "-" + random;
                foundPotentialClip = true;
            }

            SaveSystem.SetString("UnlockedCutscenes", currentlyUnlockedCutscenes);
            GameManager.Instance.Save.Invoke();
            
            base.OnCollecting();
        }
    }
}
