using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ArrowUI : MonoBehaviour
{
    public static ArrowUI Instance { get; private set; }

    [SerializeField] private Image arrowImage;

    public Vector3 direction;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (GameHandler.Instance.GetGameState() == GameHandler.States.WaitToStart)
        {
            // for 2d camera angle
            direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // arrow Image rotation
            float arrowImageXAngle = 90f;
            transform.rotation = Quaternion.AngleAxis(-1f * angle + arrowImageXAngle, Vector3.up);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
