using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredBlock : Block, IHitInterface
{
    [SerializeField] protected int hitToDestroy;
    private HitUI hitUI;

    Collision hitCollision;

    private void Awake()
    {
        hitUI = GetComponentInChildren<HitUI>();
    }

    protected override void Start()
    {
        hitUI.SetHitText(hitToDestroy);
        base.Start();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        hitCollision = collision;
    }

    // IHitInterface
    // After implementing IHitInterface
    public int GetHitToDestroy()
    {
        return hitToDestroy;
    }

    public void GetHit()
    {
        if(hitToDestroy > 0)
        {
            hitToDestroy -= 1;
        }

        if (GetHitToDestroy() <= 0)
        {
            base.PlayCrashEffect(hitCollision.GetContact(0).point, sfxs.blockDestroyedSFX);
            hitCollision = null;
            base.DestroyBlock();
        }
        else
        {
            // UI visual doesn't need to know about logic
            hitUI.SetHitText(hitToDestroy);
        }
    }
    // ## After Implementing IHitInterface 

    public void SetHit(int val)
    {
        hitToDestroy = val;
        hitUI.SetHitText(hitToDestroy);
    }
}
