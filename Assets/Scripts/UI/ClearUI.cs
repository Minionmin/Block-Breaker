using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearUI : MonoBehaviour
{
    public static ClearUI Instance { get; private set; }

    private DialogueHandler dialogueHandler;

    private void Awake()
    {
        // must be active in the hierarchy otherwise Awake won't be called
        // and will result in nullptr
        Instance = this;

        dialogueHandler = GetComponent<DialogueHandler>();
    }

    private void Start()
    {

    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(dialogueHandler.TypeDialogue("Clear!!!", false, SFXHandler.Instance.sfxSO.textSFX));
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
