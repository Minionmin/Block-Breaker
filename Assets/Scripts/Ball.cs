using DG.Tweening;
using System.Net;
using UnityEngine;

public class Ball : MonoBehaviour
{
    /// <summary> ball's movement speed </summary>
    [SerializeField] private float speed;
    /// <summary> ball's speed when affected by explosion </summary>
    private float explodedSpeed;

    /// <summary> ball's movement direction </summary>
    public Vector3 moveDir { get; private set; }

    /// <summary> Ball collision raycast range </summary>
    [SerializeField] private float raycastRange;

    /// <summary> bar's reference </summary>
    [SerializeField] private GameObject bar;

#region Shader&Material
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material invisibleMaterial;
    #endregion

    #region VFXObject
    /// <summary> Explosion trail VFX </summary>
    [SerializeField] GameObject explodeTrail;
    /// <summary> Second life VFX </summary>
    [SerializeField] GameObject secondLifeBarrier;
    /// <summary> Second life respawning visual </summary>
    [SerializeField] GameObject secondLifeWing;
#endregion

#region EffectFlag
    /// <summary> Has ball hit with explosion block or not </summary>
    public bool hasExploded = false;
    /// <summary> Does ball have Piercing effect or not </summary>
    public bool hasPiercing = false;
    /// <summary> Does ball have Second Life effect or not </summary>
    public bool hasSecondLife = false;
    /// <summary> Does ball have Enlarge effect or not </summary>
    public bool hasEnlarged = false;
#endregion

#region EffectActiveTime
    /// <summary> Current Piercing effect active time </summary>
    public float curPiercingActiveTime = 10f;
    /// <summary> Current Enlarge effect active time </summary>
    public float curEnlargedActiveTime = 10f;
#endregion

#region LayerMask
    /// <summary> bit of the object we hit </summary>
    private int targetLayerMaskBit = -1;

    /// <summary> wall's mask bit </summary>
    [SerializeField] private LayerMask wallLayer;
    /// <summary> deathZone's mask bit </summary>
    [SerializeField] private LayerMask deathZoneLayer;
    /// <summary> block's mask bit </summary>
    [SerializeField] private LayerMask blockLayer;
    /// <summary> bar's mask bit </summary>
    [SerializeField] private LayerMask barLayer;
    /// <summary> boss's mask bit </summary>
    [SerializeField] private LayerMask bossLayer;
#endregion

    void Start()
    {
        explodeTrail.SetActive(false);
        secondLifeBarrier.SetActive(false);
        secondLifeWing.SetActive(false);
    }

    void Update()
    {
        Debug.DrawRay(transform.position, moveDir * raycastRange, Color.green);
        // Since we are using our own method of calculating movement
        // we need to check every frame if the ball is going to hit anything
        if(Physics.Raycast(transform.position, moveDir, out RaycastHit hitInfo, raycastRange))
        {
            // Get GameObject of the object we collided with
            GameObject other = hitInfo.collider.gameObject;

            // Get bit data
            targetLayerMaskBit = other.layer;

            // Reset ball's speed after hitting something else that is not a wall
            if (hasExploded && !IsWall())
            {
                StopExploding();
            }

            // Compare bit data if it's a wall or not
            if (IsWall() || (IsBlock() && !hasPiercing) || IsBoss())
            {
                // If ball has piercing effect, we won't calculate bouncing vector
                moveDir = CalculateWallBouncing(hitInfo);
            }
            // If the ball hit the bar
            else if (IsBar())
            {
                // Calculate bouncing vector when hitting the bar
                CalculateBarBouncing(hitInfo);
            }
            // If the ball hit death zone
            else if(IsDeathZone())
            {
                // Destroy the ball and reset gamestate
                DeathZone.Instance.DestroyBall();
            }

            // After Implementing IHitInterface
            if (other.TryGetComponent<IHitInterface>(out IHitInterface hitable))
            {
                hitable.GetHit();
            }

            // Object might have been destroyed after get hit
            if (other == null) return;
        }
    }

