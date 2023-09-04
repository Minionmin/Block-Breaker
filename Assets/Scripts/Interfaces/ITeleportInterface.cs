using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITeleportInterface
{
    public Transform destinationTransform { get; set; }

    public void TeleportObjectToAnother(Transform obj);
}
