using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBar : MonoBehaviour
{
    [SerializeField] private float movespeed;
    [SerializeField] private LayerMask wallLayerMask;

    void Update()
    {
        float moveDistance = movespeed * Time.deltaTime;
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);

        float castDistance = 2.5f;
        bool canMove = !Physics.Raycast(transform.position, moveDirection, castDistance, wallLayerMask);

        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }
    }

}
