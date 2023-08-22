using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBall : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 START_VECTOR = new Vector3(1,0,1);
    private float speed = 14f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.AddForce(START_VECTOR * speed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //rb.velocity *= 1.02f;

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
}
