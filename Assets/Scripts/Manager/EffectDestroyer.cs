using System.Collections;
using UnityEngine;

public class EffectDestroyer : MonoBehaviour
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
