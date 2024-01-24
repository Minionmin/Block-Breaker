using UnityEngine;

public class Bar : MonoBehaviour
{
    public static Bar Instance {  get; private set; }

    /// <summary> Bar's movement speed </summary>
    [SerializeField] private float movespeed;
    /// <summary> Wall layer mask bit for limiting player's movement </summary>
    [SerializeField] private LayerMask wallLayerMask;
    /// <summary> Reference to bar's visual </summary>
    [SerializeField] private Transform bar;

    /// <summary> Ball's spawn point </summary>
    public Transform ballSpawnPoint;

    /// <summary> Moving direction </summary>
    private Vector3 moveDir;
    /// <summary> Movement speed multiplied by delta time </summary>
    private float moveDistance;
    /// <summary> Is the bar bumping into the wall or not </summary>
    private bool canMove;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // If not in wait state or game state then does not take any input
        if (!CanInput()) return;

        // Calculating movable distance
        moveDistance = movespeed * Time.deltaTime;

        // Get the direction from input
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);

        // Raycast distance based on bar's size(scale)
        float castDistance = bar.transform.localScale.x / 2.0f;
        canMove = !Physics.Raycast(transform.position, moveDir, castDistance, wallLayerMask);
        
        // Pausing/Unpausing button
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }

        // Cheat codes
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LifeManager.Instance.IncreaseLife(2);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            GameHandler.Instance.HasWon();
        }
    }

    private void LateUpdate()
    {
        if (canMove && CanInput())
        {
            transform.position += moveDir * moveDistance;
        }
        else
        {
            // Reset any float error
            bar.localPosition = Vector3.zero;
        }
    }

    /// <summary> Pause & Unpause the game </summary>
    private static void PauseUnpause()
    {
        if (PauseUI.Instance.gameObject.activeSelf)
        {
            PauseUI.Instance.Hide();
        }
        else
        {
            PauseUI.Instance.Show();
        }
    }

    public float GetBarMovespeed() { return movespeed; }
    public void SetBarMovespeed(float newMovespeed) { movespeed = newMovespeed; }

    private bool CanInput()
    {
        return GameHandler.Instance.GetGameState() == GameHandler.States.WaitToStart ||
            GameHandler.Instance.GetGameState() == GameHandler.States.InGame;
    }
}
