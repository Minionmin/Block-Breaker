using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    [Header("Game Setup")]
    [SerializeField, Tooltip("Number of block to generate in this level")]
    private int blockNum;

    [SerializeField, Tooltip("Field width")]
    private int fieldWidth;
    [SerializeField, Tooltip("Field height")]
    private int fieldHeight;

    [SerializeField, Tooltip("The position where you are going to spawn the bar")]
    private Transform barSpawnPoint;

    /// <summary> Reference to ball </summary>
    [HideInInspector] public Ball ball;
    /// <summary> Reference to bar </summary>
    [HideInInspector] public Bar bar;
    /// <summary> Reference to bar transform </summary>
    [HideInInspector] public Transform barTransform;

    /// <summary> Parent for the generated blocks </summary>
    [SerializeField] private Transform blockParentTransform;

    /// <summary> Block types that are going to be generated in this level </summary>
    [SerializeField] private SO_Blocks SO_blocktypes;
    private List<Block> blockTypes;

    [Header("Teleportation")]
    [SerializeField, Tooltip("Portal prefab")]
    private GameObject portalSingle;

    [Tooltip("Portals if there is any teleport block")]
    private List<TeleportDoor> portals;
    [Tooltip("Portals that are going to be removed")]
    private List<TeleportDoor> portalToBeRemoved;

    /// <summary> Portal's parent position </summary>
    [SerializeField] private Transform portalParent;

    // Portal's setting
    public float minPortalXOffset;
    public float maxPortalXOffset;
    public float maxPortalZOffset;
    public int minPortalNumber;
    public int maxPortalNumber;

    [Tooltip("Destroy all of these blocks to win the game")]
    public List<GameObject> blockList {  get; private set; }

    [Tooltip("List that manages teleport block")]
    public List<GameObject> tpBlockList { get; private set; }

    // Gamestate and Gamemode
    private States state = States.Preparing;
    private Gamemode gamemode;

    public int endlessLevel { get; private set; }
    public int buffCount { get; private set; }

    private TextMeshProUGUI endlessLevelLabel;
    private Image endlessLevelIcon;

    private void Awake()
    {
        Instance = this;

        // Find ball ref when changing level
        ball = GameObject.Find("Ball").GetComponent<Ball>();
        // Get bar transform from bar ref
        barTransform = GameObject.Find("BarObject").transform;
        // Find bar ref when changing level
        bar = barTransform.GetComponent<Bar>();

        // Initialization
        portals = new List<TeleportDoor>();
        portalToBeRemoved = new List<TeleportDoor>();
        blockList = new List<GameObject>();
        tpBlockList = new List<GameObject>();

        buffCount = 0;
    }

    private void Start()
    {
        // Set gamemode based on the scene name
        SetGamemode();

        // Move the bar position to corresponding level's spawn point
        barTransform.transform.position = barSpawnPoint.position;

        // Also move the ball to bar position
        ball.transform.position = bar.ballSpawnPoint.position;

        // Get block types
        blockTypes = new List<Block>(SO_blocktypes.blocks);

        // If gamemode is Endless, then dynamicly generate the blocks
        if (gamemode == Gamemode.Endless)
        {
            SetUI();

            endlessLevelLabel = GameObject.Find("EndlessLevelLabel").GetComponent<TextMeshProUGUI>();
            endlessLevelIcon = GameObject.Find("EndlessLevelIcon").GetComponent<Image>();

            endlessLevel = 1;
            endlessLevelLabel.text = endlessLevel.ToString();

            StartCoroutine(GenerateBlocks(endlessLevel * 2));
        }
        else if (gamemode == Gamemode.Normal)
        {
            SetUI();

            StartCoroutine(GenerateBlocks(blockNum, fieldWidth, fieldHeight));
        }
    }

    private void Update()
    {
        switch (state)
        {
            case States.WaitToStart:

                // Make the ball follow the pad before launching it
                GetTheBallReady();

                // Launch the ball in the direction of arrow
                if (Input.GetButtonDown("Jump"))
                {
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

    /// <summary> Reposition the ball according to the pad </summary>
    private void GetTheBallReady()
    {
        // Update ball position/launch direction every frame
        ball.transform.position = bar.ballSpawnPoint.position;
        ball.SetMoveDir(ball.CalculateBallStartDirectionNormalized());
    }


    /// <summary> Remove the block from list and destroy the object.
    ///           If there is no block left, the player wins </summary>
    public void DestroyBlock(Block targetBlock, bool isTpBlock = false)
    {
        RemoveObject(targetBlock.gameObject, isTpBlock);

        if (HasNoBlockLeft())
        {
            HasWon();
        }
    }

    /// <summary> Remove target block from list </summary>
    public void RemoveObject(GameObject targetBlock, bool isTpBlock = false)
    {
        // Check for block type
        if (isTpBlock)
        {
            tpBlockList.Remove(targetBlock);

            if (tpBlockList.Count == 0)
            {
                ClearPortals();
            }
        }
        else
        {
            blockList.Remove(targetBlock);
        }

        // After removing block from the list, destroy it
        Destroy(targetBlock);
    }

    /// <summary> Clear all portals if there is no teleport block left 
    /// [Because we are using foreach, changing member in the list will cause error] </summary>
    private void ClearPortals()
    {
        foreach (var portal in portalToBeRemoved)
        {
            portal.PlayTeleportDoorCloseEffect();
            portals.Remove(portal);
            Destroy(portal.gameObject);
        }
    }

    /// <summary> Check for any remaining block </summary>
    public bool HasNoBlockLeft()
    {
        return blockList.Count == 0 &&
            tpBlockList.Count == 0;
    }

    /// <summary> [Normal: Deactivate ball, take buff and proceed to next level]
    ///           [Endless: Deactivate ball, take buff and regenerate blocks] </summary>
    public void HasWon()
    {
        // Reset ball's speed
        ball.SetMoveDir(Vector3.zero);

        if (gamemode == Gamemode.Normal)
        {
            // Move to winning state, hide the ball and clear all the remaining items
            ToHasWonState();
            DeactivateBall();
            ItemManager.Instance.ClearItem();
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
            int boardWidth = 6;
            int boardHeight = 4;
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

    public void ActivateBall() { ball.gameObject.SetActive(true); }
    public void DeactivateBall() 
    {
        // Reset all power up/effect
        ball.hasExploded = false;
        ball.hasPiercing = false;

        // Hide the ball
        ball.gameObject.SetActive(false); 
    }

    /// <summary> Clear all UIs and reset ball direction </summary>
    public void ToWaitToStartState()
    {
        // Show arrow indicator
        ArrowUI.Instance.Show();

        // Reset the ball
        ball.SetMoveDir(Vector3.zero);
        ActivateBall();

        state = States.WaitToStart;
    }

    private void SetUI()
    {
        // Hide all the UI and go to waiting state
        if (FadePlaneUI.Instance.fadeImage.color.a == 1.0f)
        {
            FadePlaneUI.Instance.fadeSeq.Restart();
        }

        PauseUI.Instance.Hide();
        ClearUI.Instance.Hide();
        NextMatchUI.Instance.Hide();
        BuffUI.Instance.Hide();
        ArrowUI.Instance.Show();
        if(gamemode == Gamemode.Normal)
        {
            NormalLevelUI.Instance.PlayLevelStartAnimation();
        }
        else
        {
            NormalLevelUI.Instance.Hide();
        }
    }

    public void ToPreparingState() { state = States.Preparing; }
    public void ToInGameState() { state = States.InGame; }
    public void ToHasWonState() { state = States.HasWon; }
    public void ToHasLostState() { state = States.HasLost; }
    public States GetGameState() { return state; }

    /// <summary> Set gamemode that player chose in MainMenu </summary>
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

    /// <summary> Set current gamemode </summary>
    public Gamemode GetGamemode() { return gamemode; }

    /// <summary> Add target block to the list </summary>
    public void AddBlockToList(GameObject targetBlock, List<GameObject> targetList)
    {
        targetList.Add(targetBlock);
    }

    /// <summary> Generate blocks at random position </summary>
    public IEnumerator GenerateBlocks(int number, int width_in = 0, int height_in = 0)
    {
        int width, height;
        float blockWidth, blockHeight;
        List<int> addedPosition = new List<int>();

        blockWidth = 6f;
        blockHeight = 2f;

        if (gamemode == Gamemode.Endless)
        {
            // In Endless
            width = 6; // 6 columns per row
            height = 4; // level has 4 rows
        }
        else
        {
            // In normal, we use the parameter values instead
            width = width_in;
            height = height_in;
        }

        int blockNumber = width * height;
        // Prevent setting error (number of block exceeds the total of width x height)
        if(number > blockNumber) number = blockNumber;

        // Generate block [number] times
        for (int i = 0; i < number; i++)
        {
            // Random a position to generate a block
            int randomPosition = Random.Range(0, blockNumber);

            // Random again until it's not the same
            while (addedPosition.Contains(randomPosition))
            {
                randomPosition = Random.Range(0, blockNumber);
            }
            // Prevent generating block on the same position
            addedPosition.Add(randomPosition);

            // Type of block that can be placed in this level
            int randomBlockType;
            if(gamemode == Gamemode.Endless && endlessLevel < 10)
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
            int col = randomPosition % width;

            // Generate the block
            GameObject generatedBlock = Instantiate(blockTypes[randomBlockType]).gameObject;

            // Put generated block as a child in designed object so the hierachy is more beautiful
            generatedBlock.transform.SetParent(blockParentTransform, false);

            // Move block to their corresponding position
            // Block x range is from -15 to 15 (column)
            // Block z range is from 10 to 15 (row)
            generatedBlock.transform.position = new Vector3((col * blockWidth) - 15, 0, 10 - (row * blockHeight));

            // If randomized block type is armored, random it's durability (clamp at 5)
            if (randomBlockType == 1)
            {
                int durability = Random.Range(2, 5);

                if (generatedBlock.TryGetComponent<ArmoredBlock>(out ArmoredBlock armoredBlock))
                {
                    armoredBlock.SetHit(durability);
                }
            }

            // If the block is teleport block then add it to the other list
            if (randomBlockType == 3) { AddBlockToList(generatedBlock, tpBlockList); }
            else { AddBlockToList(generatedBlock, blockList); }

            // Generating animation
            generatedBlock.transform.localScale = Vector3.zero;
            generatedBlock.transform.DOScale(new Vector3(5, 1, 1), 0.1f).SetEase(Ease.OutBack);

            float animationWaitTime = 2.1f / blockNumber;
            yield return new WaitForSeconds(animationWaitTime);
        }

        // If there is any teleport block, generate portal(s)
        if (tpBlockList.Count > 0)
        {
            GeneratePortal(minPortalXOffset, maxPortalXOffset, maxPortalZOffset, minPortalNumber, maxPortalNumber);
        }

        // Start game at the end of block generation
        ToWaitToStartState();
    }

    /// <summary> Increase buff count by 1 </summary>
    public void IncrementBuffCount() { buffCount++; }

    /// <summary> Generate portal(s) at random position if there is any teleport block </summary>
    public void GeneratePortal(float minXOffset, float maxXOffset, float maxZOffset, int minNum = 1, int maxNum = 1, bool shouldPreventStacking = true)
    {
        // Number of portal based on maximum number we can possibly generate
        int num = Random.Range(minNum, maxNum + 1);
        float xOffset, zOffset;

        // For checking if the portals are stacking or not
        var portalPosition = new List<KeyValuePair<float, float>>();

        for (int i = 0; i < num; i++)
        {
            // Random x offset and z offset for portal
            xOffset = Random.Range(minXOffset, maxXOffset);
            zOffset = Random.Range(0, maxZOffset);

            // Random offset sign
            if (Random.Range(1, 3) % 2 == 0)
            {
                xOffset = -xOffset;
            }

            // Should prevent portals from stacking or not
            if(shouldPreventStacking)
            {
                // This method will always create at least 1 portal
                if (portalPosition.Count > 0)
                {
                    bool isStacked = false;
                    foreach (var val in portalPosition)
                    {
                        // Distance from center point to the existed portal
                        float toExistedPortal = Mathf.Pow(val.Key, 2) + Mathf.Pow(val.Value, 2);
                        // Distance from center point to the new portal
                        float fromCurrentPortal = Mathf.Pow(xOffset, 2) + Mathf.Pow(zOffset, 2);
                        // Distance between both portals
                        float distance = Mathf.Abs(toExistedPortal - fromCurrentPortal);

                        // Distance between each portal, can be adjusted freely
                        if (distance <= 18f)
                        {
                            // If distance between portal is less than what we want, do not create it
                            isStacked = true;
                        }
                    }
                    if (isStacked) continue;
                }
                portalPosition.Add(new KeyValuePair<float, float>(xOffset, zOffset));
            }


            // Set portal position after randomizing offset
            var portalInst = Instantiate(portalSingle, portalParent);
            portalInst.transform.localPosition = new Vector3(portalInst.transform.localPosition.x + xOffset, 0f, portalInst.transform.localPosition.z + zOffset);

            // Add the generated portal to the list so we can manage them later
            var portal = portalInst.GetComponent<TeleportDoor>();
            portals.Add(portal);
            portalToBeRemoved.Add(portal);
        }
    }

    /// <summary> Get a portal at random </summary>
    public TeleportDoor GetTeleportDestination()
    {
        int index = Random.Range(0, portals.Count);
        return portals[index];
    }
}