using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public static PauseUI Instance { get; private set; } 

    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Image fadePlane;

    // We will stop the ball when paused
    private float originalBallSpeed = 16f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // In case we pause at the very beginning
        originalBallSpeed = GameHandler.Instance.ball.GetBallSpeed();

        mainMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneName.MAIN_MENU);
            DestroyAllDoNotDestroyOnLoad();
        });

        optionButton.onClick.AddListener(() =>
        {
            OptionUI.Instance.Show();
        });

        // Quit on clicked
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void DestroyAllDoNotDestroyOnLoad()
    {
        Destroy(DontDestroy.Instance.gameObject);
    }

    public void Show()
    {
        var ball = GameHandler.Instance.ball;

        // Fake time stop upon opening menu
        originalBallSpeed = ball.GetBallSpeed();
        ball.SetBallSpeed(0f);

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        var ball = GameHandler.Instance.ball;

        // Return ball speed upon closing Pause UI
        ball.SetBallSpeed(originalBallSpeed);

        OptionUI.Instance.Hide();
        gameObject.SetActive(false);
    }
}
