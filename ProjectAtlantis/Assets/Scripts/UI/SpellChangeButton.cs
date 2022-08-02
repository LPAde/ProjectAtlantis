using System;
using System.Collections.Generic;
using Gameplay.Combat.Spells;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class SpellChangeButton : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler,
        IEndDragHandler
    {
        [SerializeField] private BaseSpell spell;
        [SerializeField] private GameObject image;
        [SerializeField] private float toleranceValue;

        private void Start()
        {
            gameObject.GetComponent<Image>().sprite = spell.SpellSprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // TODO: Message with drag and drop notification && Open Small Window with description.
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(MainMenuBehaviour.Instance.image != null)
                return;
            
            MainMenuBehaviour.Instance.image = Instantiate(image,Input.mousePosition,Quaternion.identity,transform);
            image.GetComponent<Image>().sprite = spell.SpellSprite;
        }

        public void OnDrag(PointerEventData eventData)
        {
            MainMenuBehaviour.Instance.image.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
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
                if ((MainMenuBehaviour.Instance.image.transform.position -
                     MainMenuBehaviour.Instance.MovementSpellImage.transform.position).magnitude < toleranceValue)
                {
                    movementSpell = moveSpell;
                    MainMenuBehaviour.Instance.MovementSpellImage.sprite = spell.SpellSprite;
                }
                else
                {
                    print((MainMenuBehaviour.Instance.image.transform.position -
                           MainMenuBehaviour.Instance.MovementSpellImage.transform.position).magnitude);
                }
            }
            else
            {
                // Distance Check.
                for (int i = 0; i < MainMenuBehaviour.Instance.CombatSpellImages.Count; i++)
                {
                    if ((MainMenuBehaviour.Instance.image.transform.position -
                         MainMenuBehaviour.Instance.CombatSpellImages[i].transform.position).magnitude < toleranceValue)
                    {
                        combatSpells[i] = (CombatSpell) spell;
                        MainMenuBehaviour.Instance.CombatSpellImages[i].sprite = spell.SpellSprite;
                        break;
                    }
                }
            }    

            // Overwriting the spells.
            SaveSystem.SetString
            ("PlayerSpells", string.Concat(MainMenuBehaviour.Instance.SpellManager.GetSpellID(combatSpells[0]), "*",
                MainMenuBehaviour.Instance.SpellManager.GetSpellID(combatSpells[1]), "*",MainMenuBehaviour.Instance.SpellManager.GetSpellID(combatSpells[2])
                , "*",MainMenuBehaviour.Instance.SpellManager.GetSpellID(movementSpell), "*"));
            
            Destroy(MainMenuBehaviour.Instance.image);
        }
    }
}