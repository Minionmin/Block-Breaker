using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    [SerializeField] private float effectLifeTime = 0f;

    private void Start()
    {
        StartCoroutine(DestroyEffectGameObject());
    }

    private IEnumerator DestroyEffectGameObject()
    {
        yield return new WaitForSecondsRealtime(effectLifeTime);
        Destroy(gameObject);
    }
}
