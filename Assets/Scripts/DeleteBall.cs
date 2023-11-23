/**********************************************
 * 
 *  DeleteBall.cs 
 *  The bottom part of the stage
 * 
 *  製作者：Phansuwan Chaichumphon （ミン）
 * 
 **********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteBall : MonoBehaviour
{
    private const string gameOverScene = "GameOverScene";

    private void OnCollisionEnter(Collision collision)
    {
        // Decrease player life by 1
        LifeManager.Instance.DecreaseLife();

        // and see if there is still life left
        if (LifeManager.Instance.GetLife() > 0)
        {
            // See if it is the ball or not
            if(collision.gameObject.TryGetComponent<BallStart>(out BallStart ball))
            {
                // Reset the ball
                ball.rb.velocity = Vector3.zero;
                ball.transform.position = ball.ballSpawnPoint.position;

                // Show shooting arrow
                ArrowUI.Instance.Show();

                // Reset game state
                GameHandler.Instance.ToWaitToStartState();
            }
        }
        else if(LifeManager.Instance.GetLife() == 0)
        {
            // If player has no life
            Loader.failedScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(gameOverScene);
        }
    }

}
