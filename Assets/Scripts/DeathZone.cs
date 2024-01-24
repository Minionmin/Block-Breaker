using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    public static DeathZone Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Destroy the ball when contact with this deathzone
        DestroyBall();
    }

    /// <summary> Destroy the ball and reset gamestate </summary>
    public void DestroyBall()
    {
        // If ball has Second Life effect, do not decease life and then reset the ball
        var ball = GameHandler.Instance.ball;
        if (ball.hasSecondLife)
        {
            ProcessSecondLife(ball);
            return;
        }

        // Decrease player life by 1
        LifeManager.Instance.DecreaseLife();

        // and see if there is still life left
        if (LifeManager.Instance.GetLife() > 0)
        {
            // Reset game state
            GameHandler.Instance.ToWaitToStartState();
        }
        else if (LifeManager.Instance.GetLife() == 0)
        {
            // If player has no life
            Loader.failedScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(SceneName.GAME_OVER);
        }
    }

    /// <summary> Deactivate Second Life effect and reset the ball </summary>
    private void ProcessSecondLife(Ball ball)
    {
        // Deactivate effect
        ball.LoseSecondLife();
        // Disable player input
        GameHandler.Instance.ToPreparingState();
        // Ball resetting animation
        ball.transform.DOLocalMove(GameHandler.Instance.bar.ballSpawnPoint.position, 1f)
            .onComplete = () =>
            {
                ball.HideSecondLifeVisual();
                GameHandler.Instance.ToWaitToStartState();
            };
        return;
    }
}
