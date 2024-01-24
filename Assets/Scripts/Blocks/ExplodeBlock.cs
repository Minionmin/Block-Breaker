using UnityEngine;

public class ExplodeBlock : Block
{
    /// <summary> Speed multiplier when the block exploded </summary>
    [SerializeField] private float explosionSpeedMultiplier;

    public override void GetHit()
    {
        DropItem();
        Explode();
        PlayCrashEffect(transform.position, sfxs.explodeBlockDestroyedSFX);
        GameHandler.Instance.DestroyBlock(this);
    }

    /// <summary> Make the ball significantly faster by explosion </summary>
    private void Explode()
    {
        // Play Camera shake effect when exploded
        CameraManager.Instance.Shake(0.3f, 0.4f, 30);

        Ball ball = GameHandler.Instance.ball;

        // If the ball hasn't exploded yet
        if (!ball.hasExploded)
        {
            // Multiply ball speed, raise the flag and activate trail VFX
            ball.Explode(ball.GetBallSpeed() * explosionSpeedMultiplier);

        }
    }
}
