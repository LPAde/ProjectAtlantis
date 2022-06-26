using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace UI
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        [SerializeField] private List<Button> buttons; 
        [SerializeField] private List<VideoClip> cutsceneClips;
        [SerializeField] private Sprite lockedSprite;
        
        public void OnStartGameClick()
        {
            SceneManager.LoadScene(1);
        }
        
        public void OnDeleteClick()
        {
            
        }
        
        public void OnCreditsClick()
        {
            SceneManager.LoadScene(2);
        }

        public void StartClip(Button btn)
        {
            // TODO: Do clip logic.
        }

        private void OpenCutsceneCollection()
        {
            // Setup.
            string unlockedCutscenes = SaveSystem.GetString("UnlockedCutscenes");
            var cutscenes = unlockedCutscenes.Split("-");

            // Activate correct buttons.
            for (int i = 0; i < cutscenes.Length; i++)
            {
                buttons[int.Parse(cutscenes[i])].interactable = true;
            }
            
            // Change sprites of the locked cutscenes.
            for (int i = 0; i < buttons.Count; i++)
            {
                if(buttons[i].interactable)
                    continue;

                buttons[i].image.sprite = lockedSprite;
            }
        }
    }
}