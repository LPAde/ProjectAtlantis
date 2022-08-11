using System.Collections.Generic;
using Gameplay.Combat.Spells;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class SpellChangeButton : MonoBehaviour, IBeginDragHandler, IDragHandler,
        IEndDragHandler
    {
        [SerializeField] private Button button;
        [SerializeField] private BaseSpell spell;
        [SerializeField] private GameObject image;
        [SerializeField] private float toleranceValue;

        [Header("Tool Tip related stuff")] 
        [SerializeField] private GameObject toolTip;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI coolDown;

        public Button Button => button;
        
        public void Start()
        {
            if(MainMenuBehaviour.Instance.SpellManager.CheckSpellUnlocked(spell))
            {
                button.image.sprite = spell.SpellSprite;
                description.text = spell.Description;
                coolDown.text = spell.MaxCooldown + "s";
                button.interactable = true;
            }
            else
            {
                button.image.sprite = MainMenuBehaviour.Instance.LockedSprite;
                button.interactable = false;
            }
        }

        public void OnClick()
        {
            toolTip.SetActive(true);
            MainMenuBehaviour.Instance.ToggleAllSpellButtons();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(!button.interactable)
                return;
            
            MainMenuBehaviour.Instance.Image = Instantiate(image,Input.mousePosition,Quaternion.identity,transform);
            MainMenuBehaviour.Instance.Image.GetComponent<Image>().sprite = spell.SpellSprite;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(!button.interactable)
                return;
            
            MainMenuBehaviour.Instance.Image.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(!button.interactable)
                return;
            
            // Setup.
            string spellString = SaveSystem.GetString("PlayerSpells");
            var idStrings = spellString.Split("*");
            
            List<CombatSpell> combatSpells = new List<CombatSpell>();
            for (int i = 0; i < MainMenuBehaviour.Instance.CombatSpellImages.Count; i++)
            {
                combatSpells.Add((CombatSpell)MainMenuBehaviour.Instance.SpellManager.GetSpell(int.Parse(idStrings[i])));
            }
            var movementSpell = (MovementSpell) MainMenuBehaviour.Instance.SpellManager.GetSpell(int.Parse(idStrings[3]));
            
            // Check type of Spell
            if(spell is MovementSpell moveSpell)
            {
                // Distance Check.
                if ((MainMenuBehaviour.Instance.Image.transform.position -
                     MainMenuBehaviour.Instance.MovementSpellImage.transform.position).magnitude < toleranceValue)
                {
                    movementSpell = moveSpell;
                    MainMenuBehaviour.Instance.MovementSpellImage.sprite = spell.SpellSprite;
                }
            }
            else if(spell is CombatSpell combatSpell)
            {
                // Distance Check.
                for (int i = 0; i < MainMenuBehaviour.Instance.CombatSpellImages.Count; i++)
                {
                    if ((MainMenuBehaviour.Instance.Image.transform.position -
                         MainMenuBehaviour.Instance.CombatSpellImages[i].transform.position).magnitude < toleranceValue)
                    {
                        bool hasSpellAlready = false;
                        // Check if spell is already taken.
                        for (int j = 0; j < combatSpells.Count; j++)
                        {
                            if (combatSpells[j] == combatSpell)
                            {
                                hasSpellAlready = true;
                                
                                // If both spells are the same we just end it.
                                if(i == j)
                                    break;

                                var temp = combatSpells[i];
                                combatSpells[i] = combatSpells[j];
                                combatSpells[j] = temp;

                                MainMenuBehaviour.Instance.CombatSpellImages[j].sprite = combatSpells[j].SpellSprite;
                                MainMenuBehaviour.Instance.CombatSpellImages[i].sprite = combatSpells[i].SpellSprite;
                                
                            }
                        }
                        if(!hasSpellAlready)
                        {
                            combatSpells[i] = combatSpell;
                            MainMenuBehaviour.Instance.CombatSpellImages[i].sprite = combatSpell.SpellSprite;
                            break;
                        }
                    }
                }
            }    

            // Overwriting the spells.
            SaveSystem.SetString
            ("PlayerSpells", string.Concat(MainMenuBehaviour.Instance.SpellManager.GetSpellID(combatSpells[0]), "*",
                MainMenuBehaviour.Instance.SpellManager.GetSpellID(combatSpells[1]), "*",MainMenuBehaviour.Instance.SpellManager.GetSpellID(combatSpells[2])
                , "*",MainMenuBehaviour.Instance.SpellManager.GetSpellID(movementSpell), "*"));
            
            Destroy(MainMenuBehaviour.Instance.Image);
        }
    }
}