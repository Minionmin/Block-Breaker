using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    /// <summary> Save camera position for resetting after Camera shake effect </summary>
    private Vector3 originalPosition;

    /// <summary> Shake effect offset </summary>
    private Vector3 _shakeOffset;

    /// <summary> Camera shake animation </summary>
    private Sequence _shakeSeq;

    private void Awake()
    {
        Instance = this;
        originalPosition = transform.position;
    }

    private void Update()
    {
        transform.position += _shakeOffset;
    }

    /// <summary> Camera shake effect </summary>
    public void Shake(float duration = 0.3f, float strength = 0.02f, int vibrato = 30)
    {
        // If the camera is shaking, kill the effect and play it again
        _shakeSeq?.Kill();

        // In 0.3sAshake into random direction (in 0.02m) 30 times
        _shakeSeq = DOTween.Sequence()
            .SetLink(gameObject)
            .Append(DOTween.Shake(() => Vector3.zero, offset => _shakeOffset = offset, duration, strength, vibrato))
            .OnComplete(() => { transform.position = originalPosition; }); // Reset camera position after shaking randomly
    }
}
