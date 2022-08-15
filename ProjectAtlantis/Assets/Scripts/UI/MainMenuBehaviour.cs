using System.Collections;
using System.Collections.Generic;
using Gameplay.Combat.Spells;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    [DefaultExecutionOrder(0)]
    public class MainMenuBehaviour : MonoBehaviour
    {
        public static MainMenuBehaviour Instance;
        
        [SerializeField] private List<Button> allButtons;
        [SerializeField] private List<Button> qualityButtons;
        [SerializeField] private List<string> allSaveFileStrings;
        [SerializeField] private List<GameObject> allWindows;
        [SerializeField] private string websiteUrl;
        [SerializeField] private List<TextMeshProUGUI> highScores;
        [SerializeField] private SaveSystemSetup setup;
        
        [Header("Audio Related Stuff")] 
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip startGameSound;

        [Header("Spell Related Stuff")] 
        [SerializeField] private SpellManager spellManager;
        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private List<Image> combatSpellImages;
        [SerializeField] private Image movementSpellImage;
        [SerializeField] private Transform movementSpellImageBorder;
        [SerializeField] private List<BaseSpell> startingSpells;
        [SerializeField] private List<SpellChangeButton> spellChangeButtons;
        
        public GameObject Image { get; set; }
        public SpellManager SpellManager => spellManager;
        public Sprite LockedSprite => lockedSprite;
        public List<Image> CombatSpellImages => combatSpellImages;
        public Image MovementSpellImage => movementSpellImage;
        public Transform MovementSpellImageBorder => movementSpellImageBorder;
        
        private void Awake()
        {
            if(Instance != null)
                Destroy(Instance);
            
            Instance = this;

            // Dirtier than Frankfurt.
            setup.Awake();
            
            // As dirty as the line above.
            if(Screen.width / Screen.height > 1.8f)
            {
                Screen.SetResolution(2560, 1440, FullScreenMode.FullScreenWindow);
            }

            if(SaveSystem.GetInt("GameQuality") < 1)
                SaveSystem.SetInt("GameQuality", 1);

            if (SaveSystem.GetInt("GameQuality") == 1)
            {
                qualityButtons[0].interactable = true;
                qualityButtons[1].interactable = false;
            }
            else
            {
                qualityButtons[0].interactable = false;
                qualityButtons[1].interactable = true;
            }
            
            string spellString;
            
            // Setup.
            spellString = SaveSystem.GetString("PlayerSpells");

            // First time.
            if (string.IsNullOrEmpty(spellString))
            {
                OnDeleteClick();
                allButtons[2].interactable = false;
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

            string playerStats = SaveSystem.GetString("PlayerStats");

            if (string.IsNullOrEmpty(playerStats))
            {
                allButtons[2].interactable = false;
                return;
            }
            
            var stats = playerStats.Split("*");
            
            highScores[0].text = stats[0];
            highScores[1].text = stats[2];
            highScores[2].text = stats[3];
            highScores[3].text = stats[4];
            highScores[4].text = stats[5];
            highScores[5].text = stats[6];
            highScores[6].text = SaveSystem.GetInt("BestWave").ToString();
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
            SaveSystem.SetInt("BestWave", 0);
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

            foreach (var t in spellChangeButtons)
            {
                t.Start();
            }
            
            allButtons[2].interactable = false;
            
            audioSource.Play();
        }
        
        public void OnCreditsClick()
        {
            SceneManager.LoadScene(2);
            
            audioSource.Play();
        }

        public void OnWindowChangeClick(int activatedWindowIndex)
        {
            for (int i = 0; i < allWindows.Count; i++)
            {
                allWindows[i].SetActive(i == activatedWindowIndex);
            }
            
            audioSource.Play();
        }

        public void OnWebsiteClick()
        {
            Application.OpenURL(websiteUrl);
            
            audioSource.Play();
        }

        public void OnExitClick()
        {
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

        public void ToggleQuality()
        {
            int gameQuality = SaveSystem.GetInt("GameQuality");
            gameQuality = gameQuality == 1 ? 3 : 1;
            SaveSystem.SetInt("GameQuality", gameQuality);
            audioSource.Play();
        }
        
        private IEnumerator LoadYourAsyncScene()
        {
            // Wait until the asynchronous scene fully loads
            while (audioSource.isPlaying)
            {
                yield return null;
            }
            
            SceneManager.LoadScene(SaveSystem.GetInt("GameQuality"));
        }
    }
}