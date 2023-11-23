using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class BallStart : MonoBehaviour
{
    public float speed;
    public bool hasExploded;
    public Rigidbody rb;
    public Vector3 START_VECTOR = new Vector3(1, 0, 1);
    public Transform ballSpawnPoint;
    public LayerMask wallLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hasExploded = false;
    }

    void Update()
    {
        // Visual presented by ArrowUI.cs
        START_VECTOR = CalculateBallStartDirectionNormalized();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // After Implementing IHitInterface
        if (collision.gameObject.TryGetComponent<IHitInterface>(out IHitInterface hitable))
        {
            hitable.GetHit();
        }

        // Object might have been destroyed after get hit
        if (collision.gameObject == null) return;

        // After Implementing ITeleportInterface
        if (collision.gameObject.TryGetComponent<ITeleportInterface>(out ITeleportInterface teleportable))
        {
            teleportable.TeleportObjectToAnother(this.gameObject.transform);
        }

        // If ball hit explosion block
        if (hasExploded)
        {
            int targetLayerMaskBit = collision.gameObject.layer;

            // Reset velocity if ball hits something else that is not wall
            if (1 << targetLayerMaskBit != wallLayer.value)
            {
                rb.velocity = rb.velocity.normalized * speed;
                hasExploded = false;
            }
        }
        else
        {
            // Make ball faster every collision
            rb.velocity *= 1.01f;

            if (Mathf.Abs(rb.velocity.x) < 5)
            {
                Vector3 v = rb.velocity;
                v.x *= 3;
                rb.velocity = v;
            }

            if (Mathf.Abs(rb.velocity.z) < 5)
            {
                Vector3 v = rb.velocity;
                v.z *= 3;
                rb.velocity = v;
            }
        }
    }

    private Vector3 CalculateBallStartDirectionNormalized()
    {
        Vector3 dir = ArrowUI.Instance.transform.forward;
        // flip z
        dir = new Vector3(dir.x, 0f, dir.z);
        return dir.normalized;
    }
}
