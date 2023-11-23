using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBar : MonoBehaviour
{
    public static MoveBar Instance {  get; private set; }

    [SerializeField] private float movespeed;
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private Transform bar;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        float moveDistance = movespeed * Time.deltaTime;
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);

        float castDistance = bar.transform.localScale.x / 2.0f;
        bool canMove = !Physics.Raycast(transform.position, moveDirection, castDistance, wallLayerMask);

        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }
        else
        {
            // Reset any float error
            bar.localPosition = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(PauseUI.Instance.gameObject.activeSelf)
            {
                PauseUI.Instance.Hide();
            }
            else
            {
                PauseUI.Instance.Show();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            LifeManager.Instance.IncreaseLife(2);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            GameHandler.Instance.HasWon();
        }
    }

    public float GetBarMovespeed()
    {
        return movespeed;
    }

    public void SetBarMovespeed(float newMovespeed)
    {
        movespeed = newMovespeed;
    }
}
