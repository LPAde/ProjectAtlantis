using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Combat.Spells;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace UI
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        public static MainMenuBehaviour Instance;
        
        [SerializeField] private List<Button> allButtons;
        [SerializeField] private List<string> allSaveFileStrings;
        [SerializeField] private List<GameObject> allWindows;
        [SerializeField] private string websiteUrl;
        
        [Header("Audio Related Stuff")] 
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip startGameSound;
        [SerializeField] private AudioClip buttonUISound;

        [Header("Spell Related Stuff")] 
        [SerializeField] private SpellManager spellManager;
        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private List<Image> combatSpellImages;
        [SerializeField] private Image movementSpellImage;
        
        public GameObject image { get; set; }
        public SpellManager SpellManager => spellManager;
        public Sprite LockedSprite => lockedSprite;
        public List<Image> CombatSpellImages => combatSpellImages;
        public Image MovementSpellImage => movementSpellImage;
        
        
        private void Awake()
        {
            if(Instance != null)
                Destroy(Instance);
            
            Instance = this;
            
            // Setup.
            string spellString = SaveSystem.GetString("PlayerSpells");
            var idStrings = spellString.Split("*");
            
            List<CombatSpell> combatSpells = new List<CombatSpell>();
            
            for (int i = 0; i < combatSpellImages.Count; i++)
            {
                combatSpells.Add((CombatSpell) SpellManager.GetSpell(int.Parse(idStrings[i])));
                combatSpellImages[i].sprite = combatSpells[i].SpellSprite;
            }
            var movementSpell = (MovementSpell) SpellManager.GetSpell(int.Parse(idStrings[3]));

            movementSpellImage.sprite = movementSpell.SpellSprite;
            
        }

        public void OnStartGameClick()
        {
            foreach (var button in allButtons)
            {
                button.interactable = false;
            }
            
            audioSource.clip = startGameSound;
            audioSource.Play();
            StartCoroutine(LoadYourAsyncScene());
        }
        
        /// <summary>
        /// Sets every Save value to zero.
        /// </summary>
        public void OnDeleteClick()
        {
            foreach (var t in allSaveFileStrings)
            {
                SaveSystem.SetString(t, string.Empty);
            }
            
            SaveSystem.SetVector3("PlayerPosition", Vector3.zero);
            SaveSystem.SetInt("UsedKeys", 0);
            SaveSystem.SetInt("UnlockedSpells", 0);
        }
        
        public void OnCreditsClick()
        {
            SceneManager.LoadScene(2);
        }

        public void OnWindowChangeClick(int activatedWindowIndex)
        {
            for (int i = 0; i < allWindows.Count; i++)
            {
                allWindows[i].SetActive(i == activatedWindowIndex);
            }
        }

        public void OnWebsiteClick()
        {
            Application.OpenURL(websiteUrl);
        }

        public void OnExitClick()
        {
            Application.Quit();
        }

        public void StartClip(Button btn)
        {
            // TODO: Do clip logic.
        }

        private IEnumerator LoadYourAsyncScene()
        {
            // Wait until the asynchronous scene fully loads
            while (audioSource.isPlaying)
            {
                yield return null;
            }
            
            SceneManager.LoadScene(1);
        }
    }
}