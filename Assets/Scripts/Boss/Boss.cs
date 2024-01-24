using UnityEngine;
public class Boss : MonoBehaviour, IHitInterface, ITeleportInterface
{
    /// <summary> Boss's current hp </summary>
    [SerializeField] private int bossHP;
    /// <summary> Boss's max hp </summary>
    private int bossMaxHP;

    /// <summary> Teleport VFX </summary>
    [SerializeField] private GameObject teleportEffect;

    /// <summary> Boss's current hp text </summary>
    [SerializeField] private HitUI hitUI;
    /// <summary> Boss's current hp bar </summary>
    [SerializeField] private BarUI hpBarUI;

    /// <summary> Summon's object </summary>
    [SerializeField] private GameObject summonParent;

    /// <summary> Has boss summoned? flag </summary>
    private bool hasSummoned = false;

    private void Start()
    {
        // Put the initialization in Start because it needs to interact with other classes
        Initialize();
    }

    public void GetHit()
    {
        // If boss gets hit
        if (bossHP > 0)
        {
            // Decrease it's life 
            bossHP -= 1;

            // Then teleport ball to random portals
            TeleportTo(GameHandler.Instance.GetTeleportDestination().transform);

            // Camera shake effect when teleported the ball
            CameraManager.Instance.Shake(0.3f, 0.5f, 30);
        }

        // If boss hasn't summoned yet and boss's hp is less than or equal to 10
        if(!hasSummoned && GetHitToDestroy() >= 0 && GetHitToDestroy() <= 10)
        {
            summonParent.SetActive(true);
        }

        // If boss hp less than or equal to 0, destroy boss object
        if (GetHitToDestroy() <= 0)
        {
            DestroyBoss();
        }
        else
        {
            // UI visual doesn't need to know about logic
            // Update boss hp text
            hitUI.SetHitText(bossHP);
            // Update boss bar
            hpBarUI.UpdateSliderUI(bossHP, bossMaxHP);
        }
    }

    /// <summary> When boss hp = 0, destroy this object and move to won state </summary>
    private void DestroyBoss()
    {
        GameHandler.Instance.RemoveObject(gameObject);
        if (GameHandler.Instance.HasNoBlockLeft())
        {
            GameHandler.Instance.HasWon();
        }
    }

    /// <summary> Teleport ball to random portal when boss gets hit </summary>
    public void TeleportTo(Transform targetPortal)
    {
        // Teleport ball
        GameObject teleportVFX = Instantiate(teleportEffect, GameHandler.Instance.ball.transform.position, teleportEffect.transform.rotation);
        GameHandler.Instance.ball.transform.position = targetPortal.position;

        // Teleport effect
        GameObject newObject = Instantiate(teleportEffect, targetPortal.position, teleportEffect.transform.rotation);
        SFXHandler.Instance.PlaySFX(SFXHandler.Instance.sfxSO.teleportSFX, Camera.main.transform.position);
    }

    /// <summary> Initializing boss variables </summary>
    private void Initialize()
    {
        // Hide summons
        summonParent.SetActive(false);

        // Generate portal for this boss gimmick
        GameHandler gameHandler = GameHandler.Instance;
        GameHandler.Instance.GeneratePortal(gameHandler.minPortalXOffset, gameHandler.maxPortalXOffset, gameHandler.maxPortalZOffset, 
            gameHandler.minPortalNumber, gameHandler.maxPortalNumber, false);

        // Initialize boss hp
        bossMaxHP = bossHP;

        // Initialize UI
        hitUI.SetHitText(bossHP);
        hpBarUI.UpdateSliderUI(bossHP, bossMaxHP);

        // Add this boss to the block list
        GameHandler.Instance.AddBlockToList(gameObject, GameHandler.Instance.blockList);
    }

    /// <summary> Return boss current HP (as hit)</summary>
    public int GetHitToDestroy() { return bossHP; }
}
