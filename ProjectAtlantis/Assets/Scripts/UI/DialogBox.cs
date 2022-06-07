using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DialogBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI dialogBoxText;
        [SerializeField] private List<string> currentText;
        [SerializeField] private string currentTalker;
        [SerializeField] private int index;

        private void Update()
        {
            Inputs();
        }

        /// <summary>
        /// Sets up a dialog and starts it.
        /// </summary>
        /// <param name="newTalker"> The person who is currently talking. </param>
        /// <param name="newText"> What that person is talking about. </param>
        public void StartDialog(string newTalker, List<string> newText)
        {
            gameObject.SetActive(true);
            currentTalker = newTalker;
            currentText = newText;
            index = -1;
        }

        private void Inputs()
        {
            if (!Input.GetKeyDown(KeyCode.Return))
                return;
            
            index++;

            if (index >= currentText.Count)
            {
                gameObject.SetActive(false);
            }
            else
            {
                dialogBoxText.text = string.Concat(currentTalker, ": ", currentText[index]);
            }
        }
    }
}