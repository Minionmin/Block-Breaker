using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Image fadePlane;

    private void Start()
    {
        exitButton.onClick.AddListener(() => { Application.Quit(); });
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            fadePlane.DOFade(1.0f, 0.6f)
                .onComplete = () =>
                {
                    SceneManager.LoadScene(SceneName.MAIN_MENU);
                    Destroy(DontDestroy.Instance.gameObject);
                };
        }
    }
}