    // Move the ball according to current move direction
    private void LateUpdate()
    {
        // Ball will only move during InGame state
        if (GameHandler.Instance.GetGameState() == GameHandler.States.InGame)
        {
            // If ball is not affected by explosion then move at normal speed
            if(!hasExploded)
            {
                transform.position += moveDir * speed * Time.deltaTime;
            }
            // Ball is affected by explosion and move at increased speed
            else
            {
                transform.position += moveDir * explodedSpeed * Time.deltaTime;
            }
        }
    }

    /// <summary> Calculate a vector to mouse pointer and adjust Arrow UI according to it </summary>
    public Vector3 CalculateBallStartDirectionNormalized()
    {
        Vector3 dir = ArrowUI.Instance.transform.forward;
        dir = new Vector3(dir.x, 0f, dir.z);
        return dir.normalized;
    }

    /// <summary> Calculate a vector when the ball bounces off the wall </summary>
    private Vector3 CalculateWallBouncing(RaycastHit hitInfo)
    {
        // Ball moving in vector
        Vector3 inVec = moveDir * speed;

        // Starting point of the object (In Unity, the bounds min is in the corner)
        // so we have to calculate the mid point of it
        Vector3 w0 = hitInfo.collider.bounds.min;
        // Vector to center point of the object
        Vector3 w = Vector3.zero;

        // Check for shorter side (to adjust the center)
        if ((hitInfo.collider.bounds.max.x - hitInfo.collider.bounds.min.x) < (hitInfo.collider.bounds.max.z - hitInfo.collider.bounds.min.z))
        {
            w0.x = hitInfo.collider.bounds.center.x;
        }
        else
        {
            w0.z = hitInfo.collider.bounds.center.z;
        }

        // Then get the vector to the center point
        w = hitInfo.collider.bounds.center - w0;
        w.y = 0;

        // Get the vector of moveDir in the direction of w (the wall)
        Vector3 k = Vector3.Dot(inVec, w.normalized) * w.normalized;

        // Vector to help us calculate bouncing vector
        Vector3 j = (inVec - k) * -1;

        // Calculate bouncing vector
        Vector3 newMoveDir = inVec + (2 * j);

        #region DebugRay
        //Debug.DrawRay(w0, k, Color.black, 4f);
        //Debug.DrawRay(w0, inVec, Color.red, 4f);
        //Debug.DrawRay(w0 + inVec, j, Color.green, 4f);
        //Debug.DrawRay(w0 + k, moveDir * speed, Color.red, 4f);
        #endregion

        return newMoveDir.normalized;
    }

    /// <summary> Calculate how ball should bounce off the bar </summary>
    private void CalculateBarBouncing(RaycastHit hitInfo)
    {
        // Get the center point of the bar
        Vector3 middlePoint = hitInfo.collider.bounds.center;

        // Get the most left point of the bar
        Vector3 leftPoint = hitInfo.collider.bounds.min;
        leftPoint.z = middlePoint.z;

        // Get the most right point of the bar
        Vector3 rightPoint = hitInfo.collider.bounds.max;
        rightPoint.z = middlePoint.z;

        // We'll use contact point for calculating ratio
        // from the center to the left/right side of the bar
        Vector3 contactPoint = hitInfo.collider.ClosestPointOnBounds(transform.position);
        contactPoint.z = middlePoint.z;

        // Get the vector from center point to contact point
        Vector3 centerToContactPoint = contactPoint - hitInfo.collider.bounds.center;
        // Then get it's distance
        float middleToContact = Vector3.Distance(middlePoint, contactPoint);

        // Multiply z velocity to prevent the ball going too slow in z Direction
        float zMultiplier = 2.5f;

        // Ball is on the left side of the bar
        if (transform.position.x < bar.transform.position.x)
        {
            // Calculate distance from center to the left side
            float middleToLeft = Vector3.Distance(middlePoint, leftPoint);

            // Get the ratio using inverse lerp
            float distanceRatio = Mathf.InverseLerp(0, middleToLeft, middleToContact);

            if (distanceRatio < 0.3) zMultiplier = 3f;

            // Set ball's velocity base on how left the contact point is
            moveDir = new Vector3(-distanceRatio, 0, Mathf.Clamp(distanceRatio * zMultiplier, 0.7f, 1.2f));
        }
        // Ball is on the right side of the bar
        else
        {
            // Calculate distance from center to the right side
            float middleToRight = Vector3.Distance(middlePoint, rightPoint);

            // Get the ratio using inverse lerp
            float distanceRatio = Mathf.InverseLerp(0, middleToRight, middleToContact);

            if (distanceRatio < 0.3) zMultiplier = 3f;

            // Set ball's velocity base on how right the contact point is
            moveDir = new Vector3(distanceRatio, 0, Mathf.Clamp(distanceRatio * zMultiplier, 0.7f, 1.2f));
        }
    }

