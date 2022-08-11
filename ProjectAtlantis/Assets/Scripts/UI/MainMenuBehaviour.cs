using System.Collections;
using System.Collections.Generic;
using Gameplay.Combat.Spells;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        [SerializeField] private List<BaseSpell> startingSpells;
        [SerializeField] private List<SpellChangeButton> spellChangeButtons;
        
        public GameObject Image { get; set; }
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

            // First time.
            if (string.IsNullOrEmpty(spellString))
            {
                OnDeleteClick();
                return;
            }
            
            // Not first time.
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
            spellManager.LockAllSpells();
            
            
            
            for (int i = 0; i < combatSpellImages.Count; i++)
            {
                combatSpellImages[i].sprite = startingSpells[i].SpellSprite;
                SpellManager.UnlockSpell(startingSpells[i]);
            }
            

            movementSpellImage.sprite = startingSpells[3].SpellSprite;
            SpellManager.UnlockSpell(startingSpells[3]);
            

            SaveSystem.SetString
            ("PlayerSpells", string.Concat(SpellManager.GetSpellID(startingSpells[0]), "*",
                SpellManager.GetSpellID(startingSpells[1]), "*",SpellManager.GetSpellID(startingSpells[2])
                , "*",Instance.SpellManager.GetSpellID(startingSpells[3]), "*"));

            for (int i = 0; i < spellChangeButtons.Count; i++)
            {
                spellChangeButtons[i].Start();
            }
            
            audioSource.clip = buttonUISound;
            audioSource.Play();
        }
        
        public void OnCreditsClick()
        {
            SceneManager.LoadScene(2);
            
            audioSource.clip = buttonUISound;
            audioSource.Play();
        }

        public void OnWindowChangeClick(int activatedWindowIndex)
        {
            for (int i = 0; i < allWindows.Count; i++)
            {
                allWindows[i].SetActive(i == activatedWindowIndex);
            }
            
            audioSource.clip = buttonUISound;
            audioSource.Play();
        }

        public void OnWebsiteClick()
        {
            Application.OpenURL(websiteUrl);
            
            audioSource.clip = buttonUISound;
            audioSource.Play();
        }

        public void OnExitClick()
        {
            audioSource.clip = buttonUISound;
            audioSource.Play();
            Application.Quit();
        }

        public void ToggleAllSpellButtons()
        {
            foreach (var btn in spellChangeButtons)
            {
                btn.Button.interactable = !btn.Button.interactable;
            }
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