using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private int letterPerSecond;
    [SerializeField] private TextMeshProUGUI dialogueText;

    public IEnumerator TypeDialogue(string dialogue, bool shouldLoop, AudioClip textSFX = null)
    {
        dialogueText.text = "";

        yield return new WaitForSeconds(1f / letterPerSecond);

        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);

            if (textSFX != null)
            {
                SFXHandler.Instance.PlaySFX(textSFX, Camera.main.transform.position);
            }
        }

        if (shouldLoop)
        {
            StartCoroutine(TypeDialogue(dialogue, true));
        }
    }
}
