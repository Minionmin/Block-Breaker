using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBlock : Block, ITeleportInterface
{
    [SerializeField] private Transform teleportDestination;
    private TeleportDoor teleportDoor;

    public Transform destinationTransform { get; set; }

    protected override void Start()
    {
        teleportDoor = teleportDestination.GetComponentInParent<TeleportDoor>();
        destinationTransform = teleportDestination;
        base.Start();
    }

    // Before implementing ITeleportInterface
    /*
    private void TeleportBall(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<BallStart>(out BallStart ball))
        {
            teleportDoor.teleportBlocks.Remove(gameObject);
            ball.transform.position = new Vector3 (teleportDestination.position.x, ball.transform.position.y, teleportDestination.position.z);
            PlayCrashEffect(teleportDestination.position, sfxs.teleportSFX);

            if (teleportDoor.HasNoBlockLeft())
            {
                teleportDoor.PlayTeleportDoorCloseEffect();
            }
        }
        else
        {
            return;
        }
    }
    
    protected override void OnCollisionEnter(Collision collision)
    {
        DestroyBlock();
    }
    */


    // ITeleportInterface
    // After implementing ITeleportInterface
    public void TeleportObjectToAnother(Transform obj)
    {
        teleportDoor.teleportBlocks.Remove(gameObject);

        obj.position = new Vector3(destinationTransform.position.x, obj.position.y, destinationTransform.position.z);

        PlayCrashEffect(destinationTransform.position, sfxs.teleportSFX);

        if (teleportDoor.HasNoBlockLeft())
        {
            teleportDoor.PlayTeleportDoorCloseEffect();
        }
    }
    // ## After implementing ITeleportInterface
}
