using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnReturn : MonoBehaviour
{
    private void Start()
    {
        NextMatchUI.Instance.OnMainMenuClicked += () => { Destroy(gameObject); };
    }
}
