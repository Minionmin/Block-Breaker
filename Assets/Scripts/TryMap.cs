using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TryMap : MonoBehaviour
{
    [SerializeField] private string testMap;

    private void Start()
    {
        if(testMap != "")
        {
            SceneManager.LoadScene(testMap);
        }
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.N))
        {
            Loader.Load(Loader.GetNextLevel());
        }
    }
}
