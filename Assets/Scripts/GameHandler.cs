using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }

    public enum Gamemode
    {
        Normal,
        Endless
    }

    public enum States
    {
        Preparing,
        WaitToStart,
        InGame,
        HasWon,
        HasLost
    }

    [Header("Normal")]
    [SerializeField, Tooltip("Randomizing block list pattern")]
    private List<GameObject> patterns;

    [SerializeField, Tooltip("Destroy all of these blocks to win the game")] 
    private List<GameObject> blocksList;

    [SerializeField, Tooltip("The position where you are going to spawn the bar")] 
    private Transform spawnPointTransform;

    // Ball and bar references
    private BallStart ball;
    private Transform bar;

    [Header("Endless")]
    // Setup for endless gamemode
    [SerializeField] private Transform blockParentTransform;
    [SerializeField] private SO_Blocks SO_blocktypes;
    private List<Block> blockTypes;

    // Portals for endless level 10+
    [SerializeField] private List<TeleportDoor> portals;

    // Gamestate and Gamemode
    private States state = States.Preparing;
    private Gamemode gamemode;

    public int endlessLevel { get; private set; }
    public int buffCount = 0;

    private TextMeshProUGUI endlessLevelLabel;
    private Image endlessLevelIcon;

    private void Awake()
    {
        Instance = this;

        // Find ball ref
        ball = GameObject.Find("Ball").GetComponent<BallStart>();
        // Find bar ref
        bar = GameObject.Find("BarObject").transform;
    }

    private void Start()
    {
        SetGamemode();

        // Move the bar position to corresponding level's spawn point
        bar.transform.position = spawnPointTransform.position;

        // If gamemode is Endless, then dynamicly generate the blocks
        if (gamemode == Gamemode.Endless)
        {
            // Get block types
            blockTypes = new List<Block>(SO_blocktypes.blocks);

            // Preparing scene
            PauseUI.Instance.Hide();
            FadePlaneUI.Instance.fadeSeq.Restart();
            ClearUI.Instance.Hide();
            NextMatchUI.Instance.Hide();
            BuffUI.Instance.Hide();

            endlessLevelLabel = GameObject.Find("EndlessLevelLabel").GetComponent<TextMeshProUGUI>();
            endlessLevelIcon = GameObject.Find("EndlessLevelIcon").GetComponent<Image>();

            endlessLevel = 1;
            endlessLevelLabel.text = endlessLevel.ToString();

            // All: 6 rows 7 columns
            StartCoroutine(GenerateBlocks(endlessLevel * 5));
        }
        else if (gamemode == Gamemode.Normal)
        {
            // Randomizing pattern
            int patternIndex = Random.Range(0, patterns.Count);
            patterns[patternIndex].SetActive(true);

            // Get blocks into list from the chosen pattern
            foreach (Transform block in patterns[patternIndex].transform)
            {
                blocksList.Add(block.gameObject);
            }

            // Change to waiting state at the beginning of the game
            ToWaitToStartState();
        }
    }

    private void Update()
    {
        switch (state)
        {
            case States.WaitToStart:

                // Update ball position every frame to match the center of the bar
                ball.transform.position = ball.ballSpawnPoint.position;

                // Shoot the ball in the direction of arrow
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
            default:
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
        if(gamemode == Gamemode.Normal)
        {
            // Move to winning state and hide the ball
            ToHasWonState();
            DeactivateBall();
            BuffUI.Instance.Show();
        }
        else if (gamemode == Gamemode.Endless)
        {
            // Move to winning state and reset the ball
            ToHasWonState();
            DeactivateBall();

            // Processing the win + Grant player reward every 3 turn
            endlessLevel++;
            endlessLevelLabel.text = endlessLevel.ToString();
            Sequence endlessIconSeq = DOTween.Sequence(endlessLevelIcon)
                .Append(endlessLevelIcon.transform.DORotate(new Vector3(0.0f, 0.0f, 15.0f), 0.05f, RotateMode.Fast))
                .Append(endlessLevelIcon.transform.DORotate(new Vector3(0.0f, 0.0f, -15.0f), 0.05f, RotateMode.Fast))
                .SetLoops(4, LoopType.Yoyo);
            endlessIconSeq.onComplete = () => { endlessLevelIcon.transform.DORotate(Vector3.zero, 0.1f, RotateMode.Fast); };

            endlessIconSeq.Restart();

            // Generate block again
            int boardWidth = 7;
            int boardHeight = 6;
            int numberOfBlock;

            if (buffCount == 0)
            {
                numberOfBlock = 5;
            }
            else
            {
                numberOfBlock = buffCount * 5;
            }

            // Clamping numbers of block
            if (numberOfBlock > boardWidth * boardHeight)
            {
                numberOfBlock = boardWidth * boardHeight;
                // Add variety by adding noise to numbers of maximum block
                int noise = Random.Range(0, 5);
                numberOfBlock -= noise;
            }

            if (endlessLevel > 10)
            {
                // Make one of the portals available
                int thePortalIndex = Random.Range(0, portals.Count);
                portals.RemoveAt(thePortalIndex);
                portals[0].gameObject.SetActive(true);
            }

            if ((endlessLevel - 1) % 3 == 0)
            {
                // Show buff first, then generate after player selected a buff
                BuffUI.Instance.Show();
            }
            else
            {
                StartCoroutine(GenerateBlocks(numberOfBlock));
            }
        }
    }

    public void ActivateBall()
    {
        ball.gameObject.SetActive(true);
    }

    public void DeactivateBall()
    {
        ball.rb.velocity = Vector3.zero;
        ball.gameObject.SetActive(false);
    }

    public void ToWaitToStartState()
    {
        // Hide all the UI and go to waiting state
        if(FadePlaneUI.Instance.fadeImage.color.a == 1.0f)
        {
            FadePlaneUI.Instance.fadeSeq.Restart();
        }

        PauseUI.Instance.Hide();
        ClearUI.Instance.Hide();
        NextMatchUI.Instance.Hide();
        BuffUI.Instance.Hide();
        ArrowUI.Instance.Show();

        ActivateBall();
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

    private void SetGamemode()
    {
        if (SceneManager.GetActiveScene().name == "Endless")
        {
            gamemode = Gamemode.Endless;
        }
        else
        {
            gamemode = Gamemode.Normal;
        }
    }

    public Gamemode GetGamemode()
    {
        return gamemode;
    }

    public IEnumerator GenerateBlocks(int number)
    {
        List<int> addedPosition = new List<int>();

        int width = 7; // 7 columns per row
        int height = 6; // level has 6 rows
        int blockNumber = width * height;

        // Generate block [number] times
        for(int i = 0; i < number; i++)
        {
            // Random a position to generate a block
            int randomPosition = Random.Range(0, blockNumber);

            // Random again until it's not the same
            while(addedPosition.Contains(randomPosition))
            {
                randomPosition = Random.Range(0, blockNumber);
            }

            int randomBlockType;
            // Do not generate teleport block until level 10+
            if (endlessLevel <= 10)
            {
                // Random type of block to generate
                randomBlockType = Random.Range(0, blockTypes.Count - 1);
            }
            else
            {
                randomBlockType = Random.Range(0, blockTypes.Count);
            }

            // Calculate from 1d array position to 2d array position (Block position is visualized in 2d array)
            int row = randomPosition / width;
            int col = randomPosition % (height + 1);

            // Generate block and add to game handler block list
            GameObject generatedBlock = Instantiate(blockTypes[randomBlockType]).gameObject;
            blocksList.Add(generatedBlock);

            // Put generated block as a child in designed object so the hierachy is more beautiful
            generatedBlock.transform.SetParent(blockParentTransform, false);

            // Move block to their corresponding position
            // Block x range is from -15 to 15 (column)
            // Block z range is from 10 to 15 (row)
            generatedBlock.transform.position = new Vector3((col * 5) - 15, 0, 10 - (row * 5));

            // If randomized block type is armored, random it's durability (clamp at 5)
            if (randomBlockType == 1)
            {
                int durability = Random.Range(2, 6);

                if(generatedBlock.TryGetComponent<ArmoredBlock>(out ArmoredBlock armoredBlock))
                {
                    armoredBlock.SetHit(durability);
                }
            }
            // If randomized block type is teleport, make a specific setup
            else if (randomBlockType == 3)
            {
                if(generatedBlock.TryGetComponent<TeleportBlock>(out TeleportBlock teleportBlock))
                {
                    // TeleportDoor also needs list of teleport blocks
                    portals[0].teleportBlocks.Add(teleportBlock.gameObject);

                    // TeleportBlock need teleportDestination transform
                    teleportBlock.teleportDestination = portals[0].transform.Find("BallTeleportPoint");
                }
            }

            // Generating animation
            generatedBlock.transform.localScale = Vector3.zero;
            generatedBlock.transform.DOScale(new Vector3(5,1,1), 0.1f).SetEase(Ease.OutBack);

            float animationWaitTime = 2.1f / blockNumber;
            yield return new WaitForSeconds(animationWaitTime);

            // Prevent generating block on the same position
            addedPosition.Add(randomPosition);
        }

        // Start game at the end of block generation
        ToWaitToStartState();
    }
}
