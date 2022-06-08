using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField] private string npcName;
    [SerializeField] private List<string> dialog;

    private void OnTriggerStay(Collider other)
    {
        // Only talks when player is in range and presses a key.
        if (other.CompareTag("Player"))
        {
            if(GameManager.Instance.DialogBox.gameObject.activeSelf)
                return;
            
            if(Input.GetKeyDown(KeyCode.Return))
                Talk();
        }
    }

    /// <summary>
    /// Makes the npc talk.
    /// </summary>
    private void Talk()
    {
        GameManager.Instance.DialogBox.StartDialog(npcName, dialog);
    }
}
