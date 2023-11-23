using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    public List<GameObject> teleportBlocks = new List<GameObject>();
    [SerializeField] public GameObject teleportDoorCloseEffect;

    public bool HasNoBlockLeft()
    {
        return teleportBlocks.Count <= 0;
    }

    public void PlayTeleportDoorCloseEffect()
    {
        SFXHandler.Instance.PlaySFX(SFXHandler.Instance.sfxSO.teleportDoorDestroyedSFX, Camera.main.transform.position);
        GameObject newObject = Instantiate(teleportDoorCloseEffect, transform.position, teleportDoorCloseEffect.transform.rotation);

        // Destroy this portal only in normal mode
        if (GameHandler.Instance.GetGamemode() == GameHandler.Gamemode.Normal)
        {
            Destroy(gameObject);
        }

    }
}
