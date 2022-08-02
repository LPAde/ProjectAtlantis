using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace UI
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        [SerializeField] private List<Button> allButtons;
        [SerializeField] private List<string> allSaveFileStrings;
        [SerializeField] private List<GameObject> allWindows;
        [SerializeField] private string websiteUrl;
        
        [Header("Audio Related Stuff")] 
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip startGameSound;
        [SerializeField] private AudioClip buttonUISound;
        
        [Header("Spell Related Stuff")]
        [SerializeField] private List<Button> buttons; 
        [SerializeField] private List<VideoClip> cutsceneClips;
        [SerializeField] private Sprite lockedSprite;

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
            
            if(allWindows[1].activeSelf)
                OpenCutsceneCollection();
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

        private void OpenCutsceneCollection()
        {
            // Setup.
            string unlockedCutscenes = SaveSystem.GetString("UnlockedCutscenes");
            
            // Only activate buttons when there are activated buttons.
            if (unlockedCutscenes != String.Empty)
            {
                var cutscenes = unlockedCutscenes.Split("-");
                
                // Activate correct buttons.
                for (int i = 0; i < cutscenes.Length; i++)
                {
                    buttons[int.Parse(cutscenes[i])].interactable = true;
                }
            }
            
            // Change sprites of the locked cutscenes.
            for (int i = 0; i < buttons.Count; i++)
            {
                if(buttons[i].interactable)
                    continue;

                buttons[i].image.sprite = lockedSprite;
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