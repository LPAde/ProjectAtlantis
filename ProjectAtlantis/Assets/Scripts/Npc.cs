using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField] private string npcName;
    [SerializeField] private List<string> dialog;

    /// <summary>
    /// Makes the npc talk.
    /// </summary>
    private void Talk()
    {
        GameManager.Instance.DialogBox.StartDialog(npcName, dialog);
    }
}
