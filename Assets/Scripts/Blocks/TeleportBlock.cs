using UnityEngine;

public class TeleportBlock : Block, ITeleportInterface
{
    protected override void Start()
    {
        base.Start();
    }

    public override void GetHit()
    {
        DropItem();
        TeleportTo(GameHandler.Instance.GetTeleportDestination().transform);

        // Play teleport VFX at the block
        PlayCrashEffect(transform.position, sfxs.blockDestroyedSFX);
        GameHandler.Instance.DestroyBlock(this, true);
    }

    /// <summary> Move this object to target position </summary>
    public void TeleportTo(Transform targetTransform)
    {
        // Play teleport VFX at target portal
        PlayCrashEffect(targetTransform.position, sfxs.teleportSFX);
        GameHandler.Instance.ball.transform.position = new Vector3(targetTransform.position.x, 0f, targetTransform.position.z);
    }
}
