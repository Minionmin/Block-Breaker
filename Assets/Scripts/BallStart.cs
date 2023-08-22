using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallStart : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;
    public Vector3 START_VECTOR = new Vector3(1, 0, 1);
    public Transform ballSpawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Visual presented by ArrowUI.cs
        START_VECTOR = CalculateBallStartDirectionNormalized();
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity *= 1.02f;

        if (Mathf.Abs(rb.velocity.x) < 5)
        {
            Vector3 v = rb.velocity;
            v.x *= 5;
            rb.velocity = v;
        }

        if (Mathf.Abs(rb.velocity.z) < 5)
        {
            Vector3 v = rb.velocity;
            v.z *= 5;
            rb.velocity = v;
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
