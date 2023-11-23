using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    private DialogueHandler dialogueHandler;
    private SpriteHandler spriteHandler;

    private void Awake()
    {
        dialogueHandler = GetComponent<DialogueHandler>();
        spriteHandler = GetComponent<SpriteHandler>();
    }

    private void Start()
    {
        StartCoroutine(dialogueHandler.TypeDialogue(". . . ", true));
        StartCoroutine(spriteHandler.RunSprite());
        StartCoroutine(Loader.LoaderCallback(Random.Range(1f, 3f)));
    }
}
