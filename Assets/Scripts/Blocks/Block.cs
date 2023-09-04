using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] protected GameObject crashEffectObject;

    protected SO_SFXs sfxs;

    protected virtual void Start()
    {
        sfxs = SFXHandler.Instance.sfxSO;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        PlayCrashEffect(collision.GetContact(0).point, sfxs.blockDestroyedSFX);
        DestroyBlock();
    }

    protected virtual void DestroyBlock()
    {
        GameHandler.Instance.RemoveObject(gameObject);

        if (GameHandler.Instance.IsNoBlockLeft())
        {
            GameHandler.Instance.HasWon();
        }
    }

    protected void PlayCrashEffect(Vector3 playPosition, AudioClip sfxClip)
    {
        SFXHandler.Instance.PlaySFX(sfxClip, Camera.main.transform.position);
        GameObject newObject = Instantiate(crashEffectObject, playPosition, crashEffectObject.transform.rotation);
    }
}