    /// <summary> Check if the target is a wall or not by using bit mask </summary>
    private bool IsWall()
    {
        return 1 << targetLayerMaskBit == wallLayer.value;
    }

    /// <summary> Check if the target is death zone or not by using bit mask </summary>
    private bool IsDeathZone()
    {
        return 1 << targetLayerMaskBit == deathZoneLayer.value;
    }

    /// <summary> Check if the target is a block or not by using bit mask </summary>
    private bool IsBlock()
    {
        return 1 << targetLayerMaskBit == blockLayer.value;
    }

    /// <summary> Check if the target is a bar or not by using bit mask </summary>
    private bool IsBar()
    {
        return 1 << targetLayerMaskBit == barLayer.value;
    }

    /// <summary> Check if the target is a boss or not by using bit mask </summary>
    private bool IsBoss()
    {
        return 1 << targetLayerMaskBit == bossLayer.value;
    }

    /// <summary> moveDir setter for other classes </summary>
    public void SetMoveDir(Vector3 newMoveDir) { moveDir = newMoveDir; }

    /// <summary> ball speed getter for other classes </summary>
    public float GetBallSpeed() { return speed; }

    /// <summary> ball speed setter for other classes </summary>
    public void SetBallSpeed(float newSpeed) { speed = newSpeed; }

    /// <summary> ball exploded speed setter for other classes </summary>
    public void SetExplodedSpeed(float newSpeed) { explodedSpeed = newSpeed; }

    /// <summary> Ball moves at increased speed with additional VFX </summary>
    /// <param name="newSpeed"> Speed upon explosion (explosionSpeed) </param>
    public void Explode(float newSpeed)
    {
        // Multiply ball speed upon explosion
        SetExplodedSpeed(newSpeed);
        hasExploded = true;
        explodeTrail.SetActive(true);
    }

    /// <summary> Return the ball from exploded state </summary>
    public void StopExploding()
    {
        hasExploded = false;
        explodeTrail.SetActive(false);
    }

    /// <summary> Show Second Life VFX </summary>
    public void GainSecondLife()
    {
        hasSecondLife = true;
        secondLifeBarrier.SetActive(true);
    }

    /// <summary> Hide Second Life VFX </summary>
    public void LoseSecondLife()
    {
        secondLifeBarrier.SetActive(false);

        // Second life visual start animation
        secondLifeWing.SetActive(true);
        secondLifeWing.transform.localScale = Vector3.zero;
        secondLifeWing.transform.DOScale(Vector3.one, 0.5f);
    }

    /// <summary> Hide Second respawn visual</summary>
    public void HideSecondLifeVisual()
    {
        // Set the flag down
        hasSecondLife = false;

        // Second life visual end animation
        secondLifeWing.transform.localScale = Vector3.one;
        secondLifeWing.transform.DOScale(Vector3.zero, 0.5f)
            .onComplete = () => 
            {
                secondLifeWing.SetActive(false);
            };
    }

    /// <summary> Get ball current raycast length </summary>
    public float GetRaycastLength()
    {
        return raycastRange;
    }

    /// <summary> Set new raycast length upon enlarged </summary>
    public void SetRaycastLength(float newLength)
    {
        raycastRange = newLength;
    }

    /// <summary> Show ball outline </summary>
    public void ActivateOutline()
    {
        // Set the first material to outline material
        meshRenderer.material = outlineMaterial;
    }

    /// <summary> Hide ball outline </summary>
    public void DeactivateOutline()
    {
        // Set the first material to invisible material
        meshRenderer.material = invisibleMaterial;
    }
}
