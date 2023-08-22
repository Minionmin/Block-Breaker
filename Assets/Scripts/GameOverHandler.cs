using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    private const string gameScene = "SampleScene";

    [SerializeField] private Button exitButton;

    private void Start()
    {
        exitButton.onClick.AddListener(() => { Application.Quit(); });
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            Loader.LoadFailedScene();
        }
    }
}
