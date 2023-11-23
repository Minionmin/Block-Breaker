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
            fadePlane.DOFade(1.0f, 1.0f)
                .onComplete = () =>
                {
                    SceneManager.LoadScene(Loader.LoaderScene.MainMenu.ToString());
                    DontDestroy doNotDestroyParent = GameObject.Find("Do Not Destroy").GetComponent<DontDestroy>();
                    Destroy(doNotDestroyParent.gameObject);
                };
        }
    }
}
