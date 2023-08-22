using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }

    public enum States
    {
        WaitToStart,
        InGame,
        HasWon,
        HasLost
    }

    [SerializeField] private BallStart ball;
    [SerializeField] private List<GameObject> blocksList;

    private States state;
    private DialogueHandler dialogueHandler;

    private void Awake()
    {
        Instance = this;
        dialogueHandler = GetComponent<DialogueHandler>();
    }

    private void Start()
    {
        state = States.WaitToStart;
    }

    private void Update()
    {
        
        switch (state)
        {
            default:
            case States.WaitToStart:
                ball.transform.position = ball.ballSpawnPoint.position;
                if (ball.rb.velocity == Vector3.zero && Input.GetButtonDown("Jump"))
                {
                    ball.rb.AddForce(ball.START_VECTOR * ball.speed, ForceMode.VelocityChange);
                    ArrowUI.Instance.Hide();
                    GameHandler.Instance.ToInGameState();
                }
                break;
            case States.InGame:
                break;
            case States.HasWon:  
                break;
            case States.HasLost:
                break;
        }
    }

    public void RemoveObject(GameObject obj)
    {
        blocksList.Remove(obj);
        Destroy(obj);
    }

    public int GetBlocksCount()
    {
        return blocksList.Count;
    }

    public bool IsNoBlockLeft()
    {
        return GetBlocksCount() == 0;
    }

    public void HasWon()
    {
        ToHasWonState();
        ball.rb.velocity = Vector3.zero;
        ClearUI.Instance.Show();
        ball.gameObject.SetActive(false);
        StartCoroutine(dialogueHandler.TypeDialogue("Clear!!!", false));
    }

    public void ToWaitToStartState()
    {
        state = States.WaitToStart;
    }

    public void ToInGameState()
    {
        state = States.InGame;
    }

    public void ToHasWonState()
    {
        state = States.HasWon;
    }

    public void ToHasLostState()
    {
        state = States.HasLost;
    }

    public States GetGameState()
    {
        return state;
    }
}
