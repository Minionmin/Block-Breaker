using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IHitInterface
{
    public virtual void GetHit()
    {
        throw new System.NotImplementedException();
    }

    public virtual int GetHitToDestroy()
    {
        return 0;
    }
}
