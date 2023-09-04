using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplodeBlock : Block
{
    [SerializeField] private float explosionSpeed;

    protected override void OnCollisionEnter(Collision collision)
    {
        Explode(collision);
        PlayCrashEffect(collision.GetContact(0).point, sfxs.explodeBlockDestroyedSFX);
        DestroyBlock();
    }

    private void Explode(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<BallStart>(out BallStart ball))
        {
            if(!ball.hasExploded)
            {
                Vector3 explosionDirection = collision.GetContact(0).point - this.transform.position;
                Vector3 explosionVectorNormalize = new Vector3(explosionDirection.x, 0f, explosionDirection.z);
                explosionVectorNormalize.Normalize();
                ball.rb.AddForce(explosionDirection * explosionSpeed, ForceMode.VelocityChange);
                ball.hasExploded = true;
            }
        }
    }
}
