using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Combat.Spells
{
    public class SpellManager : MonoBehaviour
    {
        [SerializeField] private List<BaseSpell> allPlayerSpells;

        [Header("Spell-Changing Related Stuff")] 
        [SerializeField] private GameObject spellChooseWindow;
        [SerializeField] private List<bool> allUnlockedPlayerSpells;
        [SerializeField] private List<Button> spellButtons;
        [SerializeField] private Sprite lockedSpellSprite;
        [SerializeField] private BaseSpell targetedSpell;

        public bool SpellChooseWindowOpen => spellChooseWindow.activeSelf;
        
        private void Awake()
        {
            GameManager.Instance.Load += Load;
            GameManager.Instance.Save += Save;
        }

        private void Start()
        {
            // Image and button fixing.
            for (int i = 0; i < allUnlockedPlayerSpells.Count; i++)
            {
                if (!allUnlockedPlayerSpells[i])
                {
                    spellButtons[i].interactable = false;
                    spellButtons[i].image.sprite = lockedSpellSprite;
                }
                else
                {
                    spellButtons[i].image.sprite = allPlayerSpells[i].SpellSprite;
                }
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                spellChooseWindow.SetActive(true);
            }
            else
            {
                spellChooseWindow.SetActive(false);
            }
            
            if (!spellChooseWindow.activeSelf)
                return;
            
            if(targetedSpell == null)
                return;

            if (GameManager.Instance.Player.CombatSpells.Contains((CombatSpell) targetedSpell))
            {
                targetedSpell = null;
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameManager.Instance.Player.SetSpell(targetedSpell, 0);
                targetedSpell = null;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                GameManager.Instance.Player.SetSpell(targetedSpell, 1);
                targetedSpell = null;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                GameManager.Instance.Player.SetSpell(targetedSpell, 2);
                targetedSpell = null;
            }
        }

        #region private Methods

        /// <summary>
        /// Loads the list of unlocked player Spells and activates them.
        /// </summary>
        private void Load()
        {
            string unlockedString = SaveSystem.GetString("UnlockedSpells");
            print(unlockedString);
            if(string.IsNullOrEmpty(unlockedString))
                return;
                
            var bools = unlockedString.Split("-");

            for (int i = 0; i < allUnlockedPlayerSpells.Count; i++)
            {
                print(bools[i]);
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
            print(unlockedString);
            SaveSystem.SetString("UnlockedSpells", unlockedString);
        }

        #endregion

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

        public void OnSpellPress(Button btn)
        {
            if(!btn.enabled)
                return;

            int buttonIndex = -1;
            
            for (int i = 0; i < spellButtons.Count; i++)
            {
                if(spellButtons[i] != btn)
                    continue;

                buttonIndex = i;
                break;
            }

            if (allPlayerSpells[buttonIndex] is MovementSpell)
            {
                GameManager.Instance.Player.SetSpell(allPlayerSpells[buttonIndex], 3);
            }
            else
            {
                targetedSpell = allPlayerSpells[buttonIndex];
            }
        }
    }
}