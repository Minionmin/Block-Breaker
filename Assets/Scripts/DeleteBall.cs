using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteBall : MonoBehaviour
{
    private const string gameOverScene = "GameOverScene";

    private void OnCollisionEnter(Collision collision)
    {
        LifeManager.Instance.DecreaseLife();

        if (LifeManager.Instance.GetLife() > 0)
        {
            // see if it is the ball or not
            if(collision.gameObject.TryGetComponent<BallStart>(out BallStart ball))
            {
                ball.rb.velocity = Vector3.zero;
                ball.transform.position = ball.ballSpawnPoint.position;
                ArrowUI.Instance.Show();
                GameHandler.Instance.ToWaitToStartState();
            }
        }
        else if(LifeManager.Instance.GetLife() == 0)
        {
            Loader.failedScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(gameOverScene);
        }
        else
        {
            // do nothing
        }

        LifeManager.Instance.UpdateVisual();
    }

}
