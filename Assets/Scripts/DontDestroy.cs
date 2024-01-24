using UnityEngine;

// Objects that continue to the next scene
public class DontDestroy : MonoBehaviour
{
    public static DontDestroy Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
