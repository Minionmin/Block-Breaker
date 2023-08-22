using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BlockBreak : MonoBehaviour
{
    [SerializeField] private GameObject crashEffectObject;

    private void OnCollisionEnter(Collision collision)
    {
        playCrashEffect(collision);
        DestoyBlock();
    }

    private void DestoyBlock()
    {
        GameHandler.Instance.RemoveObject(gameObject);

        if (GameHandler.Instance.IsNoBlockLeft())
        {
            GameHandler.Instance.HasWon();
        }
    }

    private void playCrashEffect(Collision collision)
    {
        GameObject newObject = Instantiate(crashEffectObject, collision.GetContact(0).point, crashEffectObject.transform.rotation);
    }
}
