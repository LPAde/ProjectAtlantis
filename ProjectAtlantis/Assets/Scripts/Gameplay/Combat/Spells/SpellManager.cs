using System.Collections.Generic;
using Gameplay.Combat.Projectiles;
using UnityEngine;

namespace Gameplay.Combat.Spells
{
    public class SpellManager : MonoBehaviour
    {
        [SerializeField] private List<BaseSpell> allPlayerSpells;

        [Header("Spell-Changing Related Stuff")] 
        [SerializeField] private List<bool> allUnlockedPlayerSpells;

        private void Awake()
        {
            if (GameManager.Instance != null)
            { 
                GameManager.Instance.Load += Load; 
                GameManager.Instance.Save += Save;
            }
        }

        private void Start()
        {
            if(GameManager.Instance == null)
            {
                Load();
            }
        }

        #region private Methods

        /// <summary>
        /// Loads the list of unlocked player Spells and activates them.
        /// </summary>
        private void Load()
        {
            string unlockedString = SaveSystem.GetString("UnlockedSpellsString");
            
            if(string.IsNullOrEmpty(unlockedString))
                return;
                
            var bools = unlockedString.Split("-");

            for (int i = 0; i < allUnlockedPlayerSpells.Count; i++)
            {
                if (bools[i] == "True")
                {
                    allUnlockedPlayerSpells[i] = true;
                }
                else
                {
                    allUnlockedPlayerSpells[i] = false;
                }
            }
        }

        /// <summary>
        /// Saves the list of unlocked spells.
        /// </summary>
        private void Save()
        {
            string unlockedString = "";

            for (int i = 0; i < allUnlockedPlayerSpells.Count; i++)
            {
                unlockedString += allUnlockedPlayerSpells[i].ToString();
                unlockedString += "-";
            }
            
            SaveSystem.SetString("UnlockedSpellsString", unlockedString);
        }

        #endregion

        /// <summary>
        /// Locks all non starting spells.
        /// </summary>
        public void LockAllSpells()
        {
            for (int i = 4; i < allUnlockedPlayerSpells.Count; i++)
            {
                allUnlockedPlayerSpells[i] = false;
            }
        }
        
        /// <summary>
        /// Gives you an id by its spell.
        /// </summary>
        /// <param name="spell"> The spell you want the id of. </param>
        /// <returns> The id of that spell. </returns>
        public int GetSpellID(BaseSpell spell)
        {
            // Fail-save, in case spell is not included.
            if (!allPlayerSpells.Contains(spell))
                return -1;

            // Finding the correct ID.
            int id = 0;

            for (int i = 0; i < allPlayerSpells.Count; i++)
            {
                if(spell != allPlayerSpells[i])
                    continue;

                id = i;
                break;
            }

            return id;
        }

        // Checks if your spell is unlocked.
        public bool CheckSpellUnlocked(BaseSpell spellToCheck)
        {
            for (int i = 0; i < allPlayerSpells.Count; i++)
            {
                if (allPlayerSpells[i] == spellToCheck)
                    return allUnlockedPlayerSpells[i];
            }

            return false;
        }
        
        /// <summary>
        /// Gives you a spell by its id.
        /// </summary>
        /// <param name="id"> The id you want the spell of. </param>
        /// <returns> The spell. </returns>
        public BaseSpell GetSpell(int id)
        {
            // Fail-save, in case spell is not included.
            if (id == -1)
                return null;

            return allPlayerSpells[id];
        }

        public void UnlockSpell(BaseSpell spellToUnlock)
        {
            for (int i = 0; i < allPlayerSpells.Count; i++)
            {
                if (spellToUnlock == allPlayerSpells[i])
                {
                    allUnlockedPlayerSpells[i] = true;
                }
            }
            
            Save();
        }
    }
}