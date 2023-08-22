using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private int letterPerSecond;
    [SerializeField] private TextMeshProUGUI dialogueText;

    public IEnumerator TypeDialogue(string dialogue, bool shouldLoop)
    {
        dialogueText.text = "";

        yield return new WaitForSeconds(1f / letterPerSecond);

        if (shouldLoop)
        {
            foreach (char letter in dialogue.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(1f / letterPerSecond);
            }
            StartCoroutine(TypeDialogue(dialogue, true));
        }
        else
        {
            foreach (char letter in dialogue.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(1f / letterPerSecond);
            }
        }
    }
}
